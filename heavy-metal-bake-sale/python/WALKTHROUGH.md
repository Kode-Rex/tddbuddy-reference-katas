# Heavy Metal Bake Sale — Python Walkthrough

Same design as the C# implementation — read [`../csharp/WALKTHROUGH.md`](../csharp/WALKTHROUGH.md) for the full rationale. This document covers Python-specific choices.

## Money Uses `Decimal`

Python's `float` has the same rounding hazards as JavaScript's `Number`. The `Money` dataclass wraps `Decimal` and quantizes to two decimal places on construction. This means `Money(0.75) + Money(1.35) + Money(1.50)` equals `Money(3.60)` exactly, without manual rounding at each call site.

The frozen dataclass gives us value equality for free — `Money(0.75) == Money(0.75)` works as expected in assertions.

## Exception Types

Python uses named exception classes (`OutOfStockException`, `InsufficientPaymentException`, `UnknownPurchaseCodeException`) extending `Exception`. The message strings are byte-identical to C# and TypeScript. Tests use `pytest.raises` with `match=` for both type and message verification.

## Builders

`ProductBuilder` and `OrderBuilder` follow the same fluent pattern as the other languages. Python's explicit `self` and type annotations make the builders slightly longer (~35 lines for `ProductBuilder`) — this is idiomatic, not a sign of over-engineering.

## Inventory Property Returns a Copy

`BakeSale.inventory` returns `list(self._inventory)` — a shallow copy. This prevents tests from accidentally mutating the internal list while still allowing them to inspect individual `Product` objects (which are mutable by design, since stock changes).

## How to Run

```bash
cd heavy-metal-bake-sale/python
pytest
```
