# Circuit Breaker

A resilience-pattern aggregate that wraps a fallible operation and trips open after a configurable number of consecutive failures, fails fast while open, and probes recovery through a half-open state after a timeout elapses. Great for practicing **state-machine modelling**, **injected clocks**, and **collaboration over a callable operation** with test data builders.

## What this kata teaches

- **Test Data Builders** — `BreakerBuilder().WithThreshold(n).WithTimeout(...).WithClock(clock).Build()` returns a `(Breaker, Clock)` tuple so tests drive transitions deterministically.
- **Injected Clock** — `Clock` is a collaborator interface; `FixedClock` in tests lets `Open → HalfOpen` timeout tests advance simulated wall-time without sleeping.
- **Operation as Callable** — `execute(operation)` takes a `Func<T>` / `() => T` / `Callable[[], T]`, not a pre-computed value. The breaker needs to *observe the call* — catch its exception to count a failure, or decline to invoke it when open. A pre-computed result would strip that observation away.
- **State as an Enum** — `BreakerState` is a three-value enum (`Closed`, `Open`, `HalfOpen`). The transition logic lives in the `Breaker` aggregate, not in state-object subclasses; the state machine is small enough that a flat `switch` reads clearer than a Gang-of-Four State pattern here.
- **Domain Exception Types** — `CircuitOpenException` / `CircuitOpenError` names the rejection when the breaker refuses to call the underlying operation, rather than throwing a generic `InvalidOperationException` / `Error` / `RuntimeError`.
- **Generic Result Type** — `Breaker` is polymorphic over the operation's return type so callers can wrap any `T`-producing function without the domain leaking concrete types.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
