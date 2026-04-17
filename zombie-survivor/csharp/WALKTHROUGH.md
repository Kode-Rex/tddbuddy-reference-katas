# Zombie Survivor — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the domain design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice.

Rather than stepping through thirty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Game ──owns──> Survivor[]
  │               │
  │               ├── Wounds : int
  │               ├── Experience : int
  │               ├── Level : enum
  │               ├── Equipment[] (slot: InHand | InReserve)
  │               ├── Skills[] (PlusOneAction, Hoard, Sniper, Tough)
  │               ├── ReceiveWound() : bool
  │               ├── Equip(name) — throws EquipmentCapacityExceededException
  │               └── KillZombie()
  │
  ├── GameLevel : Level (derived from living survivors)
  ├── History : HistoryEntry[] (append-only event log)
  ├── AddSurvivor() — throws DuplicateSurvivorNameException
  ├── WoundSurvivor() — records wound, death, game-end events
  └── KillZombie() — records level-up and game-level-change events
```

## Why Domain-Specific Exceptions

`EquipmentCapacityExceededException` and `DuplicateSurvivorNameException` are named for the invariant they protect, not for the mechanism of rejection. A stack trace reading `EquipmentCapacityExceededException: Cannot carry more than 5 pieces of equipment.` tells both a developer and an AI agent exactly what went wrong — no grep required.

The message strings are byte-identical across C#, TypeScript, and Python. The exception type names are language-idiomatic.

## Why `SurvivorBuilder`

Most tests need a survivor in a specific state — wounded, equipped, experienced. Without a builder, every test arranges loops of `KillZombie()` and `Equip()` calls. With `new SurvivorBuilder().WithZombieKills(7).Build()`, setup is one line that reads as domain language.

The builder applies operations in a deliberate order: kills first (to set experience/level), then equipment (which depends on capacity), then wounds (which reduce capacity). This ordering prevents invalid intermediate states without the test author needing to think about it.

## Why `HistoryBuilder`

Game-level tests need a game with specific history — survivors added, wounds dealt, zombies killed. `HistoryBuilder` records these as actions replayed at build time. It returns both the `Game` and its `FixedClock`, following the tuple-return pattern established in `bank-account`.

## Why `IClock`

History entries carry timestamps. Without clock injection, tests would assert on `DateTime.Now` and flake on slow machines. `FixedClock` makes every timestamp deterministic.

## Why `Game` Orchestrates Events

`Game.WoundSurvivor()` doesn't just delegate to `Survivor.ReceiveWound()` — it also records the wound event, checks for death (recording that too), and checks for game end. This keeps the event-logging concern in one place rather than scattering it across Survivor methods. Survivor stays a pure domain entity; Game is the coordinator.

## Why Equipment Uses an Enum Slot

`EquipmentSlot.InHand` vs `EquipmentSlot.InReserve` is computed at equip time based on current hand count. The domain rule is simple — first two go in hand, rest in reserve — but making it explicit lets tests assert on slot placement directly.

## Scenario Map

Thirty-two scenarios from [`../SCENARIOS.md`](../SCENARIOS.md) live across six test files, one `[Fact]` per scenario:

- `SurvivorTests.cs` — scenarios 1–7
- `EquipmentTests.cs` — scenarios 8–13
- `GameTests.cs` — scenarios 14–17
- `ExperienceAndLevelTests.cs` — scenarios 18–22
- `HistoryTests.cs` — scenarios 23–29
- `SkillTests.cs` — scenarios 30–32

## How to Run

```bash
cd zombie-survivor/csharp
dotnet test
```
