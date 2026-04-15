using FluentAssertions;
using Xunit;

namespace BalancedBrackets.Tests;

public class BalancedBracketsTests
{
    [Fact]
    public void Empty_string_is_balanced()
    {
        BalancedBrackets.IsBalanced("").Should().BeTrue();
    }

    [Fact]
    public void Single_pair_is_balanced()
    {
        BalancedBrackets.IsBalanced("[]").Should().BeTrue();
    }

    [Fact]
    public void Two_sequential_pairs_are_balanced()
    {
        BalancedBrackets.IsBalanced("[][]").Should().BeTrue();
    }

    [Fact]
    public void Nested_pair_is_balanced()
    {
        BalancedBrackets.IsBalanced("[[]]").Should().BeTrue();
    }

    [Fact]
    public void Deeply_nested_mixed_pairs_are_balanced()
    {
        BalancedBrackets.IsBalanced("[[[][]]]").Should().BeTrue();
    }

    [Fact]
    public void Closing_before_opening_is_not_balanced()
    {
        BalancedBrackets.IsBalanced("][").Should().BeFalse();
    }

    [Fact]
    public void Alternating_reversed_pairs_are_not_balanced()
    {
        BalancedBrackets.IsBalanced("][][").Should().BeFalse();
    }

    [Fact]
    public void Trailing_imbalance_is_not_balanced()
    {
        BalancedBrackets.IsBalanced("[][]][").Should().BeFalse();
    }

    [Fact]
    public void A_lone_opener_is_not_balanced()
    {
        BalancedBrackets.IsBalanced("[").Should().BeFalse();
    }

    [Fact]
    public void A_lone_closer_is_not_balanced()
    {
        BalancedBrackets.IsBalanced("]").Should().BeFalse();
    }

    [Fact]
    public void Unmatched_opener_at_end_is_not_balanced()
    {
        BalancedBrackets.IsBalanced("[[]").Should().BeFalse();
    }

    [Fact]
    public void Unmatched_closer_at_end_is_not_balanced()
    {
        BalancedBrackets.IsBalanced("[]]").Should().BeFalse();
    }
}
