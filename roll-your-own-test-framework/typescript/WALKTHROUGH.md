# Roll Your Own Test Framework — TypeScript Walkthrough

Same design as the C# implementation — same domain abstractions, same assertion messages, same builder pattern — but the discovery mechanism is fundamentally different because TypeScript has no runtime reflection.

## Discovery via Object Keys

TypeScript cannot inspect a class's methods at runtime the way C# reflection can. Instead, the runner takes a `TestSuite` which is simply `Record<string, () => void>` — a plain object where each key is a test name and each value is a test function. The runner iterates `Object.entries(suite)` and calls each function.

This is the most honest approach for TS: no decorators, no prototype crawling, no runtime magic. The caller builds a suite as a literal object or via the builder. This makes the TS implementation the simplest of the three — discovery is just property enumeration.

See `src/TestRunner.ts`.

## The Builder Is Trivially Simple

Where C# needed dynamic assembly emission to create a `Type` with attributed methods, the TS `TestSuiteBuilder` just accumulates entries in a `Record<string, () => void>`. `.withPassingTest("name")` adds a no-op function; `.withFailingTest("name", "msg")` adds one that throws `AssertionFailedException`; `.build()` returns a shallow copy of the record.

This is the starkest divergence from C# — the builder is ~20 lines vs ~50 lines of IL emission. The builder pattern still earns its keep by making test setup read as English, but the implementation complexity collapses because TS's object model is just a dictionary.

## Error Classification

The runner distinguishes FAIL from ERROR by `instanceof AssertionFailedException`. Since TS's `instanceof` works on the prototype chain, we extend `Error` properly (with `this.name = 'AssertionFailedException'` in the constructor). Any other `Error` or thrown value becomes an ERROR result.

## Assertions

The three assertion functions are standalone exports (not methods on a class), matching TS idiom for stateless utility functions. Messages are byte-identical to C# and Python:

- `assertEqual(5, 3)` → `"expected 5 but got 3"`
- `assertTrue(false)` → `"expected true"`
- `assertThrows(() => {})` → `"expected exception"`

## Scenario Map

Twelve scenarios from [`../SCENARIOS.md`](../SCENARIOS.md) across two test files: `testRunner.test.ts` (scenarios 1–6) and `assertions.test.ts` (scenarios 7–12).

## How to Run

```bash
cd roll-your-own-test-framework/typescript
npm install
npx vitest run
```
