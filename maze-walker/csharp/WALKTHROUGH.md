# Maze Walker — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Cell (readonly record struct)
   └── CardinalNeighbors() → 4 adjacent Cells (up, down, left, right)

CellType (enum)
   └── Open | Wall | Start | End

Maze (immutable)
   ├── owns → CellType[,] grid
   ├── Start → Cell
   ├── End → Cell
   ├── CellTypeAt(row, col) → CellType?
   └── IsWalkable(cell) → bool

Walker
   ├── owns → Maze
   └── FindPath() → IReadOnlyList<Cell>

MazeBuilder (tests only)
   └── WithLayout(string).Build() → Maze

WalkerBuilder (tests only)
   └── WithMaze(maze).Build() → Walker
```

## Why `Cell` Is a `readonly record struct`

A cell is a coordinate — pure value semantics. Two cells at `(2, 3)` are the same cell regardless of how they were created. `readonly record struct` gives value equality, immutability, and `GetHashCode` for free, which matters because the walker stores visited cells in a `HashSet`. The `CardinalNeighbors()` method yields the four orthogonal neighbors — no diagonals, matching the maze-walking constraint.

See `src/MazeWalker/Cell.cs`.

## Why the Maze Is a 2D Array, Not a Set

Unlike Game of Life's infinite plane, a maze is a bounded rectangular grid with walls. A 2D `CellType[,]` array maps directly to the string-art representation: row and column indices correspond to visual position. The maze knows its dimensions, start, and end — all validated at construction time.

See `src/MazeWalker/Maze.cs`.

## Why Domain Exception Types

Four exception types name the four ways a maze layout can be invalid: `NoStartCellException`, `NoEndCellException`, `MultipleStartCellsException`, `MultipleEndCellsException`. Each carries the message "Maze must have exactly one start/end cell." byte-identical across all three languages. Tests catch the specific exception type, and a reader sees the domain constraint in the stack trace without decoding a generic `ArgumentException`.

See `src/MazeWalker/MazeExceptions.cs`.

## Why BFS, Not DFS or A*

BFS guarantees the shortest path in an unweighted grid. DFS would find *a* path but not necessarily the shortest. A* is optimal too but requires a heuristic — overkill for a unit-cost grid where BFS already runs in O(rows * cols). The `FindPath()` method builds a `cameFrom` dictionary during exploration, then reconstructs the path by walking backward from end to start.

See `src/MazeWalker/Walker.cs`.

## Why Two Builders

`MazeBuilder` parses string-art into a validated `Maze`. Without it, every test would construct a `CellType[,]` array by hand — unreadable for anything beyond a trivial grid. The string-art format (`#` walls, `.` open, `S` start, `E` end) makes test mazes visually scannable.

`WalkerBuilder` is minimal — it just wraps the maze-to-walker wiring. Its value is consistency: every test reads `new WalkerBuilder().WithMaze(maze).Build()`, making the pattern recognizable even though the builder is thin.

See `tests/MazeWalker.Tests/MazeBuilder.cs` and `tests/MazeWalker.Tests/WalkerBuilder.cs`.

## Test File Organization

Twenty scenarios across six test files:

- `MazeConstructionTests.cs` — scenarios 1–6 (building, validation, exceptions)
- `TrivialPathTests.cs` — scenarios 7–9 (adjacent, horizontal, vertical)
- `NoPathTests.cs` — scenarios 10–11 (blocked, unreachable)
- `ShortestPathTests.cs` — scenarios 12–14 (around walls, two routes, winding)
- `LargerMazeTests.cs` — scenarios 15–16 (5x5, exploration)
- `PathPropertyTests.cs` — scenarios 17–20 (starts at start, ends at end, adjacency, no walls)

One `[Fact]` per scenario; test names match the SCENARIOS titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd maze-walker/csharp
dotnet test
```
