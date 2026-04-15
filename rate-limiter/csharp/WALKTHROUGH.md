# Rate Limiter — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through nineteen red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Rule (readonly record struct)        — (MaxRequests, Window), validated at construction
Decision (abstract record)
   ├── Allowed
   └── Rejected(RetryAfter)

Limiter
   ├── Request(key) : Decision        — single mutating operation
   └── private Dictionary<string, WindowState>   — (windowStart, count) per key

Collaborator: IClock.Now() — injected; FixedClock in tests
Defaults: DefaultMaxRequests = 100, DefaultWindowDuration = 60 seconds
```

## Why Fixed Window, Not Sliding Window

The kata brief describes a **sliding window** algorithm — at t=11, a request made at t=3 (eight seconds ago) is still counted because it falls within the last ten seconds. That's the most accurate algorithm and what you'd want in production when strictness matters.

This reference ships **fixed window** instead. The reasoning:

1. **Teaching clarity.** Fixed window is a two-state-variable algorithm: `(windowStart, count)` per key. Every behavior — allow, reject, reset, retryAfter — collapses to three comparisons against those two values. A reader can hold the whole state machine in their head before looking at any test.
2. **Scenario coverage is identical.** Every teaching point the spec lists — *allow under limit, reject at limit, retryAfter on rejection, window expiry resets count, keys isolated, invalid rule rejected* — is exercisable with fixed window. The only thing that changes is the exact timing of the "allowed again" moment.
3. **Sliding window is a stretch.** Its shape is `Deque<DateTime>` per key plus a prune-on-every-call sweep; a reader ready for that should recognize the fixed-window code and implement sliding as a drop-in replacement of the `WindowState` struct. That's a cleaner next-step exercise than scaffolding the sliding version as the reference.

The end-to-end worked example in `WorkedExampleTests.cs` demonstrates the fixed-window sequence verbatim: alice allowed at t=0,1,2; rejected at t=3 with `retryAfter=7s`; bob independent; at t=10 the window resets cleanly.

See `src/RateLimiter/Limiter.cs`.

## Why `Decision` Is a Union (`Allowed | Rejected(RetryAfter)`)

A boolean return — `true` or `false` — forces the caller to ask a second question ("if false, how long?") through a separate channel. Returning a `bool` plus an `out TimeSpan? retryAfter` parameter works in C#, but it's a code smell the moment another language tries to mirror it.

A **sealed record hierarchy** (`Decision.Allowed`, `Decision.Rejected(TimeSpan)`) carries the information together. The rejection *is* the duration; the allowance has none. Pattern-matching at the call site reads as "what happened, and what do we know about it" — which is exactly the domain vocabulary. The TypeScript implementation uses a discriminated union with the same shape; the Python implementation uses two `@dataclass(frozen=True)` subtypes.

See `src/RateLimiter/Decision.cs`.

## Why `Rule` Is a Value Type, Not Two Constructor Parameters on `Limiter`

Every validation the limiter cares about lives in the rule: `maxRequests > 0`, `window > 0`. Packaging them as `Rule(maxRequests, window)` does three things:

1. **Validation lives with the value.** `new Rule(0, …)` throws; the limiter never sees an invalid configuration because the invalid value never becomes a `Rule` in the first place. The five rule-construction scenarios exercise the rule directly — they don't need a `Limiter` to exist.
2. **Named domain concept.** "Rule" is the word a policy document would use ("the rule is 100 req/min"). Builders and tests pick up the noun without extra ceremony.
3. **Structural equality.** `readonly record struct` gives value equality for free; two limiters configured with `new Rule(3, TenSeconds)` compare equal for their configuration.

See `src/RateLimiter/Rule.cs`.

## Why `IClock`, Not `DateTime.UtcNow`

Eight scenarios pivot on time elapsing: retryAfter as remaining window duration, decreasing retryAfter as clock advances, window boundary crossings, alice-and-bob's independently-starting windows. If those tests called `DateTime.UtcNow` directly, they'd either sleep (slow, flaky) or thread a stopwatch into every assertion.

`IClock.Now()` makes the collaboration explicit. `FixedClock` in the test project is the deterministic fake — not a mocking-library mock, just a tiny `IClock` that remembers the time the test set. The tests read "request at t=0, advance three seconds, request again with retryAfter=7s" instead of "sleep then hope." That's the same pattern used in [`memory-cache`](../../memory-cache/csharp/WALKTHROUGH.md#why-iclock-not-datetimeutcnow) and [`circuit-breaker`](../../circuit-breaker/csharp/WALKTHROUGH.md).

Inside `Limiter.Request`, the clock is read **exactly once** per call. Every comparison in the method uses the same `now`. Reading twice would be a latent bug: the boundary check and the retryAfter math could disagree if the clock ticked between them.

See `src/RateLimiter/IClock.cs` and `tests/RateLimiter.Tests/FixedClock.cs`.

## Why `LimiterBuilder` Returns a Tuple

Most scenarios need to **advance time after the limiter exists** — three requests, jump forward three seconds, one rejection with a specific retryAfter. If the builder only returned the limiter, the test would have no handle on the clock. Returning `(Limiter, FixedClock)` gives the test exactly the two collaborators it drives, without reaching into private fields.

The builder also defaults `maxRequests` and `window` to the domain constants, so scenarios that only care about one dimension don't have to spell out the other. `.WithMaxRequests(3).WithWindow(TenSeconds).Build()` is the line most tests want to write.

See `tests/RateLimiter.Tests/LimiterBuilder.cs`.

## Why Per-Key State Is a Private Dictionary

"Keys are isolated" is a first-class domain rule, but the *storage* is a private implementation detail. A `Dictionary<string, WindowState>` gives O(1) lookup and O(1) insert — the only two operations `Request` needs. No external caller sees it; there's no `GetCount(key)` on the API because every scenario is expressible through `Request` itself.

`WindowState` is a `record struct` (value semantics, no identity) containing `(Start, Count)`. Updating it is a `with`-expression that replaces the dictionary entry — no mutation of existing structs, no aliasing risk.

See `src/RateLimiter/Limiter.cs`.

## Why Domain Exception Type (`LimiterRuleInvalidException`)

A zero or negative `maxRequests` does not form a rule. Neither does a zero window. Throwing `ArgumentException` would blur those two failures with "some other argument was wrong somewhere." A named exception puts the domain rule in the type system: tests `Should().Throw<LimiterRuleInvalidException>()`, and a reader sees the rule invariant named in the stack trace.

The messages — `"Max requests must be positive"`, `"Window must be positive"` — are **byte-identical** across the three languages. Only the exception class differs (C# `...Exception`, TS `...Error`, Python error class).

See `src/RateLimiter/Exceptions.cs`.

## Why Five Test Files

The nineteen scenarios split naturally into five concerns:

- `RuleTests.cs` — rule construction and validation (scenarios 1–5)
- `AllowAndRejectTests.cs` — core allow/reject behavior and retryAfter math (scenarios 6–12)
- `WindowExpiryTests.cs` — boundary and reset behavior (scenarios 13–15)
- `KeyIsolationTests.cs` — per-key independence (scenarios 16–18)
- `WorkedExampleTests.cs` — the end-to-end scenario (scenario 19)

One `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## What's Deliberately Not Modeled

The kata brief's bonus section asks for the **token bucket** algorithm, `remainingRequests(clientId)`, `retryAfter(clientId)` as a standalone query, burst allowance, and tiered limits. This reference scopes to the nineteen core scenarios of the single fixed-window strategy — every line of domain code earns its keep against a named test. A reader extending the kata has the `Rule` and `Decision` types ready to grow (tiered limits = a `Rule` per tier; token bucket = a different `WindowState` shape) without the reference hiding the seams under premature abstraction.

## How to Run

```bash
cd rate-limiter/csharp
dotnet test
```
