# Robot Factory — Python Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### Enums for `PartType` and `PartOption`

Python uses `Enum` classes with string values (`PartType.HEAD = "Head"`). The `.value` attribute gives cross-language-consistent strings for error messages: `f"Part not available: {option.value}"` produces the same byte-for-byte message as C# and TypeScript.

### `Money` Uses `Decimal`

Same rationale as the bank-account kata: `decimal.Decimal` avoids floating-point surprises. The `Money` dataclass is frozen for value-type semantics, with `__lt__` and `__gt__` for price comparison in `min()`.

### `PartSupplier` as a `Protocol`

Python has no interfaces. `Protocol` from `typing` provides structural typing. `FakePartSupplier` doesn't inherit from `PartSupplier`; it just implements `name`, `get_quote`, and `purchase`. Type checkers accept it; runtime duck-typing handles the rest.

### Frozen Dataclasses for Value Types

`PartQuote`, `PurchasedPart`, `CostBreakdown`, and `Money` are all `@dataclass(frozen=True)`. This gives automatic `__eq__` for assertions (`assert quote == PartQuote(...)`) and prevents accidental mutation.

### Builder Style

Python builders use explicit `self` and return `RobotOrderBuilder` rather than `Self` (compatible with 3.11 without importing `Self` from `typing_extensions`). The pattern is identical to the C# and TypeScript builders — fluent chains with sensible defaults.

## Scenario Map

Twenty scenarios live in `tests/test_robot_factory.py`, one function per scenario.

## How to Run

```bash
cd robot-factory/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
