from string_calculator.calculator import add


def test_empty_string_returns_zero():
    assert add("") == 0


def test_single_number_returns_itself():
    assert add("1") == 1


def test_two_numbers_return_their_sum():
    assert add("1,2") == 3


def test_many_numbers_returns_their_sum():
    assert add("1,2,3,4") == 10
