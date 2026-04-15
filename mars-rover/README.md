# Mars Rover

A rover sits on a rectangular Martian grid at position `(x, y)` facing one of `N | E | S | W` and executes a string of commands — `F` (forward), `B` (backward), `L` (turn left), `R` (turn right). The grid wraps (Mars is a sphere), and a set of known obstacles may block movement. The API is a single pure method: `rover.execute(commands)` returns a new `Rover` at the post-execution position, with a `status` reporting whether the sequence completed or was stopped by an obstacle.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. One primary entity (`Rover`), two small test-folder builders — `RoverBuilder` (position / heading / grid / obstacles) and `CommandBuilder` (fluent sugar for building a command string). Typed enums for `Direction` (`N`, `E`, `S`, `W`) and `Command` (`F`, `B`, `L`, `R`), and named constants for the default grid bounds. No collaborators, no object mothers.

```csharp
var rover = new RoverBuilder()
    .At(0, 0).Facing(Direction.South)
    .OnGrid(100, 100)
    .Build();

var after = rover.Execute(new CommandBuilder()
    .Forward().Forward()
    .Left()
    .Forward().Forward()
    .Build());
// after.Position == (2, 2), after.Heading == Direction.East, after.Status == MovementStatus.Moving
```

vs. hand-rolling the setup and the command string at every call site, which obscures both the scenario under test and the command sequence a player would issue.

## Coordinate Convention

- `(0, 0)` is the top-left corner of the grid.
- `x` increases eastward; `y` increases southward.
- `N` forward is `y - 1`; `S` forward is `y + 1`; `E` forward is `x + 1`; `W` forward is `x - 1`.
- `L` rotates counter-clockwise: `N → W → S → E → N`.
- `R` rotates clockwise: `N → E → S → W → N`.
- Wrapping: stepping off one edge re-enters from the opposite edge. A rover at `(0, 0)` facing `N` that moves forward lands at `(0, gridHeight - 1)`.

The kata brief's example (`(0, 0)` facing `S` on a 100x100 grid, commands `FFLFF` → `(2, 2)`) follows from this convention.

## Obstacle Detection

A `Rover` may be constructed with zero or more obstacles at fixed `(x, y)` coordinates. Before each `F` or `B` move, the rover checks the destination square after wrapping. If the destination holds an obstacle, the rover **stops at its current square**, its `status` flips to `Blocked`, its `lastObstacle` records the blocking square, and any remaining commands in the sequence are discarded. Turn commands (`L`, `R`) never trigger blocking because they do not change position.

## Scope — Pure Domain Only

The reference covers the rover, the grid, wrapping, obstacle detection, and the command interpreter. **No renderer, no CLI loop, no command parser for alternative notations, no multi-rover coordination.** Those each introduce collaborators and tip the kata into F3.

### Stretch Goals (Not Implemented Here)

- **Renderer** — ASCII grid printing showing rover + obstacles
- **CLI loop** — interactive command entry, input validation reporting
- **Alternative command formats** — whitespace, lowercase, comma-separated
- **Multi-rover worlds** — rovers as obstacles to each other
- **Variable-speed commands** — numeric prefixes (`3F`, `2L`)

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference satisfies.
