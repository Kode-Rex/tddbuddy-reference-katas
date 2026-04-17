# Event Sourcing

Event-sourced bank account where the current state is derived entirely from replaying an immutable sequence of events. Excellent for practicing **aggregate rebuild from an event stream**, **temporal queries**, and **builder-heavy test setup** over a domain with meaningful invariants.

## What this kata teaches

- **Event Sourcing Pattern** — state is never stored; it is always derived by replaying the event stream from the beginning.
- **Test Data Builders** — `EventBuilder` constructs typed events with sensible defaults; `AccountBuilder` constructs a full event stream and rebuilds the aggregate in one fluent chain.
- **Domain Types** — `Money` and typed event discriminators, not `decimal` and strings.
- **Domain Exceptions** — `AccountNotOpenException`, `AccountClosedException`, `InsufficientFundsException`, `InvalidAmountException` — named rejections, not generic errors.
- **Temporal Queries** — balance-at-a-point-in-time and date-range filtering over the event stream.
- **Projections** — transaction history and account summary are pure read-only computations over the event stream.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
