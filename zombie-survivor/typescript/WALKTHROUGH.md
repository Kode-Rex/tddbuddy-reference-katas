# Zombie Survivor — TypeScript Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### String Enums for Level and Skill

TypeScript's `enum Level { Blue = 'Blue', ... }` gives both type safety and human-readable serialization. History messages embed the level name directly (`"Level up: Alice reached Yellow."`) — string enums make this natural without a separate `toString` mapping.

### `maxLevel` as a Free Function

C# uses `Enumerable.Max()` over the enum's ordinal values. TypeScript enums have no inherent ordering, so `maxLevel` uses an explicit `LevelOrder` lookup table. The function lives alongside the enum in `Level.ts` — colocated, not scattered.

### Exception Classes Extend `Error`

TypeScript has no checked exceptions. `EquipmentCapacityExceededException` and `DuplicateSurvivorNameException` extend `Error` and set `this.name` for reliable `instanceof` checks. The message strings are byte-identical to the C# and Python versions.

### `readonly` Arrays via Return Types

`get survivors(): readonly Survivor[]` prevents callers from mutating the internal array. This mirrors C#'s `IReadOnlyList<T>` without a wrapper type.

### Builder Pattern Matches C#

`SurvivorBuilder` and `HistoryBuilder` follow the same fluent API as C#. The builder applies operations in the same order — kills, equipment, wounds — to prevent invalid intermediate states.

## Scenario Map

Thirty-two scenarios live across six test files, one `it()` per scenario. Test names match `SCENARIOS.md` verbatim in sentence form.

## How to Run

```bash
cd zombie-survivor/typescript
npm install
npm test
```
