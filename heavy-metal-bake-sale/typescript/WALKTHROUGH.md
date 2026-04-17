# Heavy Metal Bake Sale — TypeScript Walkthrough

Same design as the C# implementation — read [`../csharp/WALKTHROUGH.md`](../csharp/WALKTHROUGH.md) for the full rationale. This document covers TypeScript-specific choices.

## Money Arithmetic

JavaScript floating-point arithmetic can produce rounding artifacts (e.g. `0.75 + 1.35 + 1.50` might not equal `3.60` exactly). The `Money` class rounds to two decimal places on every arithmetic operation using `Math.round(x * 100) / 100`, keeping all assertions exact without reaching for a `Decimal` library.

## Exception Types

TypeScript has no checked exceptions or custom exception hierarchies in the Java/C# sense. Each domain exception extends `Error` and sets `this.name` for clean identification. Tests assert both the constructor (`toThrow(OutOfStockException)`) and the message string (`toThrow('Water is out of stock')`) — the type for structured handling, the string for spec compliance.

## Builders

`ProductBuilder` and `OrderBuilder` mirror the C# builders. TypeScript's `this` return type on fluent methods avoids the need for explicit generic self-types. Colocated in `tests/` alongside the test file.

## How to Run

```bash
cd heavy-metal-bake-sale/typescript
npx vitest run
```
