from __future__ import annotations

from .money import Money

BASE_PRICE = Money("2.50")
SECOND_PRICE = Money("2.25")
THIRD_PRICE = Money("1.75")


def calculate(new_rental_count: int, existing_rental_count: int) -> Money:
    """Tally tiered price for ``new_rental_count`` fresh rentals."""
    total = Money.zero()
    for i in range(new_rental_count):
        ordinal = existing_rental_count + i + 1
        total = total + _price_for_ordinal(ordinal)
    return total


def _price_for_ordinal(ordinal: int) -> Money:
    if ordinal == 1:
        return BASE_PRICE
    if ordinal == 2:
        return SECOND_PRICE
    return THIRD_PRICE
