# Mars Rover — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Pure Domain Only

No renderer, no CLI, no multi-rover coordination. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Python Shape

- **`Direction`, `Command`, `MovementStatus` are `Enum` subclasses** with string values. Identity comparisons (`heading is Direction.NORTH`) read well in tests, pretty-print matches the byte-identical string values used in C# and TS, and mypy can check exhaustiveness.
- **`Rover` is a plain class with keyword-default constructor args.** `@dataclass(frozen=True)` is tempting for a "record" feel, but a frozen dataclass with a `set` field does not compose cleanly (the set is still mutable) and the class's `__repr__` showing the whole obstacle set on every assertion failure drowns the diagnostic.
- **Obstacles stored as a `set[tuple[int, int]]`.** Python tuples are hashable, so — unlike JavaScript — we can store coordinate pairs directly without encoding. `(1, 0) in obstacles` works.
- **Rotation uses module-level dict lookup tables** (`_LEFT_OF`, `_RIGHT_OF`, `_STEP_OF`) rather than a chain of `if` statements. The dicts read as domain tables ("left of North is West") and a reader verifies correctness by eye.
- **Modular arithmetic leans on Python's `%`**, which already returns a non-negative result for a positive modulus (`-1 % 100 == 99`). We don't need the `((x % m) + m) % m` dance that C# and TS use.
- **`UnknownCommandError` subclasses `ValueError`** because the offender is a user-provided string, not a type error. Message is byte-identical to C#/TS: `"unknown command"`.
- **Field and method names use `snake_case`**: `position`, `last_obstacle`, `with_obstacle_at`, `on_grid`. Rule-name *strings* stay identical across languages even though *method* names differ by convention — the spec is the exception message, not the method name.

## Why Two Builders Live in `tests/`

Same F2 rationale as C#: fifteen scenarios need an assortment of rover starting states and command strings. `RoverBuilder` (`.at().facing().on_grid().with_obstacle_at().build()`) assembles the entity; `CommandBuilder` emits `"FFLRB"`-style strings when a test reads better as narrative than as a cryptic literal. Each builder sits near the Python F2 40-line target — Python's explicit `self` and type annotations push idiomatic builders up the range.

`tests/__init__.py` exists so that tests can `from tests.rover_builder import RoverBuilder` — mirroring the C# `namespace MarsRover.Tests` and TS relative import.

## Scenario Map

The fifteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_rover.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd mars-rover/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
