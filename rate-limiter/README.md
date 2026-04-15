# Rate Limiter

A per-key request limiter that allows up to a configured number of requests in a rolling window and reports a `retryAfter` duration when it rejects. Great for practicing **injected clocks**, **domain decision types (Allowed / Rejected)**, and **per-key state isolation** with test data builders.

## What this kata teaches

- **Test Data Builders** — `LimiterBuilder().WithMaxRequests(n).WithWindow(...).WithClock(clock).Build()` returns a `(Limiter, Clock)` tuple so tests drive request timestamps deterministically.
- **Injected Clock** — `Clock` is a collaborator interface; `FixedClock` in tests lets window-expiry tests advance simulated wall-time without sleeping.
- **Decision as a Union / Sealed Hierarchy** — `Decision` is either `Allowed` or `Rejected(retryAfter)`. Callers pattern-match the outcome; `retryAfter` is carried *by* the rejection rather than stashed on a side channel.
- **Rule as a Value Type** — `Rule(maxRequests, window)` names the configuration; validation lives with the value, not scattered through the aggregate.
- **Fixed-Window Algorithm** — the simplest of the canonical strategies: each key has a window `[windowStart, windowStart + window)` and a count; the first request past the boundary opens a fresh window. Easy to explain, easy to test, and the one the spec's worked example matches scenario-for-scenario.
- **Domain Exception Type** — `LimiterRuleInvalidException` names the rejection for invalid rule config rather than throwing a generic `ArgumentException` / `Error` / `ValueError`.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
