using FluentAssertions;

namespace CircuitBreaker.Tests;

public class TransitionTests
{
    private static readonly TimeSpan ThirtySeconds = TimeSpan.FromSeconds(30);

    private static Func<string> Succeeds(string value) => () => value;

    private static Func<string> Fails() => () => throw new InvalidOperationException("boom");

    private static void TripToOpen(Breaker breaker)
    {
        for (var i = 0; i < 3; i++)
        {
            try { breaker.Execute(Fails()); } catch { }
        }
    }

    [Fact]
    public void Execute_before_the_reset_timeout_elapses_still_fails_fast()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        TripToOpen(breaker);

        clock.Advance(TimeSpan.FromSeconds(29));

        var act = () => breaker.Execute(Succeeds("ok"));
        act.Should().Throw<CircuitOpenException>();
    }

    [Fact]
    public void Execute_after_the_reset_timeout_elapses_probes_the_operation_in_HalfOpen()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        TripToOpen(breaker);
        clock.Advance(ThirtySeconds);

        var probed = false;
        breaker.Execute<string>(() => { probed = true; return "ok"; });

        probed.Should().BeTrue();
    }

    [Fact]
    public void A_successful_probe_transitions_HalfOpen_to_Closed()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        TripToOpen(breaker);
        clock.Advance(ThirtySeconds);

        breaker.Execute(Succeeds("ok"));

        breaker.State.Should().Be(BreakerState.Closed);
    }

    [Fact]
    public void A_successful_probe_resets_the_consecutive_failure_counter()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        TripToOpen(breaker);
        clock.Advance(ThirtySeconds);
        breaker.Execute(Succeeds("ok"));

        // Two more failures should not re-trip, because the counter reset.
        try { breaker.Execute(Fails()); } catch { }
        try { breaker.Execute(Fails()); } catch { }

        breaker.State.Should().Be(BreakerState.Closed);
    }

    [Fact]
    public void A_failed_probe_transitions_HalfOpen_back_to_Open_and_rethrows()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        TripToOpen(breaker);
        clock.Advance(ThirtySeconds);

        var act = () => breaker.Execute(Fails());

        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
        breaker.State.Should().Be(BreakerState.Open);
    }

    [Fact]
    public void A_failed_probe_restarts_the_reset_timeout()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        TripToOpen(breaker);
        clock.Advance(ThirtySeconds);
        try { breaker.Execute(Fails()); } catch { }

        // Less than thirty more seconds: still Open, still fails fast.
        clock.Advance(TimeSpan.FromSeconds(29));
        var act = () => breaker.Execute(Succeeds("ok"));
        act.Should().Throw<CircuitOpenException>();
    }
}
