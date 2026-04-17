# Game of Life

Conway's Game of Life on an unbounded infinite grid, modeled as a set of living cell coordinates. Each generation applies four simple rules to produce the next. Great for practicing **set-based domain modeling**, **immutable value types**, and **test data builders** without collaborator interfaces.

## What this kata teaches

- **Test Data Builders** — `GridBuilder().WithLivingCellsAt((0,0), (0,1), …).Build()` returns an immutable `Grid`. No collaborators, no tuple — the grid is the only entity.
- **Immutable Value Types** — `Cell` is a `(row, col)` coordinate with value equality. `Grid` is an immutable snapshot; `tick()` returns a new `Grid` rather than mutating.
- **Set-Based Domain Modeling** — The grid is an unbounded infinite plane modeled as a `Set<Cell>`. No 2D array, no boundary questions, no size parameter. A dead cell with no living neighbors simply doesn't exist.
- **Pattern Verification** — Still lifes (Block), oscillators (Blinker), and spaceships (Glider) are the GoL vocabulary. Tests name these patterns and verify multi-tick behavior.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
