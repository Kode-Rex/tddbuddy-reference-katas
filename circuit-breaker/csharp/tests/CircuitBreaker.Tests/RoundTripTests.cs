using FluentAssertions;

namespace CircuitBreaker.Tests;

public class RoundTripTests
{
    private static readonly TimeSpan ThirtySeconds = TimeSpan.FromSeconds(30);

    private static Func<string> Succeeds(string value) => () => value;

    private static Func<string> Fails() => () => throw new InvalidOperationException("boom");

    [Fact]
    public void Closed_Open_HalfOpen_Closed_round_trip_from_the_kata_brief_example()
    {
        // Threshold 3, timeout 30s: failing thrice trips; after 30s a success probe closes.
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();

        breaker.Execute(Succeeds("ok")).Should().Be("ok");      // step 1
        try { breaker.Execute(Fails()); } catch { }             // step 2: 1 fail
        try { breaker.Execute(Fails()); } catch { }             // step 3: 2 fails
        try { breaker.Execute(Fails()); } catch { }             // step 4: 3 fails → Open
        breaker.State.Should().Be(BreakerState.Open);

        var act = () => breaker.Execute(Succeeds("ignored"));    // step 5: fail fast
        act.Should().Throw<CircuitOpenException>();

        clock.Advance(ThirtySeconds);                            // step 6 setup
        breaker.Execute(Succeeds("ok")).Should().Be("ok");       // step 6: probe success → Closed
        breaker.State.Should().Be(BreakerState.Closed);
    }

    [Fact]
    public void Closed_Open_HalfOpen_Open_cycle_when_the_probe_fails()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        for (var i = 0; i < 3; i++)
        {
            try { breaker.Execute(Fails()); } catch { }
        }
        clock.Advance(ThirtySeconds);

        var act = () => breaker.Execute(Fails());                // probe fails → back to Open
        act.Should().Throw<InvalidOperationException>();
        breaker.State.Should().Be(BreakerState.Open);
    }
}
