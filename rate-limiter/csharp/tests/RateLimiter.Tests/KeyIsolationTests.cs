using FluentAssertions;

namespace RateLimiter.Tests;

public class KeyIsolationTests
{
    private static readonly TimeSpan TenSeconds = TimeSpan.FromSeconds(10);

    [Fact]
    public void Two_different_keys_have_independent_counts()
    {
        var (limiter, _) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();

        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();

        limiter.Request("bob").Should().BeOfType<Decision.Allowed>();
        limiter.Request("bob").Should().BeOfType<Decision.Allowed>();
        limiter.Request("bob").Should().BeOfType<Decision.Allowed>();
    }

    [Fact]
    public void A_rejection_on_one_key_does_not_affect_another_keys_decisions()
    {
        var (limiter, _) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice").Should().BeOfType<Decision.Rejected>();

        limiter.Request("bob").Should().BeOfType<Decision.Allowed>();
    }

    [Fact]
    public void Each_keys_window_starts_from_its_own_first_request()
    {
        var (limiter, clock) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        clock.Advance(TimeSpan.FromSeconds(5));
        limiter.Request("bob");
        limiter.Request("bob");
        limiter.Request("bob");

        // Alice's window opened at t=0, so it ends at t=10. We're at t=5.
        var aliceDecision = limiter.Request("alice");
        aliceDecision.Should().BeOfType<Decision.Rejected>()
            .Which.RetryAfter.Should().Be(TimeSpan.FromSeconds(5));

        // Bob's window opened at t=5, so it ends at t=15. Still at t=5.
        var bobDecision = limiter.Request("bob");
        bobDecision.Should().BeOfType<Decision.Rejected>()
            .Which.RetryAfter.Should().Be(TimeSpan.FromSeconds(10));
    }
}
