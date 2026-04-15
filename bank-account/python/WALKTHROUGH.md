# Bank Account — Python Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Money` Uses `Decimal`

Python's `float` is binary and will betray monetary arithmetic at surprising places (`0.1 + 0.2 != 0.3`). `decimal.Decimal` is exact. The `Money(amount)` constructor accepts `int | float | str | Decimal` and coerces through `str()` to avoid the float-in-Decimal precision trap.

Equality is automatic — `@dataclass(frozen=True)` generates value-based equality, so `Money(500) == Money(500)` even across constructions.

### `Clock` as a `Protocol`

Python has no interfaces. `Protocol` from `typing` is the closest equivalent — structural typing with static-checker recognition. `FixedClock` doesn't inherit from `Clock`; it just implements `today(): date`. Type checkers accept it; runtime doesn't care.

### `Transaction` as a Frozen Dataclass

Same motivation as TypeScript's `readonly`: no reason for a transaction to mutate after creation. `frozen=True` makes it a hash-safe value type with automatic equality.

### Tests Use `snake_case` Names

Pytest's convention. The scenario "Withdrawing zero is rejected" becomes `test_withdrawing_zero_is_rejected`. The mapping is mechanical and unambiguous.

### Package Layout

`src/bank_account/` with an `__init__.py` re-exporting the public surface (`Account`, `Money`, `Transaction`, `Clock`). Tests import from `bank_account`, not from submodules — the package boundary is the API the domain presents.

## Scenario Map

Twenty scenarios live in `tests/test_account.py`, one function per scenario.

## How to Run

```bash
cd bank-account/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
