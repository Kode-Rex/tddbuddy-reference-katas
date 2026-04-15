# Shopping Cart — TypeScript Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `DiscountPolicy` as an Interface with Classes

TypeScript offers two plausible shapes for a strategy hierarchy: an `interface` with classes implementing it, or a discriminated union (`type Discount = { kind: 'percent', ... } | ...`) dispatched through a pure function.

This implementation chose **interface + classes**. The reason is parity with C# and Python — the three reference implementations then share a vocabulary a reader can move between without mental translation. Discriminated unions are idiomatic TS and would work, but each policy would then have to be constructed through a factory or a literal object, and the `apply` dispatch would become a `switch` that every language port would have to reinvent. The class-with-method shape compresses cleanly across all three.

A secondary reason: each policy's constructor validates its own arguments (`percent in [0, 100]`, `bulkUnitPrice >= 0`). A discriminated union would move those guards into the factory functions, duplicating what the class constructor already owns.

### `Money` and `Quantity` as Classes, Not Branded Types

TypeScript's branded-type trick (`type Money = number & { __brand: 'Money' }`) is zero-cost at runtime but leaks primitives to the `Money.amount + Money.amount` arithmetic sites — every operation re-opens the brand. A plain class with `plus`, `minus`, `times` methods keeps the domain verbs visible.

`Quantity` rejects zero-and-negative in its constructor, which means the three "updating quantity is rejected" scenarios flow through `RangeError` from the value type rather than a guard in `Cart`.

### `Cart.add` Uses a Default Parameter

C# overloads `Add(Product)` and `Add(Product, Quantity)`. TypeScript doesn't overload; the idiom is a default parameter: `add(product: Product, quantity: Quantity = ONE)`. Same ergonomics at call sites.

### `readonly` on Public Fields

Every field that doesn't mutate is marked `readonly`. Compile-time guarantee plus reading aid — a reviewer scanning the class sees immediately that nothing escapes.

### Named Constants

`MIN_PERCENT`, `MAX_PERCENT`, `HUNDRED_PERCENT` in `DiscountPolicy.ts`. Same rule as C#: no magic numbers in the domain. Mode F discipline.

## Scenario Map

Eighteen scenarios live in `tests/cart.test.ts`, grouped into six `describe` blocks that mirror `SCENARIOS.md` section headings. Test names match the scenario titles in sentence form.

## How to Run

```bash
cd shopping-cart/typescript
npm install
npm test
```
