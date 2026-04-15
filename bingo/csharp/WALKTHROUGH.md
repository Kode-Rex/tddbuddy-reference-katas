# Bingo — C# Walkthrough

This kata ships in **middle gear** — the C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one primary entity (`Card`), one small test-folder builder (`CardBuilder`), a rich `WinPattern` value type, and a single domain-specific exception.

## Scope — Pure Domain Only

No card generator, no caller/draw service, no game loop, no UI. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list. The point of the kata at F2 is card state plus win detection; random card construction and call sequencing are collaborator-shaped and belong in F3.

## The Design at a Glance

```
CardMessages / CardDimensions — spec strings + named numeric constants
WinPatternKind enum           — None | Row | Column | DiagonalMain | DiagonalAnti
WinPattern (record struct)    — rich return type, carries the row/column index

Card
  ├── NumberAt(row, col)     : int?    (null at the free space)
  ├── IsMarked(row, col)     : bool
  ├── Mark(number)           : void    (throws on out-of-range; no-op off-card)
  ├── HasWon()               : bool
  └── WinningPattern()       : WinPattern   (first completed line, or None)

CardBuilder (tests/)
  ├── WithNumberAt(row, col, number)
  ├── WithMarkAt(row, col)              — test-side escape hatch for scan-order scenarios
  └── Build() : Card
```

Five files under `src/Bingo/` (Card, CardExceptions, CardMessages + CardDimensions grouped, WinPattern, csproj) and one builder under `tests/Bingo.Tests/`.

## Why `WinPattern` Is Richer Than a `bool`

The kata spec says a player may call bingo on a row, column, or diagonal line. A `bool hasWon()` discards the information the domain actually contains — *which* line completed. An enum like `WinPatternKind` answers that but forgets which row or column (there are five of each). A `readonly record struct` of `(Kind, Index)` carries both, with static factories for readability:

```csharp
WinPattern.Row(0)           // "the top row"
WinPattern.Column(4)        // "the O column"
WinPattern.DiagonalMain     // (0,0) → (4,4)
WinPattern.DiagonalAnti     // (0,4) → (4,0)
WinPattern.None             // no line complete
```

Tests assert on the exact pattern (`Should().Be(WinPattern.Row(0))`) — the spec contract includes *which* line the player won on, not merely that they won. `HasWon()` is the convenience derived from the richer result, not the primary signal.

## Why `Card` Is Mutable

`Mark(number)` mutates the receiver. Every sibling F2 kata in this repo leans on immutability (see `tic-tac-toe/`), and the temptation here was to do the same — `Card Mark(int n) => new Card(...)`. Two things pushed the other way:

1. **Bingo's domain is sequential calls on a single card.** A caller announces numbers over time; a real player mutates their card as numbers are called. Returning a new `Card` per call would model that correctly but also force the test to thread the new reference through every assertion, adding ceremony to every multi-mark scenario.
2. **The free space is a singleton fact of a card**, not a state transition. With an immutable card, the free space still has to be pre-marked at construction. That's fine. But the five `card.Mark(n)` calls that complete a row then read more naturally as a procedural sequence than as a monadic chain.

Tic-Tac-Toe's `Place` returns a new board because *validation* is the teaching point — attempts to place on occupied cells or out of bounds are domain errors. Bingo's `Mark` has only one validation (range), and the off-card no-op deliberately swallows information rather than branching on it. Mutable matches bingo; immutable matches tic-tac-toe.

## Why `Number?` Instead of a `Cell` Type

A `Cell` class or record with `Number` and `IsMarked` fields was considered and rejected. Two arrays — `int?[,] _numbers` and `bool[,] _marks` — read more cleanly for the three hot-path operations (`NumberAt`, `IsMarked`, `Mark`) because each one touches exactly one array. A `Cell[,]` forces every access through a two-step dereference and every marking update through a new-Cell allocation or a mutable-field carveout. The `int?` choice also makes the free space semantically obvious: `NumberAt(2, 2) == null` *is* the free space.

Named constants live in `CardDimensions`: `CardSize = 5`, `FreeRow = 2`, `FreeColumn = 2`, `MinNumber = 1`, `MaxNumber = 75`. This is F2's "named constants for business numbers" rule — the BINGO column ranges themselves are not enforced by `Card` (the builder places whatever the test needs), but the card size, range bounds, and free-space coordinates are named.

## Why the Builder Has `WithMarkAt` (Plus `WithNumberAt`)

Eleven of the twelve scenarios are fine with `WithNumberAt` + the natural `Mark(...)` API, exactly as the SCENARIOS.md ubiquitous-vocabulary entry describes. The twelfth — *"winning-pattern scan order is rows, then columns, then diagonals"* — needs a board that has completed **both** row 0 and column 0 without walking through numbers. `WithMarkAt(row, col)` is the test-side escape hatch that lets that scenario describe the winning state directly:

```csharp
var card = new CardBuilder()
    .WithMarkAt(0, 0).WithMarkAt(0, 1) // ... row 0 fully marked
    .WithMarkAt(1, 0).WithMarkAt(2, 0) // ... column 0 fully marked
    .Build();
```

This keeps the scan-order scenario's *intent* visible in the setup. It is a test-folder affordance only; production code has no way to mark a cell except through `Card.Mark(number)`.

## Why Marking a Missing Number Is a No-Op

The kata's domain rule is that a caller announces numbers at large; only some land on any given card. A thrown exception for "number not on this card" would force every test that simulates a real game to wrap every `Mark` call in a try/catch, which is the inverse of the real domain behaviour. The spec names exactly one marking-error condition — number out of 1..75 — and that is the only condition that throws.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/Bingo.Tests/CardTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd bingo/csharp
dotnet test
```
