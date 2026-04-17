# Maze Walker

Navigate a 2D grid maze from start to end using breadth-first search. The walker finds the shortest path through open cells, avoiding walls. Great for practicing **grid-based domain modeling**, **pathfinding algorithms**, **test data builders**, and **domain exception types**.

## What this kata teaches

- **Test Data Builders** — `MazeBuilder().WithLayout("S.#\n..E").Build()` constructs a maze from string art. `WalkerBuilder().WithMaze(maze).Build()` configures a walker. Builders make maze topology readable in tests without coordinate noise.
- **Domain Exception Types** — `NoStartCellException`, `NoEndCellException`, `MultipleStartCellsException`, `MultipleEndCellsException` name the rejection in domain language. Tests catch the meaningful type.
- **Immutable Value Types** — `Cell` is a `(row, col)` coordinate with value equality. `CellType` is an enumeration of the four cell kinds.
- **Pathfinding** — BFS guarantees the shortest path in an unweighted grid. The walker explores neighbors level-by-level, reconstructing the path from a parent map once the end is reached.
- **String-Art Construction** — Mazes are specified as human-readable grids (`#` walls, `.` open, `S` start, `E` end), parsed into the domain model by the builder.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
