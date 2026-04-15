using FluentAssertions;

namespace RomanNumerals.Tests;

public class RomanTests
{
    [Fact]
    public void One_is_I()
    {
        Roman.ToRoman(1).Should().Be("I");
    }

    [Fact]
    public void Two_is_II()
    {
        Roman.ToRoman(2).Should().Be("II");
    }
}
