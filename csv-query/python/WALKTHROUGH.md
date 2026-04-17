# CSV Query — Python Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `UnknownColumnError` Extends `Exception`

Python's convention for domain exceptions: extend `Exception`, not `RuntimeError` or `ValueError`. The message is byte-identical to C# and TypeScript: `"Unknown column: <name>"`. Tests use `pytest.raises(UnknownColumnError, match=...)` for both type and message assertion.

### `Row` Uses a Plain `dict`

Python dictionaries preserve insertion order (since 3.7), so `Row` wraps a `dict[str, str]`. The `get(column)` method raises `UnknownColumnError` instead of returning `None` or raising `KeyError` — the domain rejection is explicit. `project()` builds a new dict in the order the caller requested.

### `order_by` Uses a Tuple Sort Key

Python's `sort(key=...)` handles mixed numeric/string comparison via a tuple key: `(0, float_value)` for numbers, `(1, string_value)` for strings. Numbers always sort before strings (type bucket 0 < 1). This avoids the `TypeError` that arises from comparing `float` to `str` directly.

### `snake_case` Method Names

The Query API uses `order_by` and `count` rather than `orderBy` — Python convention. The scenario names in the test file match `SCENARIOS.md` with mechanical underscore conversion.

### Package Layout

`src/csv_query/` with `__init__.py` re-exporting the public surface. Tests import from `csv_query`, not from submodules.

## Scenario Map

Twenty-five scenarios live in `tests/test_csv_query.py`, one function per scenario.

## How to Run

```bash
cd csv-query/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
