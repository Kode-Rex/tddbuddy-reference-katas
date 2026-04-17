# Jelly vs Tower — TypeScript Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `ColorType` as a String Enum

TypeScript enums with string values (`Blue = 'Blue'`) give readable debug output and safe type narrowing. The damage table uses a string key (`${tower}:${level}:${jelly}`) built from these values, which reads well and avoids the tuple-key workaround that a `Map<[...], ...>` would need.

### Domain Exceptions Extend `Error`

TypeScript has no custom exception hierarchy beyond `Error`. `InvalidHealthException` and `InvalidLevelException` extend `Error` and set `this.name` so that `instanceof` checks work and stack traces label the exception type. The message strings are byte-identical to the C# and Python implementations.

### `RandomSource` as an Interface

Structural typing means `FixedRandomSource` just needs to have a `next(min, max)` method. Stating `implements RandomSource` documents intent and catches typos at compile time.

### `readonly` on Entity Fields

`Tower` fields are `readonly` — they're set at construction and never mutate. `Jelly` has a private `_health` field behind a getter, since health changes during combat.

### `Arena` Takes Readonly Arrays

The arena constructor accepts `readonly Tower[]` and `readonly Jelly[]`, signaling that it doesn't modify the input arrays. Internally it iterates them; the jelly mutation happens through `takeDamage()` on the jelly itself.

## Scenario Map

Twenty-five scenarios live in `tests/jellyVsTower.test.ts`, one `it()` per scenario.

## How to Run

```bash
cd jelly-vs-tower/typescript
npm install
npm test
```
