# Bingo — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Pure Domain Only

No generator, no caller, no game loop. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The TypeScript Shape

- **Everything lives in one module, `src/card.ts`.** `WinPattern` (discriminated union), `Card`, `NumberOutOfRangeError`, and the `CARD_SIZE` / `MIN_NUMBER` / `MAX_NUMBER` / free-space constants sit alongside one another. C# and Python split one type per file; TS idiom is to colocate small related types in one module. Don't force TS to split.
- **`WinPattern` is a discriminated union on `kind`** rather than an enum + index pair (the C# shape) or a class hierarchy. The union carries the index only for `Row` and `Column`, not for the two diagonals — `kind: 'DiagonalMain'` with no index reads more honestly than `DiagonalMain` with a stored-but-meaningless `-1`. A companion `WinPattern` *value* namespace exposes `.none`, `.row(i)`, `.column(i)`, `.diagonalMain`, `.diagonalAnti` — the same readable static-factory vibe the C# record-struct has, expressed as a const object. Tests compare with `toEqual(...)` because these are structural objects, not referential singletons.
- **`Card` is a class with a mutable `number | null` grid and a mutable boolean grid.** Same mutability decision as C# — bingo's domain is sequential calls on a single card, and the off-card `mark` no-op is explicitly a no-op on shared state.
- **`Card`'s constructor takes a `CardState` literal** rather than exposing a static `fromGrid`. `noUncheckedIndexedAccess` is on so grid accesses thread through `!` after the bounds checks; the bounds check *is* the proof of safety.
- **Error class subclasses `Error`** with the message string from `CardMessages`. Byte-identical to the C# and Python spellings. Tests assert on both the class and the message string; the latter is the spec contract.

## Why `CardBuilder` Lives in `tests/`

Same F2 rationale as C#: a dozen scenarios need specific card layouts, and without a builder each test would either rely on random generation and hope or manually construct a grid literal. With `withNumberAt` and `withMarkAt`, setup reads as the literal card the test cares about. The builder is 24 lines — inside the 10–30 line F2 TypeScript budget.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/card.test.ts`, one `it()` per scenario, titles matching the scenario statements.

## How to Run

```bash
cd bingo/typescript
npm install
npx vitest run
```
