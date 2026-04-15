using FluentAssertions;
using Xunit;

namespace TimeZoneConverter.Tests;

public class TimeZoneConverterTests
{
    [Fact]
    public void Identity_conversion_between_UTC_and_UTC_returns_the_same_local_time()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 12, 0, 0),
            TimeSpan.Zero,
            TimeSpan.Zero)
            .Should().Be(new DateTime(2024, 6, 15, 12, 0, 0));
    }

    [Fact]
    public void Westward_conversion_from_UTC_to_minus_five_subtracts_five_hours()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 12, 0, 0),
            TimeSpan.Zero,
            TimeSpan.FromHours(-5))
            .Should().Be(new DateTime(2024, 6, 15, 7, 0, 0));
    }

    [Fact]
    public void Eastward_conversion_from_UTC_to_plus_nine_adds_nine_hours()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 12, 0, 0),
            TimeSpan.Zero,
            TimeSpan.FromHours(9))
            .Should().Be(new DateTime(2024, 6, 15, 21, 0, 0));
    }

    [Fact]
    public void Cross_zone_conversion_without_UTC_transits_via_the_shared_instant()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 9, 0, 0),
            TimeSpan.FromHours(-5),
            TimeSpan.FromHours(9))
            .Should().Be(new DateTime(2024, 6, 15, 23, 0, 0));
    }

    [Fact]
    public void Forward_conversion_across_midnight_rolls_into_the_next_day()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 22, 0, 0),
            TimeSpan.Zero,
            TimeSpan.FromHours(5))
            .Should().Be(new DateTime(2024, 6, 16, 3, 0, 0));
    }

    [Fact]
    public void Backward_conversion_across_midnight_rolls_into_the_previous_day()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 2, 0, 0),
            TimeSpan.Zero,
            TimeSpan.FromHours(-5))
            .Should().Be(new DateTime(2024, 6, 14, 21, 0, 0));
    }

    [Fact]
    public void Half_hour_offset_plus_five_thirty_handles_non_integer_hour_zones()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 12, 0, 0),
            TimeSpan.Zero,
            new TimeSpan(5, 30, 0))
            .Should().Be(new DateTime(2024, 6, 15, 17, 30, 0));
    }

    [Fact]
    public void Quarter_hour_offset_plus_five_forty_five_handles_forty_five_minute_zones()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 12, 0, 0),
            TimeSpan.Zero,
            new TimeSpan(5, 45, 0))
            .Should().Be(new DateTime(2024, 6, 15, 17, 45, 0));
    }

    [Fact]
    public void Forward_conversion_across_month_boundary_rolls_June_into_July()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 30, 23, 30, 0),
            TimeSpan.Zero,
            TimeSpan.FromHours(2))
            .Should().Be(new DateTime(2024, 7, 1, 1, 30, 0));
    }

    [Fact]
    public void Backward_conversion_across_year_boundary_rolls_into_the_previous_year()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 1, 1, 1, 0, 0),
            TimeSpan.Zero,
            TimeSpan.FromHours(-5))
            .Should().Be(new DateTime(2023, 12, 31, 20, 0, 0));
    }

    [Fact]
    public void Forward_conversion_across_leap_day_rolls_February_twenty_ninth_into_March_first()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 2, 29, 23, 0, 0),
            TimeSpan.Zero,
            TimeSpan.FromHours(2))
            .Should().Be(new DateTime(2024, 3, 1, 1, 0, 0));
    }

    [Fact]
    public void International_date_line_swing_from_plus_twelve_to_minus_twelve_steps_back_one_day()
    {
        TimeZoneConverter.Convert(
            new DateTime(2024, 6, 15, 10, 0, 0),
            TimeSpan.FromHours(12),
            TimeSpan.FromHours(-12))
            .Should().Be(new DateTime(2024, 6, 14, 10, 0, 0));
    }
}
