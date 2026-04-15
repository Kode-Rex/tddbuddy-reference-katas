# Tic-Tac-Toe — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Pure Domain Only

No UI, no CLI, no persistence, no AI opponent. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The TypeScript Shape

- **Everything lives in one module, `src/board.ts`.** `Cell`, `Mark`, and `Outcome` are string-literal union types (`'Empty' | 'X' | 'O'`, etc.). Three error classes, the `Board` class, and the `BOARD_SIZE` and `BoardMessages` constants sit alongside. C# and Python split one type per file; TS idiom is to colocate small related types in one module. The walkthrough codifies this divergence — don't force TS to split.
- **`Cell` is a string-literal union, not an enum.** String-literal unions give the same exhaustiveness checks with zero runtime cost, pretty-print as their string values in test failure diagnostics (`expected 'Empty' to be 'X'`), and avoid the enum-reverse-mapping footgun on numeric enums.
- **`Board` is a class with a `readonly` internal grid** rather than a plain object with a `validate` closure (the pattern used in `password/`). The class reads as the domain entity, and `place` returning `new Board(next)` makes the immutable-copy semantics explicit at the call site. `Board.fromGrid` is the test-folder escape hatch the builder uses to materialise a grid without replaying moves.
- **Error classes subclass `Error`** with the message strings from `BoardMessages` — byte-identical to the C# `BoardMessages` and Python `BoardMessages` values. Tests assert on both the class (`toThrow(CellOccupiedError)`) and the message string (`toThrow('cell already occupied')`); the latter is the spec contract.
- **`noUncheckedIndexedAccess` is on**, which is why the grid accesses thread through `!` assertions after bounds checks. The bounds check *is* the proof of safety, and the `!` is the price TS charges for making that proof implicit rather than encoded in the type.

## Why `BoardBuilder` Lives in `tests/`

Same F2 rationale as C#: twelve scenarios need a dozen slightly different board literals, and without a builder each test would replay a move sequence instead of describing the board state under test. With the builder, setup is a chained list of `.withXAt` / `.withOAt` calls that reads exactly like the board. The builder is 12 lines; within the 10–30 line F2 budget. `build()` calls `Board.fromGrid` rather than reaching into private state, so the test-folder builder depends on a documented public surface only.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/board.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd tic-tac-toe/typescript
npm install
npx vitest run
```
