# Circuit Breaker — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (operation as callable, enum-based state machine, injected clock, lazy Open→HalfOpen transition, named domain exceptions).

This note captures only the TypeScript deltas.

## `Clock` Is Structural

TypeScript doesn't need a class to implement the `Clock` interface — any object with a `now(): Date` method satisfies it. `FixedClock` in the tests is a class for the convenience of `advanceMs` / `advanceTo`, but consumers of `Breaker` could pass `{ now: () => new Date() }` at the call site. Interfaces in TS describe shape, not inheritance.

See `src/Clock.ts` and `tests/FixedClock.ts`.

## Timeout as Milliseconds

`Date.getTime()` returns epoch ms; comparing open-time to `now` as `number - number >= timeoutMs` is the clean shape. The C# version keeps the ergonomic `TimeSpan` type; TS pays for an idiomatic-ms choice with clearer arithmetic. The constant `DEFAULT_RESET_TIMEOUT_MS = 30_000` makes the unit explicit at every use site.

## `BreakerState` as a String-Valued Enum

TypeScript's numeric enums would make `breaker.state === 0` type-check and silently mean `Closed` — too easy to fat-finger. A string-valued enum (`Closed = 'Closed'`) gives the same ergonomics in tests (`expect(breaker.state).toBe(BreakerState.Closed)`) with debug-time readable values and no positional coupling.

See `src/BreakerState.ts`.

## `execute<T>(operation: () => T): T`

The callable shape is the same as C# — `() => T` instead of `Func<T>`. The generic `T` flows through so the caller's type isn't erased; `breaker.execute(() => client.fetch(id))` returns whatever `client.fetch` returns. TypeScript has no runtime exception type system, so "catch the operation's throw" is `try/catch (err)` + `throw err` — the original exception identity passes through untouched.

See `src/Breaker.ts`.

## Error Classes Use `name`

Each error sets `this.name` in the constructor so `err.name === 'CircuitOpenError'` works even when the class is caught across module boundaries. The error messages — `'Failure threshold must be positive'`, `'Reset timeout must be positive'`, `'Circuit is open'` — are byte-identical to the C# and Python implementations.

See `src/errors.ts`.

## Scenario Map

Twenty scenarios across five test files:

- `tests/construction.test.ts` — scenarios 1–3
- `tests/closedState.test.ts` — scenarios 4–9
- `tests/openState.test.ts` — scenarios 10–12
- `tests/transitions.test.ts` — scenarios 13–18
- `tests/roundTrip.test.ts` — scenarios 19–20

One `it()` per scenario; test names lowercase-match the SCENARIOS titles.

## How to Run

```bash
cd circuit-breaker/typescript
npm install
npx vitest run
```
