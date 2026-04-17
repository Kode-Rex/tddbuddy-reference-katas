# Event Sourcing — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Event** | An immutable fact that happened: account opened, money deposited, money withdrawn, account closed |
| **Event Stream** | An ordered sequence of events for a single account — the source of truth |
| **Account** | The aggregate rebuilt by replaying events; never stored directly |
| **Money** | A positive monetary amount; the domain never treats money as a bare number |
| **AccountStatus** | Open or Closed — derived from the event stream |
| **Transaction** | A deposit or withdrawal entry in the transaction history, with a running balance |
| **AccountSummary** | A projection: owner name, current balance, transaction count, account status |
| **EventBuilder** | Test helper that fluently constructs typed events with sensible defaults |
| **AccountBuilder** | Test helper that fluently constructs an event stream and rebuilds the aggregate |

## Domain Rules

- The **only** source of truth is the event stream — no mutable state is stored
- An `AccountOpened` event must appear before any other event in a stream
- **Deposit** amounts must be strictly positive; the event is rejected otherwise
- **Withdrawal** amounts must be strictly positive and cannot exceed the current balance
- A **closed** account rejects deposits and withdrawals
- An account can only be **closed** when its balance is zero
- The **current balance** is the sum of deposits minus withdrawals, computed by replay
- **Transaction history** lists every deposit and withdrawal with a running balance
- **Account summary** is a projection: owner name, balance, transaction count, status
- **Balance at a point in time** replays events up to a given timestamp
- **Transactions in a date range** filters the event stream by timestamp bounds

## Test Scenarios

### Replay — Balance

1. **Replaying an opened account has a zero balance**
2. **Replaying a single deposit yields that amount as the balance**
3. **Replaying multiple deposits yields their sum**
4. **Replaying deposits and withdrawals yields the net balance**
5. **Withdrawing the full balance yields zero**

### Command Validation — Deposits

6. **Depositing a positive amount appends a MoneyDeposited event**
7. **Depositing zero is rejected**
8. **Depositing a negative amount is rejected**
9. **Depositing into a closed account is rejected**

### Command Validation — Withdrawals

10. **Withdrawing a positive amount appends a MoneyWithdrawn event**
11. **Withdrawing zero is rejected**
12. **Withdrawing a negative amount is rejected**
13. **Withdrawing more than the balance is rejected as insufficient funds**
14. **Withdrawing from a closed account is rejected**

### Command Validation — Lifecycle

15. **Operating on an account that was never opened is rejected**
16. **Closing an account with a zero balance appends an AccountClosed event**
17. **Closing an account with a non-zero balance is rejected**

### Projection — Transaction History

18. **Transaction history of a new account is empty**
19. **Transaction history lists deposits and withdrawals with running balances**
20. **Withdrawals appear as negative amounts in the transaction history**

### Projection — Account Summary

21. **Account summary shows owner name, balance, transaction count, and open status**
22. **Account summary reflects closed status after closing**

### Temporal Queries

23. **Balance at a point in time replays only events up to that timestamp**
24. **Transactions in a date range returns only matching entries**
