# Bank Account — TypeScript Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `Money` as a Class with Plain Methods

C# uses a `record struct` with operator overloads. TypeScript has no operator overloading, so `Money` is a plain class with `plus`, `minus`, `greaterThan`, `equals`. The method names are the domain verbs — `balance.plus(deposit)` reads well; using symbols would be noise without payoff.

### `Clock` as an Interface

TypeScript interfaces are structural, so `FixedClock` in the test folder simply implements `today(): Date`. No `implements` clause is strictly required — duck typing would suffice — but stating `implements Clock` documents intent.

### Dates Stay UTC

`Date` in JavaScript is notoriously timezone-lossy. The implementation formats dates using `getUTCFullYear/Month/Date`, and tests build fixed dates with `new Date(Date.UTC(2026, 0, 15))`. This keeps the serialized output (`2026-01-15`) consistent regardless of the test machine's locale.

### `readonly` on Every Field

The domain has no reason to mutate a `Transaction` after creation. Making fields `readonly` is both a compile-time guarantee and a reading aid — a reviewer scanning the class sees immediately that nothing escapes.

## Scenario Map

Twenty scenarios live in `tests/account.test.ts`, one `it()` per scenario. Test names match `SCENARIOS.md` verbatim in sentence form.

## How to Run

```bash
cd bank-account/typescript
npm install
npm test
```
