import pytest

from string_calculator.calculator import add


def test_empty_string_returns_zero():
    assert add("") == 0


def test_single_number_returns_itself():
    assert add("1") == 1


def test_two_numbers_return_their_sum():
    assert add("1,2") == 3


def test_many_numbers_returns_their_sum():
    assert add("1,2,3,4") == 10


def test_newline_is_also_a_delimiter():
    assert add("1\n2,3") == 6


def test_custom_single_char_delimiter_is_declared_in_header():
    assert add("//;\n1;2") == 3


def test_negative_number_is_rejected_with_listing_message():
    with pytest.raises(ValueError, match="negatives not allowed: -1"):
        add("-1,2")


def test_multiple_negatives_are_all_listed_in_the_message():
    with pytest.raises(ValueError, match=r"negatives not allowed: -1, -2"):
        add("-1,-2,3")


def test_numbers_greater_than_1000_are_ignored():
    assert add("2,1001") == 2


def test_delimiter_may_be_any_length_in_bracketed_header():
    assert add("//[***]\n1***2***3") == 6
