# Tennis Refactoring — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Single-Game Scoring Only

No `TennisGame` class, no tiebreak, no set or match tracking. See [`../README.md`](../README.md#scope--single-game-scoring-only) for the full list.

## Relationship to `tennis-score/`

[`../../tennis-score/typescript/`](../../tennis-score/typescript/) is the Pedagogy tennis kata; this is the refactoring mirror. Different katas, overlapping outputs. See the [top-level README](../README.md#relationship-to-tennis-score) for which one to pick.

## The TypeScript Shape

- **`getScore` is a top-level exported function**, not a method on a class. The legacy was a free function; the refactor preserves that shape. There is no domain entity with identity here — the scorer is a pure mapping from four arguments to a string.
- **Three named private helpers** — `equalScore`, `endgameScore`, `inPlayScore` — live in the same module. TypeScript idiom colocates small related functions in one module rather than scattering them across files. F2 style convention: TS may collapse multi-entity files.
- **`POINT_NAMES` is `as const`**, and `pointName(score)` indexes it. `noUncheckedIndexedAccess` is on in `tsconfig.json`, so the indexed access is narrowed with a `!` non-null assertion — the characterization contract already constrains `score` to the range `0..3` in the code paths that call `pointName`. The walkthrough calls this out rather than papering over it with a default branch.
- **Named constants for domain numbers.** `ENDGAME_THRESHOLD = 4` and `DEUCE_THRESHOLD = 3` match the C# `EndgameThreshold` / `DeuceThreshold` — the F2 style convention to prefer named business numbers to inline literals, and the opposite of the Pedagogy kata's "literals inline because the walkthrough names them by value" rule.
- **No builder.** This kata is classified *characterization test set only* — see the F2 classification in [`docs/plans/2026-04-14-remaining-katas.md`](../../docs/plans/2026-04-14-remaining-katas.md). A builder for a four-argument function would be longer than the call site.

## Scenario Map

The sixteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/tennisScorer.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd tennis-refactoring/typescript
npm install
npx vitest run
```

Expected: **16 tests passed.**
