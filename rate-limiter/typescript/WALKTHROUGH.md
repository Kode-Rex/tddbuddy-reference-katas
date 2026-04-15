# Rate Limiter — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md). This file notes what's idiomatic to TypeScript and what deliberately diverges.

## Idiomatic Deltas

- **`Decision` is a discriminated union**, not a sealed record hierarchy. `{ kind: 'allowed' } | { kind: 'rejected'; retryAfterMs: number }` is the TS idiom; a switch on `kind` exhaustively narrows the type. Factory functions `allowed()` and `rejected(ms)` keep construction readable at call sites.
- **`Rule` is a plain class**, not a `readonly record struct`. TS lacks value-type semantics, but the validation-at-construction pattern is identical — `new Rule(0, 10_000)` throws before any `Limiter` sees it.
- **`windowMs: number`** instead of a `TimeSpan`. JavaScript's native time unit is milliseconds (epoch ms, `Date#getTime()`), and introducing a custom `Duration` type would be ceremony without payoff at this scale. The domain constant is named `DEFAULT_WINDOW_MS = 60_000`.
- **Per-key state mutates in place** — `state.count += 1` — where the C# version uses `with` to produce a new record. JavaScript has no immutable value structs, and the `Map<string, WindowState>` is already a private implementation detail. Mutating the state object is idiomatic and cheap.
- **`retryAfterMs`** is a number on the rejected variant, matching the rest of the TS API's millisecond discipline. Consumers receiving this value typically feed it directly to a `setTimeout` or an HTTP `Retry-After` header (after dividing by 1000).

## Shared Design (see C# walkthrough for rationale)

- Fixed-window algorithm, not sliding window.
- `Clock` collaborator + `FixedClock` test double.
- `LimiterBuilder` returns `{ limiter, clock }` so tests drive both.
- `LimiterRuleInvalidError` with byte-identical messages: `"Max requests must be positive"`, `"Window must be positive"`.
- Five test files mirroring the C# split: `rule.test.ts`, `allowAndReject.test.ts`, `windowExpiry.test.ts`, `keyIsolation.test.ts`, `workedExample.test.ts`.

## How to Run

```bash
cd rate-limiter/typescript
npm install
npx vitest run
```
