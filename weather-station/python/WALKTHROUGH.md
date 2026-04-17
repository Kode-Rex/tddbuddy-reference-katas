# Weather Station — Python Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Reading` Uses `Decimal`

Python's `float` is binary and will betray arithmetic at surprising places (`0.1 + 0.2 != 0.3`). `decimal.Decimal` is exact. The `Reading` constructor accepts `int | float | str | Decimal` and coerces through `str()` to avoid the float-in-Decimal precision trap.

Equality is automatic — `@dataclass(frozen=True)` generates value-based equality, so two readings with identical fields compare equal.

### `Clock` as a `Protocol`

Python has no interfaces. `Protocol` from `typing` is the closest equivalent — structural typing with static-checker recognition. `FixedClock` doesn't inherit from `Clock`; it just implements `now(): datetime`. Type checkers accept it; runtime doesn't care.

### `AlertThresholds` with Coerced Decimals

The `AlertThresholds` dataclass accepts `int | float | str | Decimal | None` for each threshold and coerces non-None values to `Decimal` through `str()`. This keeps the test-facing API clean (`AlertThresholds(high_temperature_ceiling=35)`) while ensuring internal comparisons use exact arithmetic.

### Domain Exceptions

`InvalidReadingError` and `NoReadingsError` are plain `Exception` subclasses. Python's exception hierarchy is flat by convention — you don't need a base class hierarchy for two domain errors. The error messages are **byte-identical across C#, TypeScript, and Python**.

### Package Layout

`src/weather_station/` with an `__init__.py` re-exporting the public surface. Tests import from `weather_station`, not from submodules — the package boundary is the API the domain presents.

## Scenario Map

Twenty-four scenarios live in `tests/test_station.py`, one function per scenario.

## How to Run

```bash
cd weather-station/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
