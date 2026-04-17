# Zombie Survivor — Python Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Clock` as a `Protocol`

Python has no interfaces. `Protocol` from `typing` is the closest equivalent — structural typing with static-checker recognition. `FixedClock` doesn't inherit from `Clock`; it just implements `now() -> datetime`. Type checkers accept it; runtime doesn't care.

### `Equipment` and `HistoryEntry` as Frozen Dataclasses

`@dataclass(frozen=True)` gives value-based equality and immutability. An `Equipment("Bat", EquipmentSlot.IN_HAND)` is a value — two instances with the same fields are equal. This mirrors C#'s `record` and TypeScript's `readonly` interface.

### `Level` and `Skill` as String Enums

Python's `Enum` with string values (`Level.YELLOW.value == "Yellow"`) gives both type safety and human-readable serialization for history messages. The `level_for` function maps XP to level, and `max_level` handles comparison since Python enums have no inherent ordering.

### Exception Hierarchy

`EquipmentCapacityExceededException` and `DuplicateSurvivorNameException` extend `Exception` directly. The message strings are byte-identical to C# and TypeScript. Tests use `pytest.raises(ExType, match="...")` to assert both type and message.

### Package Layout

`src/zombie_survivor/` with an `__init__.py` re-exporting the public surface. Tests import from `zombie_survivor`, not from submodules — the package boundary is the API the domain presents.

### Builder Self-Types

Python builders return `"SurvivorBuilder"` (forward-reference string) rather than `Self` for compatibility with Python 3.11. The pattern is identical to the C# and TypeScript builders.

## Scenario Map

Thirty-two scenarios live across six test files, one function per scenario. Names follow pytest's `snake_case` convention.

## How to Run

```bash
cd zombie-survivor/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
