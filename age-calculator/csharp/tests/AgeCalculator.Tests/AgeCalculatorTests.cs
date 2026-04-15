using FluentAssertions;
using Xunit;

namespace AgeCalculator.Tests;

public class AgeCalculatorTests
{
    [Fact]
    public void Zenith_born_2016_10_28_on_2022_11_05_is_six()
    {
        AgeCalculator.Calculate(new DateOnly(2016, 10, 28), new DateOnly(2022, 11, 5))
            .Should().Be(6);
    }

    [Fact]
    public void Zenith_on_her_seventh_birthday_is_seven()
    {
        AgeCalculator.Calculate(new DateOnly(2016, 10, 28), new DateOnly(2023, 10, 28))
            .Should().Be(7);
    }

    [Fact]
    public void Zenith_on_the_day_before_her_seventh_birthday_is_six()
    {
        AgeCalculator.Calculate(new DateOnly(2016, 10, 28), new DateOnly(2023, 10, 27))
            .Should().Be(6);
    }

    [Fact]
    public void Born_2000_01_01_on_2024_12_31_is_twenty_four()
    {
        AgeCalculator.Calculate(new DateOnly(2000, 1, 1), new DateOnly(2024, 12, 31))
            .Should().Be(24);
    }

    [Fact]
    public void Born_today_is_zero()
    {
        AgeCalculator.Calculate(new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 1))
            .Should().Be(0);
    }

    [Fact]
    public void Leap_day_baby_on_February_28_in_a_non_leap_year_has_not_yet_aged_up()
    {
        AgeCalculator.Calculate(new DateOnly(2000, 2, 29), new DateOnly(2001, 2, 28))
            .Should().Be(0);
    }

    [Fact]
    public void Leap_day_baby_ages_up_on_March_1_in_a_non_leap_year()
    {
        AgeCalculator.Calculate(new DateOnly(2000, 2, 29), new DateOnly(2001, 3, 1))
            .Should().Be(1);
    }

    [Fact]
    public void Leap_day_baby_on_an_actual_leap_day_ages_up_exactly()
    {
        AgeCalculator.Calculate(new DateOnly(2000, 2, 29), new DateOnly(2004, 2, 29))
            .Should().Be(4);
    }

    [Fact]
    public void Born_yesterday_across_a_year_boundary_is_still_zero()
    {
        AgeCalculator.Calculate(new DateOnly(1999, 12, 31), new DateOnly(2000, 1, 1))
            .Should().Be(0);
    }

    [Fact]
    public void Born_1990_06_15_on_2024_06_14_is_thirty_three()
    {
        AgeCalculator.Calculate(new DateOnly(1990, 6, 15), new DateOnly(2024, 6, 14))
            .Should().Be(33);
    }

    [Fact]
    public void Throws_when_birthdate_is_after_today()
    {
        var act = () => AgeCalculator.Calculate(new DateOnly(2024, 6, 15), new DateOnly(2024, 6, 14));
        act.Should().Throw<ArgumentException>().WithMessage("birthdate is after today");
    }
}
