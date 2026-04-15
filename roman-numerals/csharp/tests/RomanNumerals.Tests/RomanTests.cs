using FluentAssertions;

namespace RomanNumerals.Tests;

public class RomanTests
{
    [Fact]
    public void One_is_I()
    {
        Roman.ToRoman(1).Should().Be("I");
    }
}
