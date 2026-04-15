from datetime import datetime, timedelta

from time_zone_converter import convert


UTC = timedelta(0)


def test_identity_conversion_between_UTC_and_UTC_returns_the_same_local_time():
    assert convert(datetime(2024, 6, 15, 12, 0), UTC, UTC) == datetime(2024, 6, 15, 12, 0)


def test_westward_conversion_from_UTC_to_minus_five_subtracts_five_hours():
    assert convert(datetime(2024, 6, 15, 12, 0), UTC, timedelta(hours=-5)) == datetime(2024, 6, 15, 7, 0)


def test_eastward_conversion_from_UTC_to_plus_nine_adds_nine_hours():
    assert convert(datetime(2024, 6, 15, 12, 0), UTC, timedelta(hours=9)) == datetime(2024, 6, 15, 21, 0)


def test_cross_zone_conversion_without_UTC_transits_via_the_shared_instant():
    assert convert(datetime(2024, 6, 15, 9, 0), timedelta(hours=-5), timedelta(hours=9)) == datetime(2024, 6, 15, 23, 0)


def test_forward_conversion_across_midnight_rolls_into_the_next_day():
    assert convert(datetime(2024, 6, 15, 22, 0), UTC, timedelta(hours=5)) == datetime(2024, 6, 16, 3, 0)


def test_backward_conversion_across_midnight_rolls_into_the_previous_day():
    assert convert(datetime(2024, 6, 15, 2, 0), UTC, timedelta(hours=-5)) == datetime(2024, 6, 14, 21, 0)


def test_half_hour_offset_plus_five_thirty_handles_non_integer_hour_zones():
    assert convert(datetime(2024, 6, 15, 12, 0), UTC, timedelta(hours=5, minutes=30)) == datetime(2024, 6, 15, 17, 30)


def test_quarter_hour_offset_plus_five_forty_five_handles_forty_five_minute_zones():
    assert convert(datetime(2024, 6, 15, 12, 0), UTC, timedelta(hours=5, minutes=45)) == datetime(2024, 6, 15, 17, 45)


def test_forward_conversion_across_month_boundary_rolls_June_into_July():
    assert convert(datetime(2024, 6, 30, 23, 30), UTC, timedelta(hours=2)) == datetime(2024, 7, 1, 1, 30)


def test_backward_conversion_across_year_boundary_rolls_into_the_previous_year():
    assert convert(datetime(2024, 1, 1, 1, 0), UTC, timedelta(hours=-5)) == datetime(2023, 12, 31, 20, 0)


def test_forward_conversion_across_leap_day_rolls_February_29th_into_March_1st():
    assert convert(datetime(2024, 2, 29, 23, 0), UTC, timedelta(hours=2)) == datetime(2024, 3, 1, 1, 0)


def test_international_date_line_swing_from_plus_twelve_to_minus_twelve_steps_back_one_day():
    assert convert(datetime(2024, 6, 15, 10, 0), timedelta(hours=12), timedelta(hours=-12)) == datetime(2024, 6, 14, 10, 0)
