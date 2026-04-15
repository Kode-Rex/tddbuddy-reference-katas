# Bank OCR — TypeScript Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `Digit` as a Class with Private Constructor

C# uses a `readonly record struct` with built-in equality. TypeScript has no struct equivalent, so `Digit` is a class with a **private constructor** plus two factory statics: `Digit.of(n)` for known values and `Digit.unknown` for the unknown case. Tests compare via `digit.equals(other)` because `===` on two distinct `new Digit(3)` instances would fail.

`Digit.unknown` is a singleton — a `static readonly` field shared by every unknown digit. That makes `digit === Digit.unknown` work for callers that want reference equality, though the tests prefer `.equals()` for readability.

### `parseDigit` and `parseAccountNumber` as Free Functions

C#'s `Parser` is a static class because C# requires one. TypeScript doesn't — `parseDigit` and `parseAccountNumber` are exported functions. The glyph `Map` is module-level and frozen at load time; same semantics, fewer ceremony layers.

### `noUncheckedIndexedAccess` Drives the `!` Operators

`tsconfig.json` enables `noUncheckedIndexedAccess`, so `threeRows[0]` types as `string | undefined`. Every array index in the parser is followed by `!` — not because the value might be undefined, but because the dimension validation above guarantees it isn't. The `!` is the TypeScript way to say "I just validated this; trust me."

### Tests Iterate Scenarios with a `for` Loop

Vitest doesn't have xUnit's `[Theory]`/`[InlineData]`, but a loop over `[0..9]` generating ten `it()` calls produces ten named tests in the runner — `parses the canonical glyph for 3`, etc. Same scenario count as C#, same reporting.

### `expect(() => …).toThrow(InvalidAccountNumberFormatException)`

Vitest's `toThrow` accepts the error class as a matcher. No constructor-level assertion helper is needed.

## Scope — Error Correction Deferred

Same as C#. Step 3 of the kata spec (suggest corrections) is bonus and not implemented.

## Scenario Map

Twenty-two scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/bankOcr.test.ts`. Ten digit-parse scenarios come from the `for (const value of [0..9])` loop; the remaining twelve are individual `it()` calls.

## How to Run

```bash
cd bank-ocr/typescript
npm install
npm test
```
