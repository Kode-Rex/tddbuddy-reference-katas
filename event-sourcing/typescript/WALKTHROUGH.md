# Event Sourcing — TypeScript Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### Events as Discriminated Interfaces

C# uses record inheritance with pattern matching. TypeScript uses a discriminated union: each event interface carries a `type` literal (`'AccountOpened'`, `'MoneyDeposited'`, etc.) and the `AccountEvent` base interface requires `type`, `accountId`, and `timestamp`. Factory functions (`accountOpened`, `moneyDeposited`, etc.) construct the objects — no `new` keyword needed since these are plain object literals.

### Exception Classes Extend `Error`

TypeScript has no built-in exception hierarchy beyond `Error`. Each domain exception (`AccountNotOpenException`, `AccountClosedException`, etc.) extends `Error` and sets `this.name` for readable stack traces. The message strings are byte-identical to C# and Python.

### `Money` as a Class with Methods

TypeScript has no operator overloading. `Money` provides `plus`, `minus`, `negate`, `greaterThan`, and `equals` methods. `Money.zero` is a static instance. The pattern is identical to the bank-account kata.

### Dates Stay UTC

All test timestamps use `Date.UTC(...)` to avoid timezone drift. The `AccountBuilder` auto-increments timestamps from the open time using hour offsets, keeping events well-ordered without test authors needing to think about time.

### `AccountStatus` as a String Union

Rather than an enum, `AccountStatus` is `'Open' | 'Closed'` — a string union that plays well with discriminated-union patterns and serializes naturally.

## Scenario Map

Twenty-four scenarios live in `tests/account.test.ts`, one `it()` per scenario.

## How to Run

```bash
cd event-sourcing/typescript
npm install
npm test
```
