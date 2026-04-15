using FluentAssertions;
using Xunit;

namespace TimesheetCalc.Tests;

public class TimesheetTests
{
    [Fact]
    public void An_empty_timesheet_totals_zero_across_the_board()
    {
        var timesheet = new TimesheetBuilder().Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(0);
        totals.OvertimeHours.Should().Be(0);
        totals.TotalHours.Should().Be(0);
    }

    [Fact]
    public void A_single_8_hour_weekday_is_all_regular()
    {
        var timesheet = new TimesheetBuilder().WithEntry(Day.Monday, 8).Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(8);
        totals.OvertimeHours.Should().Be(0);
        totals.TotalHours.Should().Be(8);
    }

    [Fact]
    public void A_single_weekday_under_8_hours_is_all_regular()
    {
        var timesheet = new TimesheetBuilder().WithEntry(Day.Tuesday, 6).Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(6);
        totals.OvertimeHours.Should().Be(0);
        totals.TotalHours.Should().Be(6);
    }

    [Fact]
    public void Weekday_hours_beyond_8_spill_into_overtime()
    {
        var timesheet = new TimesheetBuilder().WithEntry(Day.Monday, 10).Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(8);
        totals.OvertimeHours.Should().Be(2);
        totals.TotalHours.Should().Be(10);
    }

    [Fact]
    public void Fractional_weekday_overtime_is_tracked()
    {
        var timesheet = new TimesheetBuilder().WithEntry(Day.Monday, 8.5).Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(8);
        totals.OvertimeHours.Should().BeApproximately(0.5, 1e-9);
        totals.TotalHours.Should().BeApproximately(8.5, 1e-9);
    }

    [Fact]
    public void Saturday_hours_are_all_overtime()
    {
        var timesheet = new TimesheetBuilder().WithEntry(Day.Saturday, 4).Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(0);
        totals.OvertimeHours.Should().Be(4);
        totals.TotalHours.Should().Be(4);
    }

    [Fact]
    public void Sunday_hours_are_all_overtime()
    {
        var timesheet = new TimesheetBuilder().WithEntry(Day.Sunday, 6).Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(0);
        totals.OvertimeHours.Should().Be(6);
        totals.TotalHours.Should().Be(6);
    }

    [Fact]
    public void A_full_Monday_to_Friday_at_8_hours_each_totals_the_standard_40_hour_week()
    {
        var timesheet = new TimesheetBuilder()
            .WithEntry(Day.Monday, 8)
            .WithEntry(Day.Tuesday, 8)
            .WithEntry(Day.Wednesday, 8)
            .WithEntry(Day.Thursday, 8)
            .WithEntry(Day.Friday, 8)
            .Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(OvertimeRules.StandardWorkWeekHours);
        totals.OvertimeHours.Should().Be(0);
        totals.TotalHours.Should().Be(OvertimeRules.StandardWorkWeekHours);
    }

    [Fact]
    public void A_mixed_week_combines_weekday_overtime_with_weekend_overtime()
    {
        var timesheet = new TimesheetBuilder()
            .WithEntry(Day.Monday, 9)
            .WithEntry(Day.Tuesday, 8)
            .WithEntry(Day.Wednesday, 8)
            .WithEntry(Day.Thursday, 8)
            .WithEntry(Day.Friday, 10)
            .WithEntry(Day.Saturday, 5)
            .Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(40);
        totals.OvertimeHours.Should().Be(8);
        totals.TotalHours.Should().Be(48);
    }

    [Fact]
    public void Later_entries_for_the_same_day_replace_earlier_entries()
    {
        var timesheet = new TimesheetBuilder()
            .WithEntry(Day.Monday, 8)
            .WithEntry(Day.Monday, 10)
            .Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(8);
        totals.OvertimeHours.Should().Be(2);
        totals.TotalHours.Should().Be(10);
    }

    [Fact]
    public void A_negative_hours_entry_is_rejected_with_the_identical_cross_language_message()
    {
        Action act = () => new TimesheetBuilder().WithEntry(Day.Monday, -1).Build();

        act.Should().Throw<ArgumentException>()
            .WithMessage("hours must not be negative");
    }

    [Fact]
    public void A_zero_hour_entry_is_valid_and_contributes_nothing()
    {
        var timesheet = new TimesheetBuilder().WithEntry(Day.Monday, 0).Build();

        var totals = timesheet.Totals();

        totals.RegularHours.Should().Be(0);
        totals.OvertimeHours.Should().Be(0);
        totals.TotalHours.Should().Be(0);
    }
}
