using FluentAssertions;
using Xunit;

namespace TextJustification.Tests;

public class TextJustifierTests
{
    [Fact]
    public void An_empty_string_returns_an_empty_list()
    {
        TextJustifier.Justify("", 10).Should().BeEmpty();
    }

    [Fact]
    public void A_whitespace_only_string_returns_an_empty_list()
    {
        TextJustifier.Justify("   \t  ", 10).Should().BeEmpty();
    }

    [Fact]
    public void A_single_word_shorter_than_width_is_its_own_last_line()
    {
        TextJustifier.Justify("Word", 10).Should().Equal("Word");
    }

    [Fact]
    public void Words_that_fit_on_one_line_are_returned_unjustified()
    {
        TextJustifier.Justify("Hi there", 20).Should().Equal("Hi there");
    }

    [Fact]
    public void Two_line_justification_distributes_uneven_padding_left_first()
    {
        TextJustifier.Justify("This is a test", 12).Should().Equal("This   is  a", "test");
    }

    [Fact]
    public void Three_line_justification_pads_each_non_last_line_to_width()
    {
        TextJustifier.Justify("This is a very long word", 10).Should().Equal("This  is a", "very  long", "word");
    }

    [Fact]
    public void Multiple_consecutive_whitespace_characters_collapse()
    {
        TextJustifier.Justify("This   is   a   test", 12).Should().Equal("This   is  a", "test");
    }

    [Fact]
    public void A_single_word_non_last_line_is_right_padded_to_width()
    {
        TextJustifier.Justify("longword ab", 9).Should().Equal("longword ", "ab");
    }

    [Fact]
    public void A_word_longer_than_width_stands_alone_and_may_exceed_width()
    {
        TextJustifier.Justify("verylongword hi", 5).Should().Equal("verylongword", "hi");
    }

    [Fact]
    public void Even_space_distribution_across_equal_gaps()
    {
        TextJustifier.Justify("alpha beta gamma delta epsilon", 25).Should().Equal("alpha  beta  gamma  delta", "epsilon");
    }
}
