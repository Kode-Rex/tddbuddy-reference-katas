# Password — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Policy Only

Credential store, password repository, email reset, token expiry, and password history are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list. This reference is scoped to **policy validation only**.

## The TypeScript Shape

- **`Policy` is an interface plus a `createPolicy(spec)` factory**, not a class. Classes here would earn nothing — no inheritance, no `this`-sensitive methods beyond `validate`, no runtime `instanceof` checks anywhere. The interface names the shape; the factory closes over the spec and returns an object with `validate`. `readonly` on every field gives the same immutability guarantee that C# got from `record`.
- **`ValidationResult` is an interface with `readonly ok` and `readonly failures: readonly string[]`**. Same reasoning — it is a data pair, not a thing with identity.
- **Rule-name strings live in a single `RuleNames` object with `as const`** so TypeScript infers the literal string type and the strings stay byte-identical to the C# and Python spellings.
- **Character classes are regex literals** (`/[0-9]/`, `/[^A-Za-z0-9]/`, etc.) rather than character loops. TypeScript's regex is idiomatic and reads as the rule it enforces. The definition of "symbol" stays consistent across languages: *anything that is not ASCII letter and not ASCII digit.*

## Why `PolicyBuilder` Lives in `tests/`

Same F2 rationale as C#: scenarios need many tiny policy variations, and without a builder each test opens with a five-argument object literal where four arguments are `false`. With the builder, setup is one fluent line that names the variation. The builder is 22 lines including braces — within the 10–30 line F2 budget. `build()` calls `createPolicy` rather than reaching into the factory's internals, so the test-folder builder depends on the public surface only.

## Scenario Map

The ten scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/policy.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd password/typescript
npm install
npx vitest run
```
