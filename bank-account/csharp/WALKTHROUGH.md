# Bank Account — C# Walkthrough

This kata ships in **middle/high gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Account ──owns──> Transaction[]
   │
   ├── Balance : Money
   ├── Deposit(Money) : bool   — domain invariants return a yes/no, not throw
   ├── Withdraw(Money) : bool
   └── PrintStatement() : string
```

Four pieces. Each earns its keep.

## Why `Money` Is a Type

Tests expect specific monetary values. Asserting `account.Balance.Should().Be(new Money(500m))` is not the same as asserting on a `decimal`. It says: *balance is a money quantity, and these two moneys are equal*. That's what the domain means.

A raw `decimal` also invites the question "amount of what?" — `Money` closes that gap. When a future scenario adds currency or rounding rules, every site that already spells `Money` is ready. Sites that spelled `decimal` would all need rediscovery.

See `src/BankAccount/Money.cs`.

## Why `IClock`, Not `DateTime.Today`

Three scenarios assert on the **date** recorded with a transaction. If those tests called `DateTime.Today` directly, they'd be correct today and flaky forever after — especially across midnight or DST boundaries. More importantly, they'd couple the test to real time, which is a collaboration the test doesn't intend to have.

`IClock.Today()` makes the collaboration explicit. `FixedClock` in the test project is the mock — but not a mocking-library mock, just a tiny deterministic implementation. The tests read "on January 15, I deposited £500" instead of "on whatever today is, I deposited £500 and hope the assertion isn't near midnight."

This is the **Mocks as Behavioral Specifications** principle: when collaboration is part of the behavior, make it an interface. When collaboration isn't — like the transaction list, which is internal state — don't.

See `src/BankAccount/IClock.cs` and `tests/BankAccount.Tests/FixedClock.cs`.

## Why `AccountBuilder`

Most tests need an account that already has some history. Without a builder, every test writes four setup lines to arrange deposits with specific dates. With `AccountBuilder().WithDepositOn(Jan15, 500m).Build()`, setup is one line that reads like English.

Crucially, the builder returns **both** the account and its clock. Tests that want to advance time after setup (e.g. "deposit yesterday, withdraw today") need access to the clock. Returning a tuple keeps the builder pure — no hidden state, nothing to reach into later.

See `tests/BankAccount.Tests/AccountBuilder.cs`.

## Why `Deposit` and `Withdraw` Return `bool`

An alternative API would throw on invalid input. That buys nothing: the domain rule is "this operation either succeeded or didn't." Throwing forces every caller into try/catch; returning a bool lets the caller react to the result or ignore it.

More importantly, **tests assert on the return value directly**. `account.Deposit(zero).Should().BeFalse()` is a clearer spec than `Assert.Throws<InvalidAmountException>(() => account.Deposit(zero))`. The second asserts on the *mechanism* of rejection; the first asserts on the *fact* of rejection, which is the domain rule.

## Why `PrintStatement` Is a Pure Query

The statement reads from the transaction list and formats it. It never mutates. It has no side effect. Calling it a hundred times produces the same string a hundred times.

This matters because statement-formatting tests then don't need to reset anything between runs. Set up transactions once, assert on the statement string. The test reads as a specification of *what the format is*, not as a ritual of call-then-check.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/BankAccount.Tests/AccountTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd bank-account/csharp
dotnet test
```
