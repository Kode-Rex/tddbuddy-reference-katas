# Game of Life — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through eighteen red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Cell (readonly record struct)
   └── Neighbors() → 8 adjacent Cells

Grid (immutable)
   ├── owns → HashSet<Cell>  (the living cells)
   ├── Tick() → Grid          (next generation)
   ├── IsAlive(row, col) → bool
   └── LivingCells → IReadOnlySet<Cell>

GridBuilder (tests only)
   └── WithLivingCellsAt(params (int,int)[]).Build() → Grid
```

## Why `Cell` Is a `readonly record struct`

A cell is a coordinate — pure value semantics. Two cells at `(3, 4)` are the same cell regardless of how they were created. `readonly record struct` gives value equality, immutability, and `GetHashCode` for free, which matters because the grid stores cells in a `HashSet`. The `Neighbors()` method lives on `Cell` because "which eight cells surround me" is intrinsic to a coordinate, not to the grid.

See `src/GameOfLife/Cell.cs`.

## Why the Grid Is a `HashSet<Cell>`, Not a 2D Array

The Game of Life plays on an infinite plane. A 2D array forces a fixed boundary — then every test must reason about width, height, and what happens at the edges. A `HashSet<Cell>` makes the grid unbounded by construction: living cells are the set members, dead cells are everything else (implicitly infinite, never enumerated). The Glider test verifies that a pattern translates across the plane without hitting any wall.

This is the canonical GoL representation in the literature, and it maps directly to the domain vocabulary: a `Grid` *is* a set of living cells.

See `src/GameOfLife/Grid.cs`.

## Why `Grid` Is Immutable

`Tick()` returns a **new** `Grid` rather than mutating the current one. GoL rules are applied simultaneously — every cell's fate is determined by the *current* generation, not by partially-updated state. Immutability makes this guarantee structural: there's no way for `Tick()` to accidentally read from the generation it's building.

It also means tests can hold a reference to the original grid and compare it with the result of `Tick()`, which is exactly what the still-life and oscillator tests do.

## Why `Tick()` Uses a Neighbor-Count Dictionary

The naive approach — iterate every cell on an infinite plane — doesn't work. Instead, `Tick()` builds a `Dictionary<Cell, int>` counting how many living neighbors each candidate cell has. Only cells adjacent to at least one living cell appear in this dictionary, so the work is proportional to the living population, not to the plane.

After counting, the four GoL rules collapse to two lines:
- Count is 3 → the cell lives (whether currently alive or dead — reproduction and survival with 3 neighbors).
- Count is 2 and currently alive → the cell survives.

Everything else dies or stays dead.

## Why `GridBuilder` Is Minimal

GoL has no collaborators — no clock, no notifier, no capacity. The builder's only job is to make test setup readable: `.WithLivingCellsAt((0,0), (0,1), (0,2))` reads as "a horizontal blinker" without the noise of `new HashSet<Cell> { new Cell(0,0), ... }`. It's a convenience wrapper, not a rich builder like `CacheBuilder` — the domain doesn't need more.

See `tests/GameOfLife.Tests/GridBuilder.cs`.

## Why No Domain Exceptions

Unlike `memory-cache` or `library-management`, GoL has no invariants to reject. An empty grid is valid. Negative coordinates are valid (the plane is infinite). There's no capacity, no TTL, no "book not found." The domain is pure transformation, so there are no domain exception types.

## Test File Organization

Eighteen scenarios across five test files:

- `EmptyAndTrivialTests.cs` — scenarios 1–2
- `IndividualRuleTests.cs` — scenarios 3–8
- `StillLifeTests.cs` — scenarios 9–10
- `OscillatorTests.cs` — scenarios 11–13
- `SpaceshipTests.cs` — scenario 14
- `GridQueryTests.cs` — scenarios 15–18

One `[Fact]` per scenario; test names match the SCENARIOS titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd game-of-life/csharp
dotnet test
```
