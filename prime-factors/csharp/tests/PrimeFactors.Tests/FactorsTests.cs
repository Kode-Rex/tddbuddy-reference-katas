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
}
