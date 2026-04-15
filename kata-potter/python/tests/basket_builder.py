from __future__ import annotations

from kata_potter import Basket, BookOutOfRangeError, MAX_BOOK_ID, MIN_BOOK_ID


class BasketBuilder:
    """Test-folder fluent synthesiser. Place copies of books into the basket by id.

    Reads as the basket literal under test: two of book 1, one of book 2, etc.
    """

    def __init__(self) -> None:
        self._counts: list[int] = [0] * (MAX_BOOK_ID + 1)

    def with_book(self, series: int, count: int) -> "BasketBuilder":
        if series < MIN_BOOK_ID or series > MAX_BOOK_ID:
            raise BookOutOfRangeError()
        if count < 0:
            count = 0
        self._counts[series] += count
        return self

    def build(self) -> Basket:
        return Basket(self._counts)
