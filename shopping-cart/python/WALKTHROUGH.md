# Shopping Cart — Python Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Money` Uses `Decimal`

Python's `float` is binary and will betray monetary arithmetic at surprising places (`0.1 + 0.2 != 0.3`). `decimal.Decimal` is exact. The `Money(amount)` constructor accepts `int | float | str | Decimal` and coerces through `str()` to avoid the float-in-Decimal precision trap — `Money(1.25)` goes through `Decimal("1.25")`, not `Decimal(1.25)`.

Equality is automatic — `@dataclass(frozen=True)` generates value-based equality, so `Money("5.00") == Money(5)`.

### `DiscountPolicy` as a `Protocol`

Python has no interfaces. `Protocol` from `typing` is the closest equivalent — structural typing that static checkers recognize. Each concrete policy (`PercentOff`, `FixedOff`, `BuyXGetY`, `BulkPricing`, `NoDiscount`) just implements `apply(unit_price, quantity) -> Money`; none inherits from `DiscountPolicy`. Type checkers accept them; runtime doesn't care.

This is slightly more permissive than the C# `interface` and the TS `interface + implements`. In exchange, the author avoids an ABC's `@abstractmethod` boilerplate and the policies read as plain dataclasses.

### Frozen Dataclasses for Policies

Each policy is `@dataclass(frozen=True)` — immutable, hashable, printable for free. `__post_init__` validates the construction arguments (percent in `[0, 100]`, non-negative amounts). Validation lives with the type, not at the call site.

### `Quantity` Is a Frozen Dataclass

Same motivation: value-based equality, immutability, and `__post_init__` enforcement that rejects zero and negative values. The three "updating quantity is rejected" scenarios therefore raise `ValueError` from the value type's constructor.

### `Cart.update_quantity` Raises `LineItemNotFoundError` for Missing SKUs

`ValueError` is still the idiomatic choice when an argument's shape is wrong (zero/negative quantity). But "no line for this SKU" is a domain rejection, not a value-shape problem — so it raises a domain-named `LineItemNotFoundError` that tests can catch meaningfully and that shows up in the stack trace. The spec doesn't exercise the missing-SKU path directly but the guard documents intent and costs nothing.

### Named Constants

`MIN_PERCENT`, `MAX_PERCENT`, `HUNDRED_PERCENT` in `discount_policy.py`. Same rule as C# and TS: no magic numbers in the domain.

### Package Layout

`src/shopping_cart/` with an `__init__.py` re-exporting the public surface (`Cart`, `Product`, `LineItem`, `Money`, `Quantity`, all four discount policies, and the `DiscountPolicy` protocol). Tests import from `shopping_cart`, not from submodules — the package boundary is the API the domain presents.

## Scenario Map

Eighteen scenarios live in `tests/test_cart.py`, grouped by comment-banner sections that mirror `SCENARIOS.md`. Test names are the scenario titles converted to `snake_case`.

## How to Run

```bash
cd shopping-cart/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
