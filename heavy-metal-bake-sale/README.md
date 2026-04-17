# Heavy Metal Bake Sale

Inventory-and-pricing kata: a local metal band runs a bake sale with limited stock, coded product purchases, and change-making. Excellent for practicing **test data builders** over an order/inventory domain with stock constraints and monetary arithmetic.

## What this kata teaches

- **Test Data Builders** — `ProductBuilder` and `OrderBuilder` make scenario setup one line each.
- **Domain Types** — `Money` wraps monetary amounts; `PurchaseCode` names the product, not a bare string.
- **Inventory Management** — stock levels are first-class; the domain rejects sales when an item is out of stock.
- **Domain Exceptions** — `OutOfStockException` and `InsufficientPaymentException` name the rejection, not just throw a generic error.
- **Change Calculation** — overpayment math is a pure domain operation, not a UI concern.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
