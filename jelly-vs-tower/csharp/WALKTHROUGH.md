# Jelly vs Tower — C# Walkthrough

This kata ships in **middle/high gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-five red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Tower ──attacks──> Jelly
  │                  │
  ├── Color          ├── Color
  ├── Level (1–4)    ├── Health
  └── Id             └── IsAlive

DamageTable.CalculateDamage(tower, jelly, random) : int
Arena ──orchestrates──> [Tower], [Jelly], IRandomSource
  └── ExecuteRound() : CombatLog[]
```

Six types. Each earns its keep.

## Why `ColorType` Is an Enum

Colors could be strings — `"Blue"`, `"Red"`, `"BlueRed"`. But string comparison is case-sensitive, typo-prone, and impossible for the compiler to check. An enum makes invalid colors unrepresentable and gives the damage table a reliable lookup key.

## Why `DamageTable` Is Static

The damage rules don't change at runtime. They're a specification — a lookup table the game designer wrote. Making it a static class with a dictionary models that reality: the rules are fixed, the table is initialized once, and nothing injects or overrides it.

The table key is a tuple `(ColorType Tower, int Level, ColorType Jelly)`, which makes lookup a single dictionary hit. For `BlueRed` jellies, the table is consulted twice (once for each color column), and the higher damage wins.

## Why `IRandomSource`, Not `Random`

Five scenarios need deterministic damage. If the domain called `Random.Next()` directly, tests would need seeds or statistical assertions ("damage should be between 2 and 5, run it 1000 times"). That's fragile and slow.

`IRandomSource.Next(min, max)` is the collaboration boundary. `FixedRandomSource` in the test project always returns the same value, making damage assertions exact. The production caller passes a real `Random`-backed implementation.

See `src/JellyVsTower/IRandomSource.cs` and `tests/JellyVsTower.Tests/FixedRandomSource.cs`.

## Why Domain-Specific Exceptions

`InvalidHealthException` and `InvalidLevelException` are thrown when construction invariants are violated. They name the rejection in domain language — "health must be strictly positive" and "level must be between 1 and 4." Tests catch the meaningful type and assert on the message, which is byte-identical across all three language implementations.

## Why `TowerBuilder` and `JellyBuilder`

Most tests need a tower or jelly with specific attributes. Without builders, every test writes three constructor arguments, most of which are irrelevant to the scenario. `new TowerBuilder().WithColor(ColorType.Blue).WithLevel(4).Build()` declares only what matters; the builder fills sensible defaults for the rest.

## Why `Arena` Orchestrates

The combat flow is a coordination responsibility: iterate towers, find the first alive jelly, calculate damage, apply it, log it. Putting this in `Arena.ExecuteRound()` keeps `Tower` and `Jelly` focused on their own state, and gives tests a single method to call for round-level assertions.

## Scenario Map

The twenty-five scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/JellyVsTower.Tests/JellyVsTowerTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd jelly-vs-tower/csharp
dotnet test
```
