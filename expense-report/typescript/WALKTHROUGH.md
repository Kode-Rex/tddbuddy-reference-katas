# Expense Report — TypeScript Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### String Enums for Category and ReportStatus

C# uses `enum` with integer backing values. TypeScript uses string enums (`Category.Meals = 'Meals'`) so that serialized output (the summary) and switch exhaustiveness checks both work without a lookup table. String enums also make debugger output readable.

### Domain Errors Extend `Error`

C# has named exception classes. TypeScript mirrors this with custom error classes extending `Error`. Each sets `this.name` so that `error.name` matches the class — useful for catch-filter patterns and stack traces.

The message strings are byte-identical to C# and Python.

### `Money.format()` Uses `toLocaleString`

C#'s `decimal.ToString("N2")` and Python's `Decimal` formatting produce comma-separated thousands. TypeScript uses `toLocaleString('en-US', { minimumFractionDigits: 2 })` for the same effect, with the locale pinned so output is deterministic across machines.

### Builder Convenience Methods Return `this`

`ExpenseItemBuilder.asMeal(60)` returns `this` (not a new builder), enabling chained calls like `new ExpenseItemBuilder().asMeal(60).withDescription('Client dinner').build()`. The `this` return type preserves subclass compatibility.

### `ReportBuilder.withExpenseFrom()` Takes a Callback

C# uses `Action<ExpenseItemBuilder>`; TypeScript uses `(b: ExpenseItemBuilder) => void`. Same pattern, same benefit: tests configure an expense inline without a separate variable.

## Scenario Map

Twenty-two scenarios live in `tests/report.test.ts`, one `it()` per scenario (the per-item limits scenario uses `it.each` with five rows). Test names match `SCENARIOS.md` in sentence form.

## How to Run

```bash
cd expense-report/typescript
npm install
npm test
```
