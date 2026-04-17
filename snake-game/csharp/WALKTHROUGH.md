# Snake Game — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-three red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Position (readonly record struct)
   — (X, Y) coordinate on the board

Direction (enum + extensions)
   ├── IsOpposite(other) → bool
   └── Move(position) → Position

GameState (enum)
   — Playing | GameOver | Won

Snake
   ├── Body → IReadOnlyList<Position>   (head first)
   ├── Head → Position
   ├── Direction → Direction
   ├── Move(grow) → Snake              (new snake, immutable move)
   └── ChangeDirection(dir) → Snake    (no-op if opposite)

Game (aggregate)
   ├── Snake, Food, Score, State
   ├── Tick()                           (advance one step)
   └── ChangeDirection(dir)

BoardBuilder (tests only)
   └── WithSize(w, h).WithSnake(s).WithFoodAt(x, y).WithFoodSpawner(fn).Build() → Game

SnakeBuilder (tests only)
   └── At(x, y).MovingDown().WithBodyAt(positions).Build() → Snake
```

## Why `Position` Is a `readonly record struct`

A position is a coordinate — pure value semantics. Two positions at `(3, 4)` are the same position regardless of how they were created. `readonly record struct` gives value equality, immutability, and `with` expression support for the `Move` method in `DirectionExtensions`.

## Why Direction Uses Extension Methods

`Direction` is an enum — it cannot have instance methods in C#. The two operations on a direction (checking if another direction is its opposite, and computing the next position) live as extension methods in `DirectionExtensions`. This keeps the enum clean while giving it domain behavior.

`IsOpposite` exists because the game rule "you cannot reverse direction" is a property of the direction pair, not of the game or the snake. Placing it on `Direction` means the rule is named and testable independently.

## Why `Snake` Returns New Instances

`Move()` and `ChangeDirection()` both return a **new** `Snake` rather than mutating the current one. This makes the game loop in `Tick()` clear: compute the new head, check collisions against the **current** body, then replace the snake. There's no risk of reading partially-updated state during collision detection.

The body is stored head-first as a `List<Position>`. Movement is: prepend the new head, optionally remove the tail. This makes growth trivial — skip the tail removal when eating food.

## Why `Game` Has an Internal Constructor

The public constructor creates a standard new game: snake at `(0,0)` moving right, score zero, food spawned via the injected spawner. The `internal` constructor (visible to tests via `InternalsVisibleTo`) accepts all state directly, allowing `BoardBuilder` to place the snake, food, and score at arbitrary positions without reflection.

This is the standard pattern for making aggregates testable without exposing mutable setters.

## Why `FoodSpawner` Is a `Func`

The kata spec says "accept an injectable random source for testability." Rather than defining an `IFoodSpawner` interface for a single method, a `Func<IReadOnlyList<Position>, Position>` does the job. The function receives the list of empty cells and returns the chosen position. Tests inject a deterministic lambda; production code would pass `emptyCells => emptyCells[random.Next(emptyCells.Count)]`.

## Why No Domain Exceptions

Snake Game has no business invariants that reject input with a named error. An invalid direction change is silently ignored (the spec says "direction stays"). Hitting a wall or the snake's own body transitions to `GameOver` — it's a state transition, not an error. Unlike `library-management` or `parking-lot`, there are no "book not found" or "lot is full" rejection scenarios that warrant domain exception types.

## Collision Detection

Wall collision checks whether the new head position is outside the board bounds. Self-collision checks whether the new head position is already occupied by the snake's body. Both checks happen **before** the snake moves, so the collision is against the current body layout. This is important: if the snake grows, the tail stays — but we check against the pre-move body, which is correct because the tail hasn't moved yet.

## Test File Organization

Twenty-three scenarios across seven test files:

- `InitialStateTests.cs` — scenarios 1–4
- `BasicMovementTests.cs` — scenarios 5–8
- `DirectionChangeTests.cs` — scenarios 9–13
- `EatingFoodTests.cs` — scenarios 14–16
- `WallCollisionTests.cs` — scenarios 17–20
- `SelfCollisionTests.cs` — scenario 21
- `GameOverBehaviorTests.cs` — scenario 22
- `WinningTests.cs` — scenario 23

One `[Fact]` per scenario; test names match the SCENARIOS titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd snake-game/csharp
dotnet test
```
