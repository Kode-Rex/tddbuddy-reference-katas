# Snake Game

Classic Snake game logic on a bounded rectangular grid. The snake moves, eats food, grows, and dies on wall or self collision. No UI — pure domain logic with an injectable food spawner for testability.

## What this kata teaches

- **Test Data Builders** — `SnakeBuilder().MovingDown().WithBodyAt((1,0), (0,0)).Build()` constructs a multi-segment snake with explicit direction and body. `BoardBuilder(width, height)` sets the grid dimensions. Together they build readable game scenarios without constructor noise.
- **State Machine** — `Game` transitions between `Playing`, `GameOver`, and `Won`. The `tick()` method is the single entry point for state transitions, making the game loop testable as a pure state machine.
- **Injectable Collaborator** — `FoodSpawner` is an injectable function that selects the next food position from available empty cells. Tests provide deterministic spawners; production code provides a random one.
- **Collision Detection** — Wall collisions (bounded grid) and self-collisions (snake body lookup) are tested independently before combining in integration scenarios.
- **Direction as Value Type** — Direction is an enum with an `isOpposite` check, preventing illegal 180-degree reversals.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
