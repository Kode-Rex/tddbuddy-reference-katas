# Supermarket Pricing — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — five pricing strategies behind a `PricingRule` interface, `Money` as integer cents, `Weight` as a validated non-negative number, and `ComboDeal` as a checkout-level cross-product rule. This document covers what's idiomatic to TypeScript.

## What's the Same

- `Money` stores integer cents, same as C#.
- `PricingRule` is a TypeScript interface with four concrete implementations (`UnitPrice`, `MultiBuy`, `BuyOneGetOneFree`, `WeightedPrice`) colocated in one file — TS idiom for small related types.
- `Checkout` accumulates scans in `Map<string, number>` for quantities and `Map<string, Weight>` for weights.
- `ProductBuilder` and `CheckoutBuilder` mirror C# fluent patterns with `this` return types.

## What's Different

- **No separate files per pricing rule.** C# follows one-type-per-file; TS colocates all four rules plus the interface in `PricingRule.ts`. This is idiomatic — the rules are small and tightly related.
- **`Weight.zero` is a static readonly instance** instead of a factory property. TypeScript classes support static readonly members directly.
- **`Math.round` for weighted pricing** uses JavaScript's default rounding (round-half-away-from-zero for positive values), which matches the C# `MidpointRounding.AwayFromZero` for the positive prices in the spec.
- **`equals` method on `Money`** instead of structural equality — TypeScript objects are reference-compared by default, so tests call `checkout.total().equals(new Money(175))`.

## Scenario Map

All twenty scenarios live in `tests/checkout.test.ts`, grouped by `describe` blocks that mirror the spec's section headings.

## How to Run

```bash
cd supermarket-pricing/typescript
npm install
npm test
```
