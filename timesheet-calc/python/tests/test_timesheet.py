import pytest

from timesheet_calc import Day, STANDARD_WORK_WEEK_HOURS
from tests.timesheet_builder import TimesheetBuilder


def test_an_empty_timesheet_totals_zero_across_the_board():
    totals = TimesheetBuilder().build().totals()
    assert totals.regular_hours == 0
    assert totals.overtime_hours == 0
    assert totals.total_hours == 0


def test_a_single_8_hour_weekday_is_all_regular():
    totals = TimesheetBuilder().with_entry(Day.MONDAY, 8).build().totals()
    assert totals.regular_hours == 8
    assert totals.overtime_hours == 0
    assert totals.total_hours == 8


def test_a_single_weekday_under_8_hours_is_all_regular():
    totals = TimesheetBuilder().with_entry(Day.TUESDAY, 6).build().totals()
    assert totals.regular_hours == 6
    assert totals.overtime_hours == 0
    assert totals.total_hours == 6


def test_weekday_hours_beyond_8_spill_into_overtime():
    totals = TimesheetBuilder().with_entry(Day.MONDAY, 10).build().totals()
    assert totals.regular_hours == 8
    assert totals.overtime_hours == 2
    assert totals.total_hours == 10


def test_fractional_weekday_overtime_is_tracked():
    totals = TimesheetBuilder().with_entry(Day.MONDAY, 8.5).build().totals()
    assert totals.regular_hours == 8
    assert totals.overtime_hours == pytest.approx(0.5)
    assert totals.total_hours == pytest.approx(8.5)


def test_saturday_hours_are_all_overtime():
    totals = TimesheetBuilder().with_entry(Day.SATURDAY, 4).build().totals()
    assert totals.regular_hours == 0
    assert totals.overtime_hours == 4
    assert totals.total_hours == 4


def test_sunday_hours_are_all_overtime():
    totals = TimesheetBuilder().with_entry(Day.SUNDAY, 6).build().totals()
    assert totals.regular_hours == 0
    assert totals.overtime_hours == 6
    assert totals.total_hours == 6


def test_a_full_monday_to_friday_at_8_hours_each_totals_the_standard_40_hour_week():
    totals = (
        TimesheetBuilder()
        .with_entry(Day.MONDAY, 8)
        .with_entry(Day.TUESDAY, 8)
        .with_entry(Day.WEDNESDAY, 8)
        .with_entry(Day.THURSDAY, 8)
        .with_entry(Day.FRIDAY, 8)
        .build()
        .totals()
    )
    assert totals.regular_hours == STANDARD_WORK_WEEK_HOURS
    assert totals.overtime_hours == 0
    assert totals.total_hours == STANDARD_WORK_WEEK_HOURS


def test_a_mixed_week_combines_weekday_overtime_with_weekend_overtime():
    totals = (
        TimesheetBuilder()
        .with_entry(Day.MONDAY, 9)
        .with_entry(Day.TUESDAY, 8)
        .with_entry(Day.WEDNESDAY, 8)
        .with_entry(Day.THURSDAY, 8)
        .with_entry(Day.FRIDAY, 10)
        .with_entry(Day.SATURDAY, 5)
        .build()
        .totals()
    )
    assert totals.regular_hours == 40
    assert totals.overtime_hours == 8
    assert totals.total_hours == 48


def test_later_entries_for_the_same_day_replace_earlier_entries():
    totals = (
        TimesheetBuilder()
        .with_entry(Day.MONDAY, 8)
        .with_entry(Day.MONDAY, 10)
        .build()
        .totals()
    )
    assert totals.regular_hours == 8
    assert totals.overtime_hours == 2
    assert totals.total_hours == 10


def test_a_negative_hours_entry_is_rejected_with_the_identical_cross_language_message():
    with pytest.raises(ValueError, match="^hours must not be negative$"):
        TimesheetBuilder().with_entry(Day.MONDAY, -1).build()


def test_a_zero_hour_entry_is_valid_and_contributes_nothing():
    totals = TimesheetBuilder().with_entry(Day.MONDAY, 0).build().totals()
    assert totals.regular_hours == 0
    assert totals.overtime_hours == 0
    assert totals.total_hours == 0
