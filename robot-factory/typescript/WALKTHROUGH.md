# Robot Factory — TypeScript Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### String Literal Unions for `PartType` and `PartOption`

C# uses enums for both `PartType` and `PartOption`. TypeScript uses string literal union types instead — `type PartType = 'Head' | 'Body' | 'Arms' | 'Movement' | 'Power'`. This gives compile-time type safety while keeping runtime values readable in error messages and assertions. No `.toString()` gymnastics when composing `"Part not available: InfraredVision"`.

### `PartSupplier` as an Interface

TypeScript interfaces are structural. `FakePartSupplier` explicitly implements `PartSupplier` to document intent, but any object with matching `name`, `getQuote`, and `purchase` members would satisfy the contract. The fake adds `purchaseLog` for test assertions without polluting the production interface.

### `Money` as a Class

Same pattern as the bank-account kata: `Money` wraps a `number` with named methods (`plus`, `lessThan`, `equals`). TypeScript has no operator overloading, so the cheapest-quote reduction uses `q.price.lessThan(best.price)` instead of `q.Price < best.Price`.

### Map Key Strategy for the Fake Supplier

The fake supplier catalogs parts using a `Map<string, Money>` keyed by `"${type}:${option}"`. This avoids the composite-key equality problem that JavaScript Maps have with object keys. The key is an implementation detail of the fake, not part of the domain.

## Scenario Map

Twenty scenarios live in `tests/robotFactory.test.ts`, one `it()` per scenario. Test names match `SCENARIOS.md` verbatim in sentence form.

## How to Run

```bash
cd robot-factory/typescript
npm install
npm test
```
