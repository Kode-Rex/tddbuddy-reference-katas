# Bank Account

Classic state-management kata: deposits, withdrawals, overdraft rules, and formatted statements. Excellent for practicing **test data builders** over an aggregate with meaningful invariants.

## What this kata teaches

- **Test Data Builders** — `AccountBuilder`, `TransactionBuilder` make scenario setup one line each.
- **Domain Types** — `Money` and `AccountDate`, not `decimal` and `string`.
- **Invariant Testing** — every business rule (positive amounts, no overdraft) has a test that asserts the *rejection*, not just the success path.
- **Statement Formatting as a Query** — `printStatement()` is a pure projection over the transaction log, not a side effect.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
