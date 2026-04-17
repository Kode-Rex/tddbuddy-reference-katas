# Supermarket Pricing

Dave Thomas's classic kata for modeling flexible pricing rules. An excellent showcase for **test data builders** and the **strategy pattern** — each pricing rule (unit price, multi-buy, BOGOF, weighted, combo deal) is a self-contained strategy, and each test scenario is a sentence composed through builders.

## What this kata teaches

- **Test Data Builders** — `ProductBuilder` for creating products with specific pricing rules; `CheckoutBuilder` for composing scenarios with pre-configured products.
- **Strategy Pattern** — five pricing rules as interchangeable strategies: `UnitPrice`, `MultiBuy`, `BuyOneGetOneFree`, `WeightedPrice`, `ComboDeal`.
- **Domain Types** — `Money` (cents, not floats), `Weight` (kg); never raw numbers for business values.
- **Invariant Testing** — scanning order does not affect totals; weighted rounding is deterministic; combo deals apply exactly once per qualifying pair.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
