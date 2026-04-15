from datetime import date

from last_sunday import find


def test_last_Sunday_of_January_2013_is_the_twenty_seventh():
    assert find(2013, 1) == date(2013, 1, 27)


def test_last_Sunday_of_February_2013_is_the_twenty_fourth_in_a_non_leap_February():
    assert find(2013, 2) == date(2013, 2, 24)


def test_last_Sunday_of_March_2013_is_the_thirty_first_when_the_last_day_itself_is_Sunday():
    assert find(2013, 3) == date(2013, 3, 31)


def test_last_Sunday_of_April_2013_is_the_twenty_eighth_in_a_thirty_day_month():
    assert find(2013, 4) == date(2013, 4, 28)


def test_last_Sunday_of_June_2013_is_the_thirtieth_when_the_last_day_is_Sunday():
    assert find(2013, 6) == date(2013, 6, 30)


def test_last_Sunday_of_December_2013_is_the_twenty_ninth_at_the_year_end_boundary():
    assert find(2013, 12) == date(2013, 12, 29)


def test_last_Sunday_of_February_2020_is_the_twenty_third_in_a_leap_February_ending_Saturday():
    assert find(2020, 2) == date(2020, 2, 23)


def test_last_Sunday_of_February_2032_is_the_twenty_ninth_when_leap_day_itself_is_Sunday():
    assert find(2032, 2) == date(2032, 2, 29)


def test_last_Sunday_of_February_2100_is_the_twenty_eighth_in_a_century_non_leap_February():
    assert find(2100, 2) == date(2100, 2, 28)


def test_last_Sunday_of_December_2000_is_the_thirty_first_in_a_four_hundred_divisible_leap_year():
    assert find(2000, 12) == date(2000, 12, 31)
