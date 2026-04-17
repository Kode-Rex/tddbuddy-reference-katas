# Snake Game — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (bounded grid, mutable Game aggregate, immutable Snake movement, injectable FoodSpawner, wall/self collision detection).

This note captures only the Python deltas.

## Position as a Frozen Dataclass

`@dataclass(frozen=True)` gives value equality and immutability — the Python equivalent of C#'s `readonly record struct`. Two `Position(0, 0)` instances are equal by value.

## Direction as an Enum with Methods

Python's `Enum` supports instance methods directly, unlike C# where extension methods are needed. `Direction.is_opposite()` and `Direction.move()` live on the enum itself. The `_OPPOSITES` and `_DELTAS` lookup dictionaries are module-level, avoiding repeated dict construction.

## FoodSpawner as a Callable Type Alias

`FoodSpawner = Callable[[list[Position]], Position]` — the same approach as C#'s `Func<IReadOnlyList<Position>, Position>`. Tests pass a lambda; production code would pass a function wrapping `random.choice`.

## Game Constructor Uses Optional Parameters

Like TS, Python doesn't have `internal` visibility. The `Game` constructor uses optional keyword arguments with defaults (`snake=None`, `food=None`, `score=0`, `state=GameState.PLAYING`). `BoardBuilder` passes all arguments explicitly.

## Scenario Map

Twenty-three scenarios across eight test files:

- `tests/test_initial_state.py` — scenarios 1–4
- `tests/test_basic_movement.py` — scenarios 5–8
- `tests/test_direction_changes.py` — scenarios 9–13
- `tests/test_eating_food.py` — scenarios 14–16
- `tests/test_wall_collisions.py` — scenarios 17–20
- `tests/test_self_collision.py` — scenario 21
- `tests/test_game_over_behavior.py` — scenario 22
- `tests/test_winning.py` — scenario 23

One test method per scenario; test names use `snake_case` matching the SCENARIOS titles.

## How to Run

```bash
cd snake-game/python
python3 -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
