# Shopping Cart

Cart + line items + pricing strategies. An excellent showcase for **test data builders** because each test needs a specific variant of `Product` and `LineItem`, and pricing rules cross-cut every scenario.

## What this kata teaches

- **Test Data Builders** — `ProductBuilder`, `LineItemBuilder`, `CartBuilder` compose into fluent scenario setup.
- **Strategy Pattern** — percentage, fixed-amount, buy-X-get-Y, and bulk discounts are pricing strategies selected per product.
- **Domain Types** — `Money`, `Quantity`, `SKU`; never raw numbers for business values.
- **Invariant Testing** — cart-level invariants (totals = sum of line totals) verified independently of discounts.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
