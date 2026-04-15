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

    [Fact]
    public void Three_is_III()
    {
        Roman.ToRoman(3).Should().Be("III");
    }

    [Fact]
    public void Five_is_V()
    {
        Roman.ToRoman(5).Should().Be("V");
    }

    [Fact]
    public void Four_is_IV()
    {
        Roman.ToRoman(4).Should().Be("IV");
    }

    [Fact]
    public void Ten_is_X()
    {
        Roman.ToRoman(10).Should().Be("X");
    }

    [Fact]
    public void Nine_is_IX()
    {
        Roman.ToRoman(9).Should().Be("IX");
    }

    [Fact]
    public void Forty_is_XL()
    {
        Roman.ToRoman(40).Should().Be("XL");
    }
}
