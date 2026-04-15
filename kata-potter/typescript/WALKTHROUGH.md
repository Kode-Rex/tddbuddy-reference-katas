# Kata Potter — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Pure Domain Only

No catalogue, no receipts, no configurable series. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## The TypeScript Shape

- **Everything lives in one module, `src/basket.ts`.** `Basket`, `BookOutOfRangeError`, `BasketMessages`, `BASE_PRICE`, `SET_DISCOUNT`, and the `priceOfSet` helper sit alongside one another. C# and Python split one type per file; TS idiom is to colocate small related types in one module.
- **`price()` returns a plain `number`.** TypeScript has no native decimal — the kata's discount table is representable exactly as IEEE 754 doubles for the reachable values, but tests use `toBeCloseTo(..., 3)` to paper over any accumulated drift that C# avoids with `decimal`. The tolerance is documented in `SCENARIOS.md`.
- **`SET_DISCOUNT` is a `readonly number[]` with index 0 unused** — the same shape as the C# array. Indexing is narrowed by the `MAX_BOOK_ID` loop bound; `noUncheckedIndexedAccess` is on so accesses use the `!` suffix after the runtime-invariant is clear.
- **`BookOutOfRangeError` subclasses `Error`** with the `BasketMessages.bookOutOfRange` message. Tests assert on both the class and the message — the latter is the spec contract and is byte-identical across languages.
- **The greedy-then-adjust algorithm is identical to C#.** Same histogram representation, same single (5,3) → (4,4) swap pass, same correctness argument.

## Why `BasketBuilder` Lives in `tests/`

Same F2 rationale as C#: twelve scenarios need specific basket shapes, and the chained `withBook(series, count)` reads as the literal basket. The builder is 20 lines — inside the 10–30 line F2 TypeScript budget.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/basket.test.ts`, one `it()` per scenario, titles matching the scenario statements.

## How to Run

```bash
cd kata-potter/typescript
npm install
npx vitest run
```
