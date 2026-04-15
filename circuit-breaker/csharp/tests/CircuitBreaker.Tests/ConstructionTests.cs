using FluentAssertions;

namespace CircuitBreaker.Tests;

public class ConstructionTests
{
    [Fact]
    public void A_new_breaker_is_in_the_Closed_state()
    {
        var (breaker, _) = new BreakerBuilder().Build();

        breaker.State.Should().Be(BreakerState.Closed);
    }

    [Fact]
    public void Breaker_rejects_non_positive_failure_threshold_with_BreakerThresholdInvalidException()
    {
        var act = () => new BreakerBuilder().WithThreshold(0).Build();

        act.Should().Throw<BreakerThresholdInvalidException>()
            .WithMessage("Failure threshold must be positive");
    }

    [Fact]
    public void Breaker_rejects_non_positive_reset_timeout_with_BreakerTimeoutInvalidException()
    {
        var act = () => new BreakerBuilder().WithTimeout(TimeSpan.Zero).Build();

        act.Should().Throw<BreakerTimeoutInvalidException>()
            .WithMessage("Reset timeout must be positive");
    }
}
