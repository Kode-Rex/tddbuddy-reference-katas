# Bingo — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Pure Domain Only

No generator, no caller, no game loop. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Python Shape

- **`WinPattern` is a frozen dataclass with a `kind` string and an optional `index`.** Python has no sealed-union or discriminated-union primitive; a frozen dataclass gives structural equality (so `card.winning_pattern() == WinPatterns.row(0)` works) and immutability. The `WinPatterns` class holds the canonical singletons (`NONE`, `DIAGONAL_MAIN`, `DIAGONAL_ANTI`) and the factory functions (`row(i)`, `column(i)`). This is the Python answer to the C# record-struct and the TS discriminated-union.
- **`NumberOutOfRangeError` subclasses `ValueError`.** Python idiom: range-violation errors inherit from `ValueError`; tests still catch the specific subclass.
- **Number cells use `Optional[int]` rather than a `Cell` class.** `None` at `(2, 2)` *is* the free space. Two parallel grids (`self._numbers: List[List[Optional[int]]]`, `self._marks: List[List[bool]]`) keep the three hot-path operations one-step.
- **`Card.__init__` is the test-folder hook.** Python has no `InternalsVisibleTo` or `@internal` JSDoc equivalent; the constructor takes prepared grids and the docstring signals "production code should use a generator" (which lives in F3-shaped stretch work).
- **Method names use `snake_case`**: `mark`, `is_marked`, `number_at`, `winning_pattern`, `has_won`, `with_number_at`, `with_mark_at`. Rule-name *strings* (the exception message) stay byte-identical across C#, TS, and Python.
- **`WinPatterns.NONE` compared with `==` rather than `is`** because `WinPattern` is a frozen dataclass, not an Enum. Dataclass equality is structural; `is` would test object identity and would break after round-trips.

## Why `CardBuilder` Lives in `tests/`

Same F2 rationale as C#: a dozen scenarios need specific card layouts, and replaying a call sequence hides the card state under test. With `with_number_at` + `with_mark_at`, setup is a fluent chain that reads as the literal card. The builder is 23 lines — comfortably inside the 30–40 line Python F2 budget.

`tests/__init__.py` exists so that tests can `from tests.card_builder import CardBuilder` — mirroring the C# `namespace Bingo.Tests` and the TS relative import.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_card.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd bingo/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
