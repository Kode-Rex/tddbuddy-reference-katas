using FluentAssertions;
using Xunit;

namespace LeapYear.Tests;

public class LeapYearTests
{
    [Fact]
    public void Year_2023_is_not_a_leap_year_because_it_is_not_divisible_by_four()
    {
        LeapYear.IsLeapYear(2023).Should().BeFalse();
    }

    [Fact]
    public void Year_2024_is_a_leap_year_because_it_is_divisible_by_four_and_not_by_one_hundred()
    {
        LeapYear.IsLeapYear(2024).Should().BeTrue();
    }

    [Fact]
    public void Year_2020_is_a_leap_year_because_it_is_divisible_by_four_and_not_by_one_hundred()
    {
        LeapYear.IsLeapYear(2020).Should().BeTrue();
    }

    [Fact]
    public void Year_1900_is_not_a_leap_year_because_it_is_divisible_by_one_hundred_but_not_by_four_hundred()
    {
        LeapYear.IsLeapYear(1900).Should().BeFalse();
    }

    [Fact]
    public void Year_2100_is_not_a_leap_year_because_it_is_divisible_by_one_hundred_but_not_by_four_hundred()
    {
        LeapYear.IsLeapYear(2100).Should().BeFalse();
    }

    [Fact]
    public void Year_2000_is_a_leap_year_because_it_is_divisible_by_four_hundred()
    {
        LeapYear.IsLeapYear(2000).Should().BeTrue();
    }

    [Fact]
    public void Year_1600_is_a_leap_year_because_it_is_divisible_by_four_hundred()
    {
        LeapYear.IsLeapYear(1600).Should().BeTrue();
    }

    [Fact]
    public void Year_2001_is_not_a_leap_year_because_it_is_not_divisible_by_four()
    {
        LeapYear.IsLeapYear(2001).Should().BeFalse();
    }
}
