# Memory Cache — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (generic `Cache<V>`, injected `Clock`, explicit `evictExpired()` sweep with cross-links to `library-management` and `video-club-rental`, LinkedList+Map for O(1) LRU, named domain exceptions).

This note captures only the TypeScript deltas.

## Clock Is Structural

TypeScript doesn't need a class to implement the `Clock` interface — any object with a `now(): Date` method satisfies it. `FixedClock` in the tests is a class for the convenience of `advanceMs` / `advanceTo`, but consumers of `Cache<V>` could pass `{ now: () => new Date() }` at the call site. Interfaces in TS describe shape, not inheritance.

See `src/Clock.ts` and `tests/FixedClock.ts`.

## TTL as Milliseconds

`Date.getTime()` returns epoch ms; comparing insertion-time to `now` as `number - number >= ttlMs` is the clean shape. The C# version keeps the ergonomic `TimeSpan` type; TS pays for an idiomatic-ms choice with clearer arithmetic. The constant `DEFAULT_TTL_MS = 60_000` makes the unit explicit at every use site.

## Date UTC Discipline

`FixedClock` is constructed with `new Date(Date.UTC(2026, 0, 1))` in the builder default. The TTL math compares `getTime()` values, which are UTC milliseconds regardless of the local timezone, so timezone drift cannot silently expire or preserve an entry. The tests never construct a `Date` from a local-time literal.

## Error Classes Use `name`

Each error sets `this.name` in the constructor so `err.name === 'CacheCapacityInvalidError'` works even when the class is caught across module boundaries. The error messages — `'Capacity must be positive'`, `'TTL must be positive'` — are byte-identical to the C# and Python implementations.

See `src/errors.ts`.

## Scenario Map

Twenty scenarios across three test files:

- `tests/putGet.test.ts` — scenarios 1–10
- `tests/capacityEviction.test.ts` — scenarios 11–14
- `tests/ttlExpiry.test.ts` — scenarios 15–20

One `it()` per scenario; test names lowercase-match the SCENARIOS titles.

## How to Run

```bash
cd memory-cache/typescript
npm install
npx vitest run
```
