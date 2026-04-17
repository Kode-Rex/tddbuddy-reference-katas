# Heavy Metal Bake Sale — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

## The Design at a Glance

```
BakeSale ──owns──> Product[]
   │
   ├── Inventory : IReadOnlyList<Product>
   ├── CalculateTotal(order: string) : Money   — resolves codes, validates stock, sums prices
   └── CalculateChange(total: Money, payment: Money) : Money
```

Three domain exceptions name the three rejection modes: `OutOfStockException`, `InsufficientPaymentException`, `UnknownPurchaseCodeException`.

## Why `Money` Is a Type

The kata is about pricing and change-making. Asserting `total.Should().Be(new Money(3.60m))` communicates "this is a monetary amount" in a way that `total.Should().Be(3.60m)` does not. The `Money` value type also provides `ToDisplay()` for formatted output (`$3.60`), keeping formatting consistent without scattering `string.Format` across the codebase.

See `src/HeavyMetalBakeSale/Money.cs`.

## Why `Product` Is Mutable

Unlike `Money`, a `Product` has identity — it's a specific item in the inventory with a stock count that changes as sales are processed. The `DecrementStock()` method makes the mutation explicit and auditable. Tests verify that stock decrements on success and does **not** decrement when any item in the order is out of stock.

See `src/HeavyMetalBakeSale/Product.cs`.

## Why Domain-Specific Exceptions

The spec defines three distinct failure modes: out of stock, insufficient payment, and unknown purchase code. Each has its own exception type with a message that matches the spec byte-for-byte across all three language implementations. This lets tests assert on the meaningful type (`act.Should().Throw<OutOfStockException>()`) rather than matching on a generic error's message string.

See `src/HeavyMetalBakeSale/OutOfStockException.cs`, `InsufficientPaymentException.cs`, `UnknownPurchaseCodeException.cs`.

## Why `ProductBuilder` and `OrderBuilder`

`ProductBuilder` lets tests override only the fields that matter for a scenario — a test about out-of-stock sets `.WithStock(0)` and inherits sensible defaults for name, price, and code. `OrderBuilder` assembles the inventory that `BakeSale` operates on, with a `.WithDefaultInventory()` shorthand for the standard four-product lineup.

Together they make setup one line per scenario and remove duplication without hiding intent.

See `tests/HeavyMetalBakeSale.Tests/ProductBuilder.cs` and `OrderBuilder.cs`.

## Why Stock Validation Happens Before Decrement

`CalculateTotal` resolves all purchase codes, then validates stock for the entire order as a batch, then decrements. This ensures atomicity: if any item is out of stock, no stock is decremented and the order is fully rejected. Tests verify this explicitly — scenario 12 checks that stock is unchanged after a partial out-of-stock rejection.

## Scenario Map

The nineteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/HeavyMetalBakeSale.Tests/BakeSaleTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim.

## How to Run

```bash
cd heavy-metal-bake-sale/csharp
dotnet test
```
