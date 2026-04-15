using FluentAssertions;

namespace RateLimiter.Tests;

public class WorkedExampleTests
{
    private static readonly TimeSpan TenSeconds = TimeSpan.FromSeconds(10);

    [Fact]
    public void Fixed_window_cycle_for_alice_and_bob_produces_the_documented_sequence()
    {
        var start = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var (limiter, clock) = new LimiterBuilder()
            .WithMaxRequests(3)
            .WithWindow(TenSeconds)
            .StartingAt(start)
            .Build();

        // t=0,1,2 — alice allowed
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        clock.Advance(TimeSpan.FromSeconds(1));
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        clock.Advance(TimeSpan.FromSeconds(1));
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();

        // t=3 — alice rejected with retryAfter=7s
        clock.Advance(TimeSpan.FromSeconds(1));
        limiter.Request("alice").Should().BeOfType<Decision.Rejected>()
            .Which.RetryAfter.Should().Be(TimeSpan.FromSeconds(7));

        // t=3 — bob is independent, allowed
        limiter.Request("bob").Should().BeOfType<Decision.Allowed>();

        // t=10 — alice's window has elapsed, fresh quota
        clock.Advance(TimeSpan.FromSeconds(7));
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Rejected>();
    }
}
