# Event Sourcing — Python Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### Events as Frozen Dataclasses with Inheritance

Python's `@dataclass(frozen=True)` gives immutable value-based equality for free. The event hierarchy uses class inheritance: `AccountOpened`, `MoneyDeposited`, `MoneyWithdrawn`, and `AccountClosed` all extend `AccountEvent`. Type narrowing uses `isinstance()` — Python's equivalent of C#'s pattern matching.

### `Money` Uses `Decimal`

Same as the bank-account kata: `float` is binary and betrays monetary arithmetic. `Decimal` via `str()` coercion avoids the float-in-Decimal precision trap. `@dataclass(frozen=True)` gives automatic value equality.

### Exception Classes

Each domain exception (`AccountNotOpenException`, `AccountClosedException`, etc.) extends `Exception` directly. Python has no `InvalidOperationException` equivalent worth subclassing. The message strings are byte-identical to C# and TypeScript.

### `AccountStatus` as an Enum

Python's `enum.Enum` with string values (`"Open"`, `"Closed"`) provides type safety and readable string representation.

### `AccountBuilder` Generates Timestamps

The builder auto-increments timestamps from the open time by adding `timedelta(hours=N)`. Tests that need specific timestamps pass them explicitly; tests that don't care about time get well-ordered events for free.

### Package Layout

`src/event_sourcing/` with `__init__.py` re-exporting the full public surface. Tests import from `event_sourcing`, not from submodules. The `event_builder.py` and `account_builder.py` helpers live in the `tests/` package.

## Scenario Map

Twenty-four scenarios live in `tests/test_account.py`, one function per scenario.

## How to Run

```bash
cd event-sourcing/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
