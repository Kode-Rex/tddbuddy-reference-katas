using FluentAssertions;

namespace StringCalculator.Tests;

public class CalculatorTests
{
    [Fact]
    public void Empty_string_returns_zero()
    {
        Calculator.Add("").Should().Be(0);
    }
}
