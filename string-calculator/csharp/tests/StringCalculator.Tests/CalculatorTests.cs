using FluentAssertions;

namespace StringCalculator.Tests;

public class CalculatorTests
{
    [Fact]
    public void Empty_string_returns_zero()
    {
        Calculator.Add("").Should().Be(0);
    }

    [Fact]
    public void Single_number_returns_itself()
    {
        Calculator.Add("1").Should().Be(1);
    }

    [Fact]
    public void Two_numbers_return_their_sum()
    {
        Calculator.Add("1,2").Should().Be(3);
    }

    [Fact]
    public void Many_numbers_returns_their_sum()
    {
        Calculator.Add("1,2,3,4").Should().Be(10);
    }

    [Fact]
    public void Newline_is_also_a_delimiter()
    {
        Calculator.Add("1\n2,3").Should().Be(6);
    }

    [Fact]
    public void Custom_single_char_delimiter_is_declared_in_header()
    {
        Calculator.Add("//;\n1;2").Should().Be(3);
    }
}
