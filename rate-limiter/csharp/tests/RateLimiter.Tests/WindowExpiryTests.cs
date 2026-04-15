using FluentAssertions;

namespace RateLimiter.Tests;

public class WindowExpiryTests
{
    private static readonly TimeSpan TenSeconds = TimeSpan.FromSeconds(10);

    [Fact]
    public void A_request_exactly_at_the_window_boundary_opens_a_fresh_window_and_is_allowed()
    {
        var (limiter, clock) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        clock.Advance(TenSeconds);
        var decision = limiter.Request("alice");

        decision.Should().BeOfType<Decision.Allowed>();
    }

    [Fact]
    public void A_request_after_the_window_has_fully_elapsed_opens_a_fresh_window_and_is_allowed()
    {
        var (limiter, clock) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        clock.Advance(TimeSpan.FromSeconds(15));
        var decision = limiter.Request("alice");

        decision.Should().BeOfType<Decision.Allowed>();
    }

    [Fact]
    public void After_a_window_resets_the_full_quota_is_available_again()
    {
        var (limiter, clock) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        clock.Advance(TenSeconds);

        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Rejected>();
    }
}
