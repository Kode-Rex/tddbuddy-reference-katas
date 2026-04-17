# Expense Report — Python Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Money` Uses `Decimal`

Same as the Bank Account kata. Python's `float` will betray monetary arithmetic. `decimal.Decimal` is exact. The `Money(amount)` constructor accepts `int | float | str | Decimal` and coerces through `str()` to avoid the float-in-Decimal precision trap.

`@dataclass(frozen=True)` gives value-based equality, so `Money(50) == Money(50)` across constructions.

### `Category` and `ReportStatus` as String Enums

Python `Enum` with string values (`Category.MEALS = "Meals"`) mirrors the TypeScript pattern. The `.value` property provides the display string for the summary output without a lookup table.

### Domain Exceptions Extend `Exception`

C# has named exception classes; Python mirrors this. `EmptyReportError`, `FinalizedReportError`, etc. The message strings are byte-identical across all three languages.

### `ExpenseItem` as a Frozen Dataclass

Expenses are immutable value objects. `@dataclass(frozen=True)` makes them hash-safe and equality-comparable, which is needed for `remove_expense` to find items in the list.

### Builder Pattern with Lambdas

`ReportBuilder.with_expense_from(lambda b: b.as_meal(60))` mirrors the C# `Action<ExpenseItemBuilder>` and TypeScript callback patterns. Python lambdas are limited to one expression but that's sufficient for configuring a builder.

### Package Layout

`src/expense_report/` with an `__init__.py` re-exporting the public surface. Tests import from `expense_report`, not from submodules.

## Scenario Map

Twenty-two scenarios live in `tests/test_report.py`, one function per scenario. The per-item limits scenario uses `@pytest.mark.parametrize` with five rows.

## How to Run

```bash
cd expense-report/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
