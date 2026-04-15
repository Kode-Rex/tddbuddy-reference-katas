using FluentAssertions;

namespace CircuitBreaker.Tests;

public class ClosedStateTests
{
    private static Func<string> Succeeds(string value) => () => value;

    private static Func<string> Fails() => () => throw new InvalidOperationException("boom");

    [Fact]
    public void Execute_in_Closed_returns_the_operations_result_on_success()
    {
        var (breaker, _) = new BreakerBuilder().WithThreshold(3).Build();

        var result = breaker.Execute(Succeeds("ok"));

        result.Should().Be("ok");
        breaker.State.Should().Be(BreakerState.Closed);
    }

    [Fact]
    public void Execute_in_Closed_rethrows_the_operations_exception_on_failure()
    {
        var (breaker, _) = new BreakerBuilder().WithThreshold(3).Build();

        var act = () => breaker.Execute(Fails());

        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
    }

    [Fact]
    public void A_single_failure_in_Closed_does_not_trip_the_breaker()
    {
        var (breaker, _) = new BreakerBuilder().WithThreshold(3).Build();

        try { breaker.Execute(Fails()); } catch { /* swallow */ }

        breaker.State.Should().Be(BreakerState.Closed);
    }

    [Fact]
    public void Reaching_the_failure_threshold_in_Closed_transitions_to_Open()
    {
        var (breaker, _) = new BreakerBuilder().WithThreshold(3).Build();

        for (var i = 0; i < 3; i++)
        {
            try { breaker.Execute(Fails()); } catch { /* swallow */ }
        }

        breaker.State.Should().Be(BreakerState.Open);
    }

    [Fact]
    public void A_success_in_Closed_resets_the_consecutive_failure_counter()
    {
        var (breaker, _) = new BreakerBuilder().WithThreshold(3).Build();
        try { breaker.Execute(Fails()); } catch { }
        try { breaker.Execute(Fails()); } catch { }

        breaker.Execute(Succeeds("ok"));

        // Two more failures should not trip, because the counter reset.
        try { breaker.Execute(Fails()); } catch { }
        try { breaker.Execute(Fails()); } catch { }

        breaker.State.Should().Be(BreakerState.Closed);
    }

    [Fact]
    public void Consecutive_failures_below_the_threshold_stay_Closed_even_after_many_operations()
    {
        var (breaker, _) = new BreakerBuilder().WithThreshold(3).Build();

        for (var i = 0; i < 10; i++)
        {
            try { breaker.Execute(Fails()); } catch { }
            breaker.Execute(Succeeds("ok"));
        }

        breaker.State.Should().Be(BreakerState.Closed);
    }
}
