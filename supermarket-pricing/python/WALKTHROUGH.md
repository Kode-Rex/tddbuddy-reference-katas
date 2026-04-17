# Supermarket Pricing — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — five pricing strategies behind a `PricingRule` protocol, `Money` as integer cents, `Weight` as a validated `Decimal` in kilograms, and `ComboDeal` as a checkout-level cross-product rule. This document covers what's idiomatic to Python.

## What's the Same

- `Money` stores integer cents, same as C# and TS.
- `PricingRule` is a `Protocol` with four concrete implementations.
- `Checkout` accumulates scans in `dict[str, int]` for quantities and `dict[str, Weight]` for weights.
- `ProductBuilder` and `CheckoutBuilder` mirror the C#/TS fluent patterns.

## What's Different

- **`Weight.kg` is `Decimal`, not `float`.** Python's `Decimal` gives exact decimal arithmetic for `0.5 * 199 = 99.5`, which is then rounded to 100 via `Decimal.quantize(…, ROUND_HALF_UP)`. This avoids the float-precision issue that `0.5 * 199` can surface in some languages.
- **`@dataclass(frozen=True)` for value types.** `Money`, `Weight`, `ComboDeal`, and all pricing rules are frozen dataclasses — immutable and equality-comparable by value out of the box.
- **`Protocol` instead of ABC.** `PricingRule` is a structural type (duck typing done right). Concrete classes don't need to inherit from anything — they just implement `calculate(quantity, weight) -> Money`.
- **`defaultdict` for quantity tracking.** Pythonic alternative to `GetValueOrDefault` — the `Checkout` uses `defaultdict(int)` so `self._quantities[sku] += 1` works without a key check.
- **Test weights as string literals.** `Weight("0.5")` produces exact `Decimal("0.5")`, whereas `Weight(0.5)` would go through float conversion. Tests use string form for precision.

## Scenario Map

All twenty scenarios live in `tests/test_checkout.py`, grouped by comment sections that mirror the spec's headings.

## How to Run

```bash
cd supermarket-pricing/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
