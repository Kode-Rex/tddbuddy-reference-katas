# Tic-Tac-Toe — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one primary entity (`Board`), one small test-folder builder (`BoardBuilder`), a pair of simple value enums (`Cell`, `Outcome`), and three domain-specific exceptions.

## Scope — Pure Domain Only

No UI, no CLI, no persistence, no AI opponent. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Design at a Glance

```
Cell enum: Empty | X | O          Outcome enum: InProgress | XWins | OWins | Draw

Board (immutable)
  ├── CellAt(row, col) : Cell
  ├── CurrentTurn()   : Cell          (X if xs==os else O)
  ├── Outcome()       : Outcome       (scans 8 winning lines)
  └── Place(row, col) : Board         (returns new board, throws on invalid)

BoardBuilder (tests/)
  ├── WithXAt(row, col)
  ├── WithOAt(row, col)
  └── Build() : Board
```

Five files under `src/TicTacToe/` (Cell, Outcome, the three exception types grouped in one file, the messages/dimensions constants, the Board) and one builder under `tests/TicTacToe.Tests/`.

## Why `Board` Is Immutable

Every call to `Place(row, col)` returns a **new** `Board` rather than mutating the receiver. Three wins fall out of that choice:

1. **Tests can hold references to intermediate states** and assert on them without worrying that a later move corrupted earlier assertions. `var afterFirst = new Board().Place(0, 0); var afterSecond = afterFirst.Place(1, 1); afterFirst.CellAt(1, 1).Should().Be(Cell.Empty);` just works.
2. **The builder pattern composes naturally** — `WithXAt` and `WithOAt` do not need to hand back fresh `Board` instances per chained call; they mutate an internal `Cell[,]` privately and only materialise a `Board` at `Build()`.
3. **Accidental history-sharing is impossible.** Each `Place` clones the grid, so there is no way to end up with two boards pointing at the same underlying array.

Mutable `Game` would read fine for a CLI loop, but for an F2 test-as-spec reference it hides the state transitions behind side effects. Immutable matches the "record style" of `password/` and `pagination/`.

## Why Turn Is Derived, Not Stored

`CurrentTurn()` is computed on demand: if the board has the same number of `X` and `O` marks it is `X`'s turn, otherwise it is `O`'s. The alternative is storing a `Cell _nextTurn` field and flipping it on every `Place`. Deriving it is cheaper in lines of code and impossible to get out of sync with the cells — the cells *are* the single source of truth about whose move is next.

The same reasoning applies to `Outcome()`: it scans the eight winning lines (three rows, three columns, two diagonals) on every call rather than tracking a running game state. For a 3x3 grid this is microseconds, and it means the board can never lie about whether `X` has already won.

## Why `WinningLines` Is a Table

Eight lines, each a trio of `(row, col)` tuples. The alternative is three nested loops plus two diagonal checks. The table version reads in the order a human would read it — "top row, middle row, bottom row, left column, middle column, right column, main diagonal, anti-diagonal" — and a reader verifies correctness by eyeballing the coordinates, not by decoding loop indices. Named constant: `BoardSize = 3`.

## Why Domain-Specific Exception Types

`CellOccupiedException`, `OutOfBoundsException`, `GameOverException`. The temptation is to throw `InvalidOperationException` everywhere and distinguish via message string. Resisted because:

- The kata spec names three distinct invalid-move conditions. Naming each exception type matches the spec 1:1.
- Tests can catch the specific type without parsing strings: `act.Should().Throw<CellOccupiedException>()`.
- A stack trace or logged rejection names the domain condition, not a generic "something was invalid."

Message strings (`"cell already occupied"`, `"coordinates out of bounds"`, `"game is already over"`) are byte-identical across C#, TypeScript, and Python, codified in `BoardMessages`. The type names differ by language idiom — TS has `CellOccupiedError` (subclass of `Error`), Python has `CellOccupiedError` (subclass of `ValueError`/`RuntimeError`) — but the strings the tests assert on do not.

## Why `BoardBuilder` Exists — The F2 Signature Pattern

Twelve scenarios need a dozen slightly different board states. Without a builder, every "X wins top row" test would spell out:

```csharp
var board = new Board().Place(0, 0).Place(1, 0).Place(0, 1).Place(1, 1);
// now board is X's turn with X at (0,0),(0,1) and O at (1,0),(1,1) — but you had to read four moves to know that
```

With `BoardBuilder`, the same setup reads:

```csharp
var board = new BoardBuilder()
    .WithXAt(0, 0).WithXAt(0, 1)
    .WithOAt(1, 0).WithOAt(1, 1)
    .Build();
```

Fifteen lines of builder, twenty with braces and namespace. That is the F2 budget and the builder spends it on clarity: the test shows *the board state under test*, not the move sequence a player would have used to reach it. `BoardBuilder` is allowed to skip turn-alternation rules because it is a test-folder synthesiser of board states, not a player. `CurrentTurn()` on the built board still derives correctly from the ratio of X to O counts.

## What Is Deliberately Not Modeled

- **No renderer** — `Board.ToString()` would be a nice-to-have; tests use `CellAt` and `Outcome` directly.
- **No move history** — `place` forgets the previous board. Tests that need a chain of states chain the calls.
- **No AI opponent** — no strategy interface, no collaborator injection.
- **No variable board size** — `BoardSize = 3` is a constant, not a parameter.

Each omission points at an F3 extension. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/TicTacToe.Tests/BoardTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd tic-tac-toe/csharp
dotnet test
```
