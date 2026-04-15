# Shopping Cart — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the domain was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through eighteen red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Cart ──owns──> LineItem[]
               │
               ├── Product ──references── IDiscountPolicy
               │                            ├── NoDiscount
               │                            ├── PercentOff(p)
               │                            ├── FixedOff(amount)
               │                            ├── BuyXGetY(buy, free)
               │                            └── BulkPricing(threshold, bulkUnitPrice)
               └── Quantity
```

Five pieces on the domain side, five discount strategies. Each earns its keep.

## Why `Money` and `Quantity` Are Types

Tests assert on specific subtotals and totals. `cart.Total().Should().Be(new Money(11.00m))` says *this is a money quantity, and these two moneys are equal*. That's what the domain means. A raw `decimal` invites "amount of what?" — `Money` closes the gap.

`Quantity` rejects zero and negatives at construction. Scenarios 6 and 7 ("updating quantity to zero / negative is rejected") are then satisfied by the value type's invariant rather than by a guard sprinkled through `Cart`. The domain rule lives exactly once, where it belongs.

See `src/ShoppingCart/Money.cs`, `src/ShoppingCart/Quantity.cs`.

## Why `IDiscountPolicy` Is a Strategy Hierarchy

The naive alternative is a `DiscountKind` enum plus a big `switch` inside `LineItem.Subtotal()`. That couples every discount variant to a single file, makes adding a new variant a visible edit to core domain code, and leaves the percent/fixed/buy-X-get-Y formulas co-located with each other for no good reason.

The strategy pattern pushes each variant into its own class with one method: `Apply(unitPrice, quantity) -> Money`. The line-item calculation becomes trivial:

```csharp
public Money Subtotal() => Product.DiscountPolicy.Apply(Product.UnitPrice, Quantity);
```

`NoDiscount.Instance` is the null-object default so a `LineItem` never carries a nullable policy. Products without a promotion still flow through the same dispatch.

See `src/ShoppingCart/DiscountPolicy.cs` and the four policy files beside it.

## Why Cart Owns Its LineItems

The alternative — `LineItem` holds a back-reference to `Cart` — buys nothing and complicates construction. Line items are owned collection members, not standalone entities with their own identity. They surface to tests through `cart.Lines` (read-only) so assertions can inspect structure without mutating it.

Mutation APIs (`Add`, `Remove`, `UpdateQuantity`) go through `Cart` because that's where the "same SKU already in cart" merge rule lives. `LineItem` exposes `IncrementBy` and `SetQuantity` as `internal` — only the aggregate root gets to call them.

See `src/ShoppingCart/Cart.cs`, `src/ShoppingCart/LineItem.cs`.

## Why `ProductBuilder` and `CartBuilder`

Most tests need a product with a specific price and an optional discount policy. Without a builder, every test repeats `new Product("APPLE", "Apple", new Money(1.25m), null)`. With `new ProductBuilder().WithSku("APPLE").PricedAt(1.25m).Build()`, each test reads as a sentence in the domain.

`CartBuilder` composes on top: seed a cart with pre-added products in one line. The three "total" scenarios would otherwise open with three `cart.Add(...)` calls per test. They now read "given a cart containing these products, the total is X" — which is exactly the scenario statement.

See `tests/ShoppingCart.Tests/ProductBuilder.cs`, `tests/ShoppingCart.Tests/CartBuilder.cs`.

## Why Named Constants, Not Magic Numbers

`PercentOff` uses `HundredPercent = 100m` for the divisor and `MinPercent / MaxPercent` for the range check. Inline `100` in three places would work and fail to read — `multiplier = (HundredPercent - Percent) / HundredPercent` names the arithmetic. This is the Mode F rule: no magic numbers in the domain. (The opposite rule applies in Pedagogy-mode katas, where inline literals preserve the teaching moment.)

## Why `UpdateQuantity` Throws Instead of Returning `bool`

The deposit/withdraw API in Bank Account returns `bool` because "was this accepted?" is a meaningful runtime question — a UI might accept a deposit and dim a button if rejected. Here, zero-or-negative quantities arrive through `Quantity`'s own constructor, which is a programmer error, not a runtime condition. `ArgumentOutOfRangeException` matches what .NET convention expects for "you passed a value my contract forbids."

`UpdateQuantity` for a missing SKU throws `InvalidOperationException` — the method's precondition ("there is a line for this SKU") was violated. Scenarios don't exercise the missing-SKU path directly; guarding anyway costs nothing and documents intent.

## What's Deliberately NOT Modeled

The kata's bonus features — loyalty points, multi-currency, tax, shipping, coupon codes, saved carts — are intentionally absent. Each one would warrant its own collaborator or aggregate, and dragging them in now would dilute what this kata teaches: **domain types, strategy pattern for pricing variants, builders for scenario setup**. The eighteen scenarios exercise exactly those three ideas and nothing else.

If a future version wants to model tax, the natural extension is a `TaxPolicy` collaborator injected into `Cart.Total()`. If it wants currency conversion, `Money` grows a `Currency` field and the strategies handle mixed-currency carts explicitly. The current shape is friendly to both — no refactor required before adding them, only extension.

## Scenario Map

The eighteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/ShoppingCart.Tests/`, split into six files that mirror the spec's section headings:

- `BasicCartOperationsTests.cs` — 7 scenarios
- `SubtotalAndTotalTests.cs` — 3 scenarios
- `PercentDiscountTests.cs` — 2 scenarios
- `FixedDiscountTests.cs` — 2 scenarios
- `BuyXGetYTests.cs` — 2 scenarios
- `BulkPricingTests.cs` — 2 scenarios

Test names match the scenario titles verbatim (C# underscore convention).

## How to Run

```bash
cd shopping-cart/csharp
dotnet test
```
