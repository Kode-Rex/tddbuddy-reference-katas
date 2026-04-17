# Jelly vs Tower — Python Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `ColorType` as an `Enum`

Python's `enum.Enum` gives typed, named constants with automatic equality. `ColorType.Blue == ColorType.Blue` works without implementing `__eq__`. The damage table uses `(ColorType, int, ColorType)` tuples as dictionary keys — hashable and readable.

### `RandomSource` as a `Protocol`

Python has no interfaces. `Protocol` from `typing` is the closest equivalent — structural typing with static-checker recognition. `FixedRandomSource` doesn't inherit from `RandomSource`; it just implements `next(min_inclusive, max_inclusive)`. Type checkers accept it; runtime doesn't care.

### Domain Exceptions

`InvalidHealthException` and `InvalidLevelException` extend `Exception`. The message strings are byte-identical to the C# and TypeScript implementations. Tests use `pytest.raises` with a `match` parameter to verify both the type and the message.

### `DamageRange` as a Frozen Dataclass

`@dataclass(frozen=True)` makes `DamageRange` immutable and hashable. The damage table is a module-level dictionary — initialized once, never mutated.

### Properties for Encapsulation

`Jelly` and `Tower` use `@property` for read-only access to internal state. `Jelly._health` is private because it mutates during combat; the property exposes the current value without allowing direct assignment.

### Package Layout

`src/jelly_vs_tower/` with an `__init__.py` re-exporting the public surface. Tests import from `jelly_vs_tower`, not from submodules.

## Scenario Map

Twenty-five scenarios live in `tests/test_jelly_vs_tower.py`, one function per scenario.

## How to Run

```bash
cd jelly-vs-tower/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
