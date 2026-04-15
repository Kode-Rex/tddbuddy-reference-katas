# Kata Potter — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Pure Domain Only

No catalogue, no receipts, no configurable series. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## The Python Shape

- **`Decimal` for money.** The base price and the discount table are `Decimal` constants; `price()` returns `Decimal`. Python's `float` would introduce the same drift TypeScript has — but Python has `decimal` in the standard library, so we use it. Tests compare with `==` rather than a tolerance, matching the C# exactness.
- **`BookOutOfRangeError` subclasses `ValueError`.** Python idiom: range-violation errors inherit from `ValueError`. Tests still catch the specific subclass; the message string stays byte-identical across all three languages.
- **Method names use `snake_case`**: `price`, `with_book`, `price_of_set`. Constants are `UPPER_SNAKE`: `BASE_PRICE`, `MIN_BOOK_ID`, `MAX_BOOK_ID`, `SET_DISCOUNT`. Rule-name *strings* (the exception message) stay byte-identical across C#, TS, and Python.
- **`Basket.__init__` is the test-folder hook.** Python has no `InternalsVisibleTo`; the docstring signals "production code should use the builder".
- **The greedy-then-adjust algorithm is identical to C# and TS.** Same histogram, same single (5,3) → (4,4) swap pass, same correctness argument.

## Why `BasketBuilder` Lives in `tests/`

Same F2 rationale as C#: twelve scenarios need specific basket shapes, and the chained `with_book(series, count)` reads as the literal basket. The builder is 20 lines — comfortably inside the 30–40 line Python F2 budget.

`tests/__init__.py` exists so that tests can `from tests.basket_builder import BasketBuilder`.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_basket.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd kata-potter/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
