# Event Sourcing — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-four red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
AccountEvent (abstract record)
  ├── AccountOpened { AccountId, OwnerName, Timestamp }
  ├── MoneyDeposited { AccountId, Amount: Money, Timestamp }
  ├── MoneyWithdrawn { AccountId, Amount: Money, Timestamp }
  └── AccountClosed { AccountId, Timestamp }

Account
  ├── Rebuild(events) : Account      — the only constructor: replay from scratch
  ├── Deposit(Money, DateTime)       — command: validates, appends event
  ├── Withdraw(Money, DateTime)      — command: validates, appends event
  ├── Close(DateTime)                — command: validates, appends event
  ├── TransactionHistory()           — projection: list of transactions with running balance
  ├── Summary()                      — projection: owner, balance, tx count, status
  ├── BalanceAt(DateTime)            — temporal query
  └── TransactionsInRange(from, to)  — temporal query
```

## Why Events Are Records

C# records give value-based equality for free. Two `MoneyDeposited` instances with the same fields are equal — pattern matching in `Apply` reads naturally, and tests can assert on event identity without custom comparers.

The abstract base `AccountEvent` enforces that every event carries an `AccountId` and a `Timestamp`. Discriminated via subclass, not via a string tag.

## Why `Account.Rebuild` Is the Only Entry Point

There is no `new Account()` in production code. Every account is constructed by replaying an event stream through `Rebuild`. This enforces the event-sourcing invariant: **state is always derived from events**. If someone tries to construct an account without an `AccountOpened` event, `Rebuild` throws `AccountNotOpenException`.

Commands (`Deposit`, `Withdraw`, `Close`) validate against the current state, then call `Apply` to mutate — exactly the same `Apply` method used during replay. This means replay and live operation follow the same code path.

## Why Domain Exceptions, Not Booleans

The bank-account kata returns booleans for rejected operations. Event sourcing raises the stakes: a rejected command must **not** produce an event, and the caller needs to know *why* it was rejected — insufficient funds vs. closed account vs. invalid amount are different domain concepts.

Named exceptions (`InsufficientFundsException`, `AccountClosedException`, `InvalidAmountException`, `NonZeroBalanceException`, `AccountNotOpenException`) make the rejection visible in the stack trace, the test assertion, and the domain language. The message strings are byte-identical across all three languages.

## Why `Money` Is a Type

Same rationale as the bank-account kata: `new Money(100m)` is not the same as `100m`. The type closes the "amount of what?" gap and gives us a home for `IsPositive`, arithmetic operators, and comparison — all of which the domain needs.

## Why `EventBuilder` Is Static and `AccountBuilder` Is Fluent

`EventBuilder` is a static factory — `EventBuilder.AMoneyDeposited(100m, timestamp: T2)` — because events are immutable value objects. No fluent chain is needed; each factory call produces one independent event.

`AccountBuilder` is fluent because building an account means composing an event stream: `.WithDeposit(100m).WithWithdrawal(30m).Build()`. The builder generates timestamps automatically (incrementing from the open time) so tests that don't care about specific times stay concise. Tests that do care pass explicit timestamps.

The builder calls `Account.Rebuild` internally — it doesn't bypass the domain. This means builder-created accounts have the same invariant guarantees as production-created accounts.

## Why Projections Are Methods, Not Cached State

`TransactionHistory()`, `Summary()`, `BalanceAt()`, and `TransactionsInRange()` are all pure computations over the event list. They recompute on every call. This is deliberate: projections are cheap reads, and caching would add a stale-data dimension that obscures the teaching point.

In production, you'd cache projections or materialize them into read models. Here, the point is that the event stream is the source of truth and projections are derived.

## Scenario Map

The twenty-four scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/EventSourcing.Tests/AccountTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim.

## How to Run

```bash
cd event-sourcing/csharp
dotnet test
```
