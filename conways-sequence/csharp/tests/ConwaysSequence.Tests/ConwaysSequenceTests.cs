using FluentAssertions;
using Xunit;

namespace ConwaysSequence.Tests;

public class ConwaysSequenceTests
{
    [Fact]
    public void Next_term_of_one_is_one_one()
    {
        ConwaysSequence.NextTerm("1").Should().Be("11");
    }

    [Fact]
    public void Next_term_of_one_one_is_two_one()
    {
        ConwaysSequence.NextTerm("11").Should().Be("21");
    }

    [Fact]
    public void Next_term_of_two_one_is_one_two_one_one()
    {
        ConwaysSequence.NextTerm("21").Should().Be("1211");
    }

    [Fact]
    public void Next_term_of_one_two_one_one_is_one_one_one_two_two_one()
    {
        ConwaysSequence.NextTerm("1211").Should().Be("111221");
    }

    [Fact]
    public void Next_term_of_one_one_one_two_two_one_is_three_one_two_two_one_one()
    {
        ConwaysSequence.NextTerm("111221").Should().Be("312211");
    }

    [Fact]
    public void Next_term_of_a_single_two_is_one_two()
    {
        ConwaysSequence.NextTerm("2").Should().Be("12");
    }

    [Fact]
    public void Next_term_of_two_two_is_a_fixed_point()
    {
        ConwaysSequence.NextTerm("22").Should().Be("22");
    }

    [Fact]
    public void Next_term_of_three_two_one_one_is_one_three_one_two_two_one()
    {
        ConwaysSequence.NextTerm("3211").Should().Be("131221");
    }

    [Fact]
    public void Next_term_of_ten_consecutive_ones_describes_ten_ones()
    {
        ConwaysSequence.NextTerm("1111111111").Should().Be("101");
    }

    [Fact]
    public void Look_and_say_with_zero_iterations_returns_the_seed_unchanged()
    {
        ConwaysSequence.LookAndSay("1", 0).Should().Be("1");
    }

    [Fact]
    public void Look_and_say_with_one_iteration_equals_a_single_next_term()
    {
        ConwaysSequence.LookAndSay("1", 1).Should().Be("11");
    }

    [Fact]
    public void Look_and_say_with_five_iterations_from_one_lands_on_three_one_two_two_one_one()
    {
        ConwaysSequence.LookAndSay("1", 5).Should().Be("312211");
    }

    [Fact]
    public void Look_and_say_with_two_iterations_from_seed_two_is_one_one_one_two()
    {
        ConwaysSequence.LookAndSay("2", 2).Should().Be("1112");
    }

    [Fact]
    public void Look_and_say_with_negative_iterations_is_rejected()
    {
        var act = () => ConwaysSequence.LookAndSay("1", -1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
