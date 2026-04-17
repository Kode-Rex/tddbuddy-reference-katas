# Robot Factory

A lean-manufacturing kata: robots are assembled to order from parts sourced across multiple suppliers via web-service integration. The factory never holds stock — every part is priced and purchased on demand from the cheapest available supplier.

## What this kata teaches

- **Test Data Builders** — `RobotOrderBuilder` makes order configuration a single fluent chain; `SupplierBuilder` lets tests declare supplier catalogs inline.
- **Collaborator Interfaces** — `IPartSupplier` represents the external web-service boundary. Tests use in-memory fakes; production swaps in HTTP clients.
- **Domain Value Types** — `Money`, `PartType`, `PartOption` keep the domain language precise.
- **Domain Exceptions** — `PartNotAvailableException` when no supplier carries a required part; `OrderIncompleteException` when the order is missing a part type.
- **Strategy Selection** — the factory finds the cheapest quote per part across all suppliers, then purchases from the winning suppliers.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
