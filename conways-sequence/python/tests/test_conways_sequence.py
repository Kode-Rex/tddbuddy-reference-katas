import pytest

from conways_sequence import next_term, look_and_say


def test_next_term_of_one_is_one_one():
    assert next_term("1") == "11"


def test_next_term_of_one_one_is_two_one():
    assert next_term("11") == "21"


def test_next_term_of_two_one_is_one_two_one_one():
    assert next_term("21") == "1211"


def test_next_term_of_one_two_one_one_is_one_one_one_two_two_one():
    assert next_term("1211") == "111221"


def test_next_term_of_one_one_one_two_two_one_is_three_one_two_two_one_one():
    assert next_term("111221") == "312211"


def test_next_term_of_a_single_two_is_one_two():
    assert next_term("2") == "12"


def test_next_term_of_two_two_is_a_fixed_point():
    assert next_term("22") == "22"


def test_next_term_of_three_two_one_one_is_one_three_one_two_two_one():
    assert next_term("3211") == "131221"


def test_next_term_of_ten_consecutive_ones_describes_ten_ones():
    assert next_term("1111111111") == "101"


def test_look_and_say_with_zero_iterations_returns_the_seed_unchanged():
    assert look_and_say("1", 0) == "1"


def test_look_and_say_with_one_iteration_equals_a_single_next_term():
    assert look_and_say("1", 1) == "11"


def test_look_and_say_with_five_iterations_from_one_lands_on_three_one_two_two_one_one():
    assert look_and_say("1", 5) == "312211"


def test_look_and_say_with_two_iterations_from_seed_two_is_one_one_one_two():
    assert look_and_say("2", 2) == "1112"


def test_look_and_say_with_negative_iterations_is_rejected():
    with pytest.raises(ValueError, match="iterations must be non-negative"):
        look_and_say("1", -1)
