using FluentAssertions;
using Xunit;

namespace FizzBuzzWhiz.Tests;

public class FizzBuzzWhizTests
{
    [Fact]
    public void One_returns_the_number_as_a_string()
    {
        FizzBuzzWhiz.Say(1).Should().Be("1");
    }

    [Fact]
    public void Two_returns_the_number_as_a_string()
    {
        FizzBuzzWhiz.Say(2).Should().Be("2");
    }

    [Fact]
    public void Three_is_divisible_by_three_and_returns_Fizz()
    {
        FizzBuzzWhiz.Say(3).Should().Be("Fizz");
    }

    [Fact]
    public void Five_is_divisible_by_five_and_returns_Buzz()
    {
        FizzBuzzWhiz.Say(5).Should().Be("Buzz");
    }

    [Fact]
    public void Six_is_divisible_by_three_and_returns_Fizz()
    {
        FizzBuzzWhiz.Say(6).Should().Be("Fizz");
    }

    [Fact]
    public void Ten_is_divisible_by_five_and_returns_Buzz()
    {
        FizzBuzzWhiz.Say(10).Should().Be("Buzz");
    }

    [Fact]
    public void Fifteen_is_divisible_by_both_three_and_five_and_returns_FizzBuzz()
    {
        FizzBuzzWhiz.Say(15).Should().Be("FizzBuzz");
    }

    [Fact]
    public void Thirty_is_divisible_by_both_three_and_five_and_returns_FizzBuzz()
    {
        FizzBuzzWhiz.Say(30).Should().Be("FizzBuzz");
    }
}
