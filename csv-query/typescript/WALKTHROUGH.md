# CSV Query — TypeScript Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `Row` Uses a `Map`, Not a Plain Object

A plain object would work, but `Map` gives O(1) `.has()` checks and preserves insertion order — useful when `project()` needs to maintain the column order the caller requested. The `toRecord()` escape hatch converts to a plain object when tests need `toEqual()` comparisons.

### `UnknownColumnError` Extends `Error`

TypeScript has no checked exceptions. `UnknownColumnError` extends `Error` and sets `this.name` for stack-trace readability. Tests use `toThrow(UnknownColumnError)` for type assertion and `toThrow('Unknown column: ...')` for message assertion — byte-identical to the C# and Python messages.

### `noUncheckedIndexedAccess` Is On

The `tsconfig.json` enables strict index checking, so array access returns `T | undefined`. Tests use `rows[0]!.get(...)` with the non-null assertion — the test knows the row exists because it controls the data. Production code avoids the assertion and handles the `undefined` case.

### Smart Comparison at Query Time

Same design as C#: `isNumeric()` checks both operands at comparison time. JavaScript's `Number()` handles the parsing; the only gotcha is empty strings, which `Number('')` converts to `0` — so `isNumeric` explicitly rejects empty/whitespace strings.

## Scenario Map

Twenty-five scenarios live in `tests/csvQuery.test.ts`, one `it()` per scenario.

## How to Run

```bash
cd csv-query/typescript
npm install
npm test
```
