using FluentAssertions;
using Xunit;

namespace LastSunday.Tests;

public class LastSundayTests
{
    [Fact]
    public void Last_Sunday_of_January_2013_is_the_twenty_seventh()
    {
        LastSunday.Find(2013, 1).Should().Be(new DateOnly(2013, 1, 27));
    }

    [Fact]
    public void Last_Sunday_of_February_2013_is_the_twenty_fourth_in_a_non_leap_February()
    {
        LastSunday.Find(2013, 2).Should().Be(new DateOnly(2013, 2, 24));
    }

    [Fact]
    public void Last_Sunday_of_March_2013_is_the_thirty_first_when_the_last_day_itself_is_Sunday()
    {
        LastSunday.Find(2013, 3).Should().Be(new DateOnly(2013, 3, 31));
    }

    [Fact]
    public void Last_Sunday_of_April_2013_is_the_twenty_eighth_in_a_thirty_day_month()
    {
        LastSunday.Find(2013, 4).Should().Be(new DateOnly(2013, 4, 28));
    }

    [Fact]
    public void Last_Sunday_of_June_2013_is_the_thirtieth_when_the_last_day_is_Sunday()
    {
        LastSunday.Find(2013, 6).Should().Be(new DateOnly(2013, 6, 30));
    }

    [Fact]
    public void Last_Sunday_of_December_2013_is_the_twenty_ninth_at_the_year_end_boundary()
    {
        LastSunday.Find(2013, 12).Should().Be(new DateOnly(2013, 12, 29));
    }

    [Fact]
    public void Last_Sunday_of_February_2020_is_the_twenty_third_in_a_leap_February_ending_Saturday()
    {
        LastSunday.Find(2020, 2).Should().Be(new DateOnly(2020, 2, 23));
    }

    [Fact]
    public void Last_Sunday_of_February_2032_is_the_twenty_ninth_when_leap_day_itself_is_Sunday()
    {
        LastSunday.Find(2032, 2).Should().Be(new DateOnly(2032, 2, 29));
    }

    [Fact]
    public void Last_Sunday_of_February_2100_is_the_twenty_eighth_in_a_century_non_leap_February()
    {
        LastSunday.Find(2100, 2).Should().Be(new DateOnly(2100, 2, 28));
    }

    [Fact]
    public void Last_Sunday_of_December_2000_is_the_thirty_first_in_a_four_hundred_divisible_leap_year()
    {
        LastSunday.Find(2000, 12).Should().Be(new DateOnly(2000, 12, 31));
    }
}
