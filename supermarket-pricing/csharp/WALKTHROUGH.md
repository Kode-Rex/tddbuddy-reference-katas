# Supermarket Pricing — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the domain was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Checkout ──accumulates──> (SKU → count) + (SKU → weight)
             │
             ├── Product ──references── IPricingRule
             │                            ├── UnitPrice(cents)
             │                            ├── MultiBuy(groupSize, groupPrice, itemPrice)
             │                            ├── BuyOneGetOneFree(itemPrice)
             │                            └── WeightedPrice(centsPerKg)
             │
             └── ComboDeal(skuA, skuB, dealPrice)  ← cross-product rule
```

Five pricing strategies plus one cross-product combo deal. Each earns its keep.

## Why `Money` Is Integer Cents

The spec quotes prices like "$50" and "$1.99/kg." A naive `decimal` or `double` representation invites floating-point rounding errors — `0.5 * 199 = 99.5`, which must round to 100 cents, not 99 or 100.0. By storing `Money` as integer cents from the start, all arithmetic is exact integer math. The only rounding happens in `WeightedPrice.Calculate`, where the `decimal × int` product is explicitly rounded to the nearest cent via `Math.Round(…, MidpointRounding.AwayFromZero)`.

This is the same principle as `Money` in the shopping-cart sibling kata, but pushed further: shopping-cart used `decimal`; here we use `int` because the spec's prices are all whole-cent amounts and the integer representation makes the rounding boundary visible.

See `src/SupermarketPricing/Money.cs`.

## Why `IPricingRule` Is a Strategy Hierarchy

The alternative is a `PricingKind` enum plus a `switch` inside `Checkout.Total()`. That puts every pricing variant in one file, couples new rules to existing code, and makes the formula for "3 for $1.30" sit next to "buy one get one free" for no good reason.

The strategy pattern pushes each variant into its own class with one method: `Calculate(quantity, weight) → Money`. The checkout never knows which rule a product carries — it just calls `Calculate`. Adding a sixth rule (e.g., percentage discount) means writing one new class and zero edits to `Checkout`.

See `src/SupermarketPricing/PricingRule.cs` and the four rule files beside it.

## Why `ComboDeal` Is Separate from `IPricingRule`

Combo deals cross product boundaries — "D + C together for $25" cannot be computed by looking at D's pricing rule alone. A combo deal needs visibility into the full checkout's scan state, which individual per-product pricing rules don't have.

The design handles this by keeping `ComboDeal` as a checkout-level concept. `Checkout.Total()` processes combo deals first, consuming qualifying pairs, then delegates remaining quantities to each product's `IPricingRule`. This keeps per-product rules ignorant of cross-product promotions.

See `src/SupermarketPricing/ComboDeal.cs`, `src/SupermarketPricing/Checkout.cs`.

## Why `Weight` Is a Type

`Weight` rejects negative values at construction. This means tests never need to assert "negative weight is rejected at checkout level" — the invariant lives once, where it belongs. The `Weight.Zero` default means products that are not weighed carry a zero-weight entry, and their pricing rule (which ignores weight) computes correctly.

See `src/SupermarketPricing/Weight.cs`.

## Why `ProductBuilder` and `CheckoutBuilder`

Most tests need a product with a specific SKU and pricing rule. Without a builder, every test repeats `new Product("A", "A", new MultiBuy(3, new Money(130), new Money(50)))`. With `new ProductBuilder().WithSku("A").WithMultiBuy(3, 130, 50).Build()`, each test reads as a sentence in the domain.

`CheckoutBuilder` composes on top: seed a checkout with pre-scanned items and combo deals in one fluent chain. The builder hides the scan loop so tests read "given a checkout with 3 A's and 2 B's, the total is 175."

See `tests/SupermarketPricing.Tests/ProductBuilder.cs`, `tests/SupermarketPricing.Tests/CheckoutBuilder.cs`.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/SupermarketPricing.Tests/`, split into five files that mirror the spec's section headings:

- `SimplePricingTests.cs` — 4 scenarios
- `MultiBuyTests.cs` — 6 scenarios
- `BuyOneGetOneFreeTests.cs` — 3 scenarios
- `WeightedPriceTests.cs` — 3 scenarios
- `ComboDealTests.cs` — 4 scenarios

Test names match the scenario titles verbatim (C# underscore convention).

## How to Run

```bash
cd supermarket-pricing/csharp
dotnet test
```
