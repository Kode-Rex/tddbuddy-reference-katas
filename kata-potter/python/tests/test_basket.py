from decimal import Decimal

import pytest

from kata_potter import BookOutOfRangeError

from tests.basket_builder import BasketBuilder


def _eur(value: str) -> Decimal:
    return Decimal(value)


def test_empty_basket_costs_zero():
    basket = BasketBuilder().build()
    assert basket.price() == _eur("0.00")


def test_one_book_costs_the_base_price():
    basket = BasketBuilder().with_book(1, 1).build()
    assert basket.price() == _eur("8.00")


def test_two_copies_of_the_same_book_get_no_discount():
    basket = BasketBuilder().with_book(1, 2).build()
    assert basket.price() == _eur("16.00")


def test_two_distinct_books_get_the_five_percent_discount():
    basket = BasketBuilder().with_book(1, 1).with_book(2, 1).build()
    assert basket.price() == _eur("15.20")


def test_three_distinct_books_get_the_ten_percent_discount():
    basket = (
        BasketBuilder()
        .with_book(1, 1)
        .with_book(2, 1)
        .with_book(3, 1)
        .build()
    )
    assert basket.price() == _eur("21.60")


def test_four_distinct_books_get_the_twenty_percent_discount():
    basket = (
        BasketBuilder()
        .with_book(1, 1)
        .with_book(2, 1)
        .with_book(3, 1)
        .with_book(4, 1)
        .build()
    )
    assert basket.price() == _eur("25.60")


def test_five_distinct_books_get_the_twenty_five_percent_discount():
    basket = (
        BasketBuilder()
        .with_book(1, 1)
        .with_book(2, 1)
        .with_book(3, 1)
        .with_book(4, 1)
        .with_book(5, 1)
        .build()
    )
    assert basket.price() == _eur("30.00")


def test_duplicates_are_priced_separately_from_the_discounted_set():
    basket = BasketBuilder().with_book(1, 2).with_book(2, 1).build()
    # one 2-set (€15.20) + one 1-set (€8.00)
    assert basket.price() == _eur("23.20")


def test_two_copies_of_every_book_makes_two_five_sets():
    basket = (
        BasketBuilder()
        .with_book(1, 2)
        .with_book(2, 2)
        .with_book(3, 2)
        .with_book(4, 2)
        .with_book(5, 2)
        .build()
    )
    assert basket.price() == _eur("60.00")


def test_greedy_fails_basket_prefers_two_four_sets_over_a_five_plus_three():
    # two each of books 1,2,3 plus one each of 4,5
    basket = (
        BasketBuilder()
        .with_book(1, 2)
        .with_book(2, 2)
        .with_book(3, 2)
        .with_book(4, 1)
        .with_book(5, 1)
        .build()
    )
    # Two 4-sets (€25.60 each) beats a 5-set (€30.00) + 3-set (€21.60).
    assert basket.price() == _eur("51.20")


def test_bigger_greedy_fails_basket():
    # three each of 1,2,3 plus two each of 4,5
    basket = (
        BasketBuilder()
        .with_book(1, 3)
        .with_book(2, 3)
        .with_book(3, 3)
        .with_book(4, 2)
        .with_book(5, 2)
        .build()
    )
    # One 5-set (€30.00) + two 4-sets (€25.60 each) = €81.20.
    # Greedy would price this as two 5-sets + one 3-set = €81.60.
    assert basket.price() == _eur("81.20")


def test_basket_builder_rejects_book_ids_outside_1_to_5():
    with pytest.raises(BookOutOfRangeError) as info:
        BasketBuilder().with_book(0, 1)
    assert str(info.value) == "book id must be between 1 and 5"

    with pytest.raises(BookOutOfRangeError) as info:
        BasketBuilder().with_book(6, 1)
    assert str(info.value) == "book id must be between 1 and 5"
