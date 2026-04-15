# Circuit Breaker — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Breaker ──owns──> BreakerState  (Closed | Open | HalfOpen)
   │    ──owns──> int _consecutiveFailures
   │    ──owns──> DateTime _openedAt
   │
   ├── Execute<T>(Func<T> operation) : T
   │     — Closed:   invoke; count failure; trip at threshold
   │     — Open:     if timeout elapsed → HalfOpen + probe; else throw CircuitOpenException
   │     — HalfOpen: success → Closed + reset; failure → Open + restart timeout
   └── State : BreakerState

Collaborator: IClock.Now() — injected; FixedClock in tests
Defaults: DefaultFailureThreshold = 5, DefaultResetTimeout = 30 seconds
```

## Why `Execute` Takes a `Func<T>`, Not a Result

The most load-bearing design choice is that `Execute` accepts the **operation**, not its result. Three reasons:

1. **The breaker needs to observe the call.** When `Open`, the operation must *not be invoked at all*. If a caller pre-computed the result and passed it in, the whole fail-fast guarantee — "the unreliable downstream is shielded from traffic when tripped" — is already gone by the time the breaker sees it.
2. **Failure is an exception, not a return.** The kata counts "failure" as the operation throwing. The breaker wraps `try/catch` around the invocation itself, increments the counter in the `catch`, and re-throws. A result-based API would need a discriminated "success or failure" container and would paper over the natural contract of most production calls (HTTP clients, DB drivers — they throw).
3. **Laziness is cheap.** `Func<T>` is the zero-cost lambda shape in C#; callers write `breaker.Execute(() => client.Fetch(id))` without constructing any extra type.

See `Breaker.Execute` in `src/CircuitBreaker/Breaker.cs`. Each test's operation is a single-line lambda.

## Why `BreakerState` Is an Enum, Not a State-Object Hierarchy

The Gang-of-Four State pattern would model `Closed`, `Open`, `HalfOpen` as three classes, each with its own `Execute` implementation, and the `Breaker` would delegate to the current state object. That's the shape most compelling for *extensible* state machines — new states plug in as new classes without editing the existing ones.

This state machine has three states, full stop, with transitions fully specified by the kata brief. An enum and a single `Execute` method read better for three reasons:

- The whole state machine fits on one screen. A reader sees `Closed → Open → HalfOpen → Closed` by following the code top-to-bottom.
- Transition rules depend on **fields shared across states** (`_consecutiveFailures`, `_openedAt`). A state-object hierarchy would either hand those fields back and forth between state instances or make them `Breaker` fields that the state reaches back into — either way the "encapsulation" claim of the State pattern leaks.
- The kata spec tests **the aggregate**, not the individual states. `BreakerState` showing up in the type signature of `State` (and in one assertion per scenario) is the entire surface a caller sees.

If the state machine grew (configurable per-state hooks, on-enter/on-exit callbacks, multiple probe quotas), the State pattern would pay for itself. For the kata's scope, the enum wins.

See `src/CircuitBreaker/BreakerState.cs` and `src/CircuitBreaker/Breaker.cs`.

## Why `IClock`, Not `DateTime.UtcNow`

Eight scenarios pivot on time elapsing: `Open → HalfOpen` after 30 s, failed probe restarting the timeout, round-trip cycles. If those tests called `DateTime.UtcNow` directly, they'd either sleep (slow, flaky) or thread a stopwatch into every assertion.

`IClock.Now()` makes the collaboration explicit. `FixedClock` in the test project is the deterministic fake — not a mocking-library mock, just a tiny `IClock` that remembers the time the test set. The tests read "trip the breaker, advance 30 seconds, execute now probes" instead of "sleep then hope." That's the **Mocks as Behavioral Specifications** principle: when collaboration *is* part of the behavior, make it an interface.

See `src/CircuitBreaker/IClock.cs` and `tests/CircuitBreaker.Tests/FixedClock.cs`.

## Why `BreakerBuilder` Returns a Tuple

Most transition tests need to **advance time after the breaker exists** — trip it now, jump forward 30 seconds, assert the next call probes. If the builder only returned the breaker, the test would have no handle on the clock. Returning `(Breaker, FixedClock)` gives the test exactly the two collaborators it drives, without reaching into private fields.

The builder also defaults threshold and timeout to the domain constants, so scenarios that only care about *one* dimension (say, HalfOpen recovery) don't have to spell out the threshold too. `.WithTimeout(TimeSpan.FromSeconds(30)).Build()` is the line a timeout test wants to write.

See `tests/CircuitBreaker.Tests/BreakerBuilder.cs`.

## Why the `Open → HalfOpen` Check Lives on the Next Execute, Not a Background Tick

Expiry could be modelled two ways:

1. **Background tick** — a scheduled callback that flips `Open → HalfOpen` when the timeout elapses.
2. **Lazy check on the next `Execute`** — inspect `clock.Now() - _openedAt >= _resetTimeout` at the top of every call and transition then.

This implementation picks (2) — the same shape that memory-cache's [`Get` uses for lazy TTL expiry](../../memory-cache/csharp/WALKTHROUGH.md#why-evictexpired-is-an-explicit-sweep). A breaker has no work to do between calls; spinning a timer just to flip a flag adds threading concerns (cancellation, disposal) to a domain object that otherwise has none. The lazy check keeps `Breaker` single-threaded and the tests deterministic: "advance the clock, call execute, observe the probe."

The tradeoff is that `State` inspected *without* calling `Execute` will still read `Open` after the timeout elapses — the transition hasn't happened yet. The scenarios acknowledge this: `State` is checked after calls, not between them. In a production version where `State` needed to be precise for a health endpoint, the same lazy check would move into the `State` getter.

See `Breaker.Execute` top-of-method.

## Why Domain Exception Types

- **`BreakerThresholdInvalidException`** / **`BreakerTimeoutInvalidException`** — a zero or negative threshold doesn't form a breaker, nor does a zero timeout. Throwing `ArgumentException` would blur those two failures with "some other argument was wrong somewhere."
- **`CircuitOpenException`** — thrown by `Execute` when the breaker is tripped. A caller catches this *specific* type to route into a fallback path; catching `InvalidOperationException` would also catch bugs inside the operation itself and make both indistinguishable.

The messages — `"Failure threshold must be positive"`, `"Reset timeout must be positive"`, `"Circuit is open"` — are **byte-identical** across the three languages. Only the exception class differs (C# `...Exception`, TS `...Error`, Python error class).

See `src/CircuitBreaker/Exceptions.cs`.

## Why Five Test Files

The twenty scenarios split naturally into five concerns:

- `ConstructionTests.cs` — initial state, invalid threshold/timeout (scenarios 1–3)
- `ClosedStateTests.cs` — success, re-throw, counter behaviour, trip at threshold (scenarios 4–9)
- `OpenStateTests.cs` — fail-fast, state stays Open, message (scenarios 10–12)
- `TransitionTests.cs` — before/after timeout, HalfOpen → Closed, HalfOpen → Open, restart timeout (scenarios 13–18)
- `RoundTripTests.cs` — end-to-end cycles from the kata brief example (scenarios 19–20)

One `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## What's Deliberately Not Modelled

The kata brief's bonus section asks for `onStateChange` callbacks, a `getMetrics` snapshot, a half-open quota > 1, and exponential backoff on successive trip cycles. This reference scopes to the twenty core scenarios — every line of domain code earns its keep against a named test. The lazy-timeout check is the hook where a state-change callback would slot in; the quota > 1 would add a `_halfOpenProbesAllowed` counter; backoff would multiply `_resetTimeout` by `2^trip_count`. All are sensible extensions; all are application choices, not domain ones.

## How to Run

```bash
cd circuit-breaker/csharp
dotnet test
```
