using FluentAssertions;

namespace CircuitBreaker.Tests;

public class OpenStateTests
{
    private static readonly TimeSpan ThirtySeconds = TimeSpan.FromSeconds(30);

    private static Func<string> Fails() => () => throw new InvalidOperationException("boom");

    private static (Breaker, FixedClock) Tripped()
    {
        var (breaker, clock) = new BreakerBuilder().WithThreshold(3).WithTimeout(ThirtySeconds).Build();
        for (var i = 0; i < 3; i++)
        {
            try { breaker.Execute(Fails()); } catch { }
        }
        return (breaker, clock);
    }

    [Fact]
    public void Execute_in_Open_throws_CircuitOpenException_without_calling_the_operation()
    {
        var (breaker, _) = Tripped();
        var called = false;

        var act = () => breaker.Execute<string>(() => { called = true; return "unused"; });

        act.Should().Throw<CircuitOpenException>();
        called.Should().BeFalse();
    }

    [Fact]
    public void The_state_remains_Open_after_a_fail_fast_rejection()
    {
        var (breaker, _) = Tripped();

        try { breaker.Execute<string>(() => "unused"); } catch (CircuitOpenException) { }

        breaker.State.Should().Be(BreakerState.Open);
    }

    [Fact]
    public void The_CircuitOpenException_message_is_Circuit_is_open()
    {
        var (breaker, _) = Tripped();

        var act = () => breaker.Execute<string>(() => "unused");

        act.Should().Throw<CircuitOpenException>().WithMessage("Circuit is open");
    }
}
