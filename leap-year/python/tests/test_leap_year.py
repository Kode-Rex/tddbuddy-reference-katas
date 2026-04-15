from leap_year import is_leap_year


def test_2023_is_not_a_leap_year_because_it_is_not_divisible_by_four():
    assert is_leap_year(2023) is False


def test_2024_is_a_leap_year_because_it_is_divisible_by_four_and_not_by_one_hundred():
    assert is_leap_year(2024) is True


def test_2020_is_a_leap_year_because_it_is_divisible_by_four_and_not_by_one_hundred():
    assert is_leap_year(2020) is True


def test_1900_is_not_a_leap_year_because_it_is_divisible_by_one_hundred_but_not_by_four_hundred():
    assert is_leap_year(1900) is False


def test_2100_is_not_a_leap_year_because_it_is_divisible_by_one_hundred_but_not_by_four_hundred():
    assert is_leap_year(2100) is False


def test_2000_is_a_leap_year_because_it_is_divisible_by_four_hundred():
    assert is_leap_year(2000) is True


def test_1600_is_a_leap_year_because_it_is_divisible_by_four_hundred():
    assert is_leap_year(1600) is True


def test_2001_is_not_a_leap_year_because_it_is_not_divisible_by_four():
    assert is_leap_year(2001) is False
