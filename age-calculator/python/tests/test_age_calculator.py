from datetime import date

import pytest

from age_calculator import calculate


def test_Zenith_born_2016_10_28_on_2022_11_05_is_six():
    assert calculate(date(2016, 10, 28), date(2022, 11, 5)) == 6


def test_Zenith_on_her_seventh_birthday_is_seven():
    assert calculate(date(2016, 10, 28), date(2023, 10, 28)) == 7


def test_Zenith_on_the_day_before_her_seventh_birthday_is_six():
    assert calculate(date(2016, 10, 28), date(2023, 10, 27)) == 6


def test_born_2000_01_01_on_2024_12_31_is_twenty_four():
    assert calculate(date(2000, 1, 1), date(2024, 12, 31)) == 24


def test_born_today_is_zero():
    assert calculate(date(2000, 1, 1), date(2000, 1, 1)) == 0


def test_leap_day_baby_on_February_28_in_a_non_leap_year_has_not_yet_aged_up():
    assert calculate(date(2000, 2, 29), date(2001, 2, 28)) == 0


def test_leap_day_baby_ages_up_on_March_1_in_a_non_leap_year():
    assert calculate(date(2000, 2, 29), date(2001, 3, 1)) == 1


def test_leap_day_baby_on_an_actual_leap_day_ages_up_exactly():
    assert calculate(date(2000, 2, 29), date(2004, 2, 29)) == 4


def test_born_yesterday_across_a_year_boundary_is_still_zero():
    assert calculate(date(1999, 12, 31), date(2000, 1, 1)) == 0


def test_born_1990_06_15_on_2024_06_14_is_thirty_three():
    assert calculate(date(1990, 6, 15), date(2024, 6, 14)) == 33


def test_raises_when_birthdate_is_after_today():
    with pytest.raises(ValueError, match="^birthdate is after today$"):
        calculate(date(2024, 6, 15), date(2024, 6, 14))
