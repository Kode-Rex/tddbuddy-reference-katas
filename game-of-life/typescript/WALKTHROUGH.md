# Game of Life — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (set-based infinite grid, immutable `Grid`, neighbor-count dictionary in `tick()`, minimal `GridBuilder`).

This note captures only the TypeScript deltas.

## Cell as a Plain Interface, Not a Class

TypeScript objects don't have structural `GetHashCode` like C#'s `record struct`. Two `{ row: 0, col: 1 }` objects are not `===` to each other, so they can't be stored directly in a `Set`. The workaround: cells are stored by their string key (`"row,col"`) in the grid's internal `Set<string>`, and `cellKey()` / `parseKey()` handle the conversion. This is a well-known TS pattern for value-type semantics without runtime overhead.

See `src/Cell.ts`.

## Neighbors as a Free Function

In C#, `Neighbors()` is a method on the `Cell` record struct. In TS, `Cell` is a plain interface — no methods. So `neighbors(cell)` is a free function. This is idiomatic TS: data types are interfaces, behavior is functions.

## Test Assertions Use `arrayContaining` + `toHaveLength`

Since the living cells come back in insertion order (not sorted), the tests use `expect.arrayContaining(expected)` paired with `toHaveLength` to verify set equality without depending on order. This is the TS equivalent of FluentAssertions' `BeEquivalentTo`.

## Scenario Map

Eighteen scenarios across six test files:

- `tests/emptyAndTrivial.test.ts` — scenarios 1–2
- `tests/individualRules.test.ts` — scenarios 3–8
- `tests/stillLifes.test.ts` — scenarios 9–10
- `tests/oscillators.test.ts` — scenarios 11–13
- `tests/spaceships.test.ts` — scenario 14
- `tests/gridQueries.test.ts` — scenarios 15–18

One `it()` per scenario; test names lowercase-match the SCENARIOS titles.

## How to Run

```bash
cd game-of-life/typescript
npm install
npx vitest run
```
