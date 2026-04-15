using FluentAssertions;

namespace PrimeFactors.Tests;

public class FactorsTests
{
    [Fact]
    public void One_has_no_prime_factors()
    {
        Factors.Generate(1).Should().BeEmpty();
    }
}
