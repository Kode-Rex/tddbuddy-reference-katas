using FluentAssertions;
using Xunit;

namespace WordWrap.Tests;

public class WordWrapTests
{
    [Fact]
    public void An_empty_string_returns_an_empty_string()
    {
        WordWrap.Wrap("", 10).Should().Be("");
    }

    [Fact]
    public void A_whitespace_only_string_returns_an_empty_string()
    {
        WordWrap.Wrap("   \t  ", 10).Should().Be("");
    }

    [Fact]
    public void A_single_word_shorter_than_width_is_returned_unchanged()
    {
        WordWrap.Wrap("Hello", 10).Should().Be("Hello");
    }

    [Fact]
    public void A_single_word_equal_to_width_is_returned_unchanged()
    {
        WordWrap.Wrap("Hello", 5).Should().Be("Hello");
    }

    [Fact]
    public void Two_words_that_fit_on_one_line_are_returned_unchanged()
    {
        WordWrap.Wrap("Hello World", 20).Should().Be("Hello World");
    }

    [Fact]
    public void Two_words_break_at_the_word_boundary_when_they_do_not_fit()
    {
        WordWrap.Wrap("Hello World", 5).Should().Be("Hello\nWorld");
    }

    [Fact]
    public void Two_words_break_at_the_word_boundary_when_the_gap_pushes_past_width()
    {
        WordWrap.Wrap("Hello World", 7).Should().Be("Hello\nWorld");
    }

    [Fact]
    public void Three_words_across_three_lines_when_each_line_fits_one_word()
    {
        WordWrap.Wrap("Hello wonderful World", 9).Should().Be("Hello\nwonderful\nWorld");
    }

    [Fact]
    public void An_oversize_single_word_splits_hard_at_width()
    {
        WordWrap.Wrap("Supercalifragilisticexpialidocious", 10)
            .Should().Be("Supercalif\nragilistic\nexpialidoc\nious");
    }

    [Fact]
    public void An_oversize_word_remainder_joins_the_next_word_when_it_fits()
    {
        WordWrap.Wrap("abcdefghij kl", 5).Should().Be("abcde\nfghij\nkl");
    }

    [Fact]
    public void Multiple_consecutive_whitespace_characters_collapse()
    {
        WordWrap.Wrap("Hello   World", 5).Should().Be("Hello\nWorld");
    }

    [Fact]
    public void Multi_word_multi_line_greedy_packing()
    {
        WordWrap.Wrap("The quick brown fox jumps over the lazy dog", 10)
            .Should().Be("The quick\nbrown fox\njumps over\nthe lazy\ndog");
    }
}
