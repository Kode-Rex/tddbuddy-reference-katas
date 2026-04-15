from __future__ import annotations

from decimal import Decimal
from typing import List


# Identical byte-for-byte across C#, TypeScript, and Python.
# The exception messages are the spec (see ../SCENARIOS.md).
class BasketMessages:
    BOOK_OUT_OF_RANGE = "book id must be between 1 and 5"


BASE_PRICE = Decimal("8.00")
MIN_BOOK_ID = 1
MAX_BOOK_ID = 5

# Discount multiplier by set size. Index 0 is unused; index 1..5 are the
# discount fractions (0 for a 1-set, 0.25 for a 5-set).
SET_DISCOUNT: tuple[Decimal, ...] = (
    Decimal("0.00"),  # index 0 — unused
    Decimal("0.00"),  # 1 book
    Decimal("0.05"),  # 2 books
    Decimal("0.10"),  # 3 books
    Decimal("0.20"),  # 4 books
    Decimal("0.25"),  # 5 books
)


def price_of_set(distinct_books: int) -> Decimal:
    discount = SET_DISCOUNT[distinct_books]
    return distinct_books * BASE_PRICE * (Decimal("1") - discount)


class BookOutOfRangeError(ValueError):
    def __init__(self) -> None:
        super().__init__(BasketMessages.BOOK_OUT_OF_RANGE)


class Basket:
    def __init__(self, counts: List[int]) -> None:
        """Test-folder constructor hook; production code should use the builder."""
        self._counts: List[int] = list(counts)

    def price(self) -> Decimal:
        sets = _group_into_sets(self._counts)
        _adjust_five_plus_three_into_two_fours(sets)
        total = Decimal("0.00")
        for k in range(1, MAX_BOOK_ID + 1):
            total += sets[k] * price_of_set(k)
        return total


# Greedy pass: repeatedly pull the largest possible set of distinct titles
# out of the remaining counts. Returns a histogram where `sets[k]` is the
# number of k-sized sets.
def _group_into_sets(counts: List[int]) -> List[int]:
    remaining = list(counts)
    sets = [0] * (MAX_BOOK_ID + 1)
    while True:
        distinct = sum(1 for i in range(MIN_BOOK_ID, MAX_BOOK_ID + 1) if remaining[i] > 0)
        if distinct == 0:
            break
        sets[distinct] += 1
        for i in range(MIN_BOOK_ID, MAX_BOOK_ID + 1):
            if remaining[i] > 0:
                remaining[i] -= 1
    return sets


# Adjustment pass: a 5-set plus a 3-set always costs more than two 4-sets
# (51.60 vs 51.20 per pairing). Swap (5,3) -> (4,4) as many times as both
# counts allow. This is the only local swap that improves on greedy for the
# standard five-title discount table.
def _adjust_five_plus_three_into_two_fours(sets: List[int]) -> None:
    swaps = min(sets[5], sets[3])
    sets[5] -= swaps
    sets[3] -= swaps
    sets[4] += 2 * swaps
