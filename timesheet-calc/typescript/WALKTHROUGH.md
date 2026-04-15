# Timesheet Calc — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md). This file is a **delta** — it covers what is idiomatic to TypeScript, not the full design rationale.

## TS-Specific Idioms

### One module, multiple related types

Per the F2 style conventions: TS may collapse small related types into one module. `src/timesheet.ts` exports the `Day` enum, the `isWeekend` helper, the two named constants (`DAILY_OVERTIME_THRESHOLD`, `STANDARD_WORK_WEEK_HOURS`), the error-message string, the `Timesheet` and `TimesheetTotals` interfaces, and the `createTimesheet` factory. A one-file-per-type split would be unidiomatic TS for five small declarations, and splitting here would not improve discoverability — the whole surface fits on one screen.

C# and Python distribute the same surface across four source files; that is idiomatic to those languages. The cross-language divergence is noted here explicitly so a reader moving between folders does not infer a design difference where there is only an idiom difference.

### String-valued enum

`enum Day { Monday = 'Monday', ... }` — a string-valued enum. Numeric enums would pick up integer arithmetic and would print as numbers in test-failure diagnostics. String enums round-trip clearly and produce readable `toThrow` messages if one ever leaks into an error.

### Factory function, not a class

`createTimesheet(entries)` returns an object literal with a `totals` method. A class would force a `new` site that tests rarely want (all construction flows through the builder). The returned object is typed as `Timesheet` so the surface is still single-responsibility — `totals()` is the only method.

### `Error` with an identical message string

TS throws plain `Error`. The message string `'hours must not be negative'` is byte-identical to the C# and Python versions, codified in `ERROR_HOURS_MUST_NOT_BE_NEGATIVE`. That cross-language string is part of the spec; the error *type* is idiomatic per language (C# `ArgumentException`, Python `ValueError`).

### `ReadonlyMap<Day, number>` as the entries type

`Map<Day, number>` preserves insertion order — same as C#'s `Dictionary` and Python's `dict` since 3.7. The factory accepts `ReadonlyMap` to document that `createTimesheet` does not mutate; the builder hands over its internal `Map`, which is fine because the builder is discarded after `.build()`.

### `toBeCloseTo` for fractional hours

Floating-point comparisons in `expect` use `toBeCloseTo`, not `toBe`. Only one test needs it (the 8.5-hour fractional case); the rest compare exact integers and use `toBe`.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/timesheet.test.ts`, one `it()` per scenario.

## How to Run

```bash
cd timesheet-calc/typescript
npm install
npx vitest run
```
