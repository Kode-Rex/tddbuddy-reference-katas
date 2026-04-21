using FluentAssertions;
using Xunit;

namespace MultiThreadedSanta.Tests;

public class CostCalculatorTests
{
    [Fact]
    public void Cost_is_zero_when_no_elves_are_used()
    {
        var cost = CostCalculator.CalculateCookies(0, TimeSpan.FromSeconds(100));

        cost.Should().Be(0);
    }

    [Fact]
    public void Cost_equals_elves_multiplied_by_elapsed_seconds()
    {
        var cost = CostCalculator.CalculateCookies(5, TimeSpan.FromSeconds(10));

        cost.Should().Be(50);
    }

    [Fact]
    public void More_elves_with_shorter_time_can_cost_the_same_as_fewer_elves_with_longer_time()
    {
        var costA = CostCalculator.CalculateCookies(10, TimeSpan.FromSeconds(5));
        var costB = CostCalculator.CalculateCookies(5, TimeSpan.FromSeconds(10));

        costA.Should().Be(costB);
    }
}
