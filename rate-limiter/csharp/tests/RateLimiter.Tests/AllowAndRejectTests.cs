using FluentAssertions;

namespace RateLimiter.Tests;

public class AllowAndRejectTests
{
    private static readonly TimeSpan TenSeconds = TimeSpan.FromSeconds(10);

    [Fact]
    public void The_first_request_for_a_key_is_allowed()
    {
        var (limiter, _) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();

        var decision = limiter.Request("alice");

        decision.Should().BeOfType<Decision.Allowed>();
    }

    [Fact]
    public void Requests_up_to_the_limit_within_the_window_are_all_allowed()
    {
        var (limiter, _) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();

        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
    }

    [Fact]
    public void Each_Allowed_decision_carries_no_retryAfter()
    {
        var (limiter, _) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();

        var decision = limiter.Request("alice");

        decision.Should().BeOfType<Decision.Allowed>();
        // Allowed is a sealed record with no retryAfter member — shape assertion by type.
    }

    [Fact]
    public void The_request_past_the_limit_within_the_window_is_rejected()
    {
        var (limiter, _) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        var decision = limiter.Request("alice");

        decision.Should().BeOfType<Decision.Rejected>();
    }

    [Fact]
    public void A_rejection_reports_retryAfter_as_the_remaining_window_duration()
    {
        var (limiter, clock) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        clock.Advance(TimeSpan.FromSeconds(3));
        var decision = limiter.Request("alice");

        decision.Should().BeOfType<Decision.Rejected>()
            .Which.RetryAfter.Should().Be(TimeSpan.FromSeconds(7));
    }

    [Fact]
    public void A_rejected_request_does_not_count_against_the_window()
    {
        var (limiter, clock) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        // Five rejections in quick succession should not bump the count past max,
        // so once the window resets we still get exactly MaxRequests fresh allows.
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        clock.Advance(TenSeconds);
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Allowed>();
        limiter.Request("alice").Should().BeOfType<Decision.Rejected>();
    }

    [Fact]
    public void Repeated_rejections_report_a_decreasing_retryAfter_as_the_clock_advances()
    {
        var (limiter, clock) = new LimiterBuilder().WithMaxRequests(3).WithWindow(TenSeconds).Build();
        limiter.Request("alice");
        limiter.Request("alice");
        limiter.Request("alice");

        clock.Advance(TimeSpan.FromSeconds(2));
        var first = limiter.Request("alice") as Decision.Rejected;

        clock.Advance(TimeSpan.FromSeconds(3));
        var second = limiter.Request("alice") as Decision.Rejected;

        first!.RetryAfter.Should().Be(TimeSpan.FromSeconds(8));
        second!.RetryAfter.Should().Be(TimeSpan.FromSeconds(5));
    }
}
