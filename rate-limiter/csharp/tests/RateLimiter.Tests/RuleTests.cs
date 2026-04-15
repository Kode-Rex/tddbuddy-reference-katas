using FluentAssertions;

namespace RateLimiter.Tests;

public class RuleTests
{
    [Fact]
    public void A_rule_with_positive_max_requests_and_positive_window_is_valid()
    {
        var rule = new Rule(3, TimeSpan.FromSeconds(10));

        rule.MaxRequests.Should().Be(3);
        rule.Window.Should().Be(TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void A_rule_rejects_zero_max_requests_with_LimiterRuleInvalidException()
    {
        var act = () => new Rule(0, TimeSpan.FromSeconds(10));

        act.Should().Throw<LimiterRuleInvalidException>()
            .WithMessage("Max requests must be positive");
    }

    [Fact]
    public void A_rule_rejects_negative_max_requests_with_LimiterRuleInvalidException()
    {
        var act = () => new Rule(-1, TimeSpan.FromSeconds(10));

        act.Should().Throw<LimiterRuleInvalidException>()
            .WithMessage("Max requests must be positive");
    }

    [Fact]
    public void A_rule_rejects_zero_window_with_LimiterRuleInvalidException()
    {
        var act = () => new Rule(3, TimeSpan.Zero);

        act.Should().Throw<LimiterRuleInvalidException>()
            .WithMessage("Window must be positive");
    }

    [Fact]
    public void A_rule_rejects_negative_window_with_LimiterRuleInvalidException()
    {
        var act = () => new Rule(3, TimeSpan.FromSeconds(-1));

        act.Should().Throw<LimiterRuleInvalidException>()
            .WithMessage("Window must be positive");
    }
}
