# Timesheet Calc — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md). This file is a **delta** — it covers what is idiomatic to Python, not the full rationale.

## Python-Specific Idioms

### `Enum` for `Day`

Python's `enum.Enum` gives us the domain vocabulary with value identity; `Day.MONDAY` is a first-class citizen, not a string. String values (`"Monday"`) are chosen so `repr` and failure diagnostics are readable.

### `is_weekend` as a module-level function, not a method

Python convention prefers module-level helpers over method pollution on small enums. `is_weekend(Day.SATURDAY)` reads fine; adding `Day.is_weekend(self)` would require a class-method / mixin pattern for minimal gain.

### `@dataclass(frozen=True)` for `TimesheetTotals` with a derived property

`TotalHours` is a `@property` that computes `regular_hours + overtime_hours` on demand. `frozen=True` gives value equality and hashability for free — equivalent to the C# record. `total_hours` is not a dataclass field, so it is never stored; the invariant is enforced by construction.

### `ValueError` with identical message string

Python's natural rejection for a bad argument value is `ValueError`. The message string `"hours must not be negative"` is byte-identical to the C# `ArgumentException` and the TS `Error`, codified in `ERROR_HOURS_MUST_NOT_BE_NEGATIVE`. Cross-language message-string parity is part of the spec.

### `Mapping[Day, float]` on the constructor, copied into a `dict`

The `Timesheet` constructor accepts any `Mapping` (most commonly a `dict` from the builder) and copies it into an internal `dict`. `dict` preserves insertion order since 3.7, so the iteration order of `.totals()` is deterministic and matches the order entries were added — same as C#'s `Dictionary` and TS's `Map`.

### `src/timesheet_calc/` layout with re-export via `__init__.py`

Mirrors the password-kata convention: `src/timesheet_calc/timesheet.py` holds the real code; `src/timesheet_calc/__init__.py` re-exports the public surface so tests can write `from timesheet_calc import Day, Timesheet` without reaching into the inner module.

### The builder is ~15 lines — inside the F2 Python budget

The F2 style note budgets ~40 lines for Python builders because `self` and annotations tax Python vertical lines more than C#/TS. This one is small because the entity is simple — one primary setter (`with_entry`), one build step.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_timesheet.py`, one `def test_...` per scenario. Test names use `snake_case` matching the scenario titles.

## How to Run

```bash
cd timesheet-calc/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
