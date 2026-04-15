using FluentAssertions;

namespace PrimeFactors.Tests;

public class FactorsTests
{
    [Fact]
    public void One_has_no_prime_factors()
    {
        Factors.Generate(1).Should().BeEmpty();
    }

    [Fact]
    public void Two_is_its_own_only_prime_factor()
    {
        Factors.Generate(2).Should().Equal(2);
    }

    [Fact]
    public void Three_is_its_own_only_prime_factor()
    {
        Factors.Generate(3).Should().Equal(3);
    }

    [Fact]
    public void Four_factors_into_two_twos()
    {
        Factors.Generate(4).Should().Equal(2, 2);
    }

    [Fact]
    public void Six_factors_into_two_and_three()
    {
        Factors.Generate(6).Should().Equal(2, 3);
    }
}
