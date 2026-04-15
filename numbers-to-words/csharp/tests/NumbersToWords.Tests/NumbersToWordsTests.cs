using FluentAssertions;
using Xunit;

namespace NumbersToWords.Tests;

public class NumbersToWordsTests
{
    [Fact]
    public void Zero_is_spelled_out()
    {
        NumbersToWords.ToWords(0).Should().Be("zero");
    }

    [Fact]
    public void Five_is_spelled_out()
    {
        NumbersToWords.ToWords(5).Should().Be("five");
    }

    [Fact]
    public void Eight_is_spelled_out()
    {
        NumbersToWords.ToWords(8).Should().Be("eight");
    }

    [Fact]
    public void Ten_is_spelled_out()
    {
        NumbersToWords.ToWords(10).Should().Be("ten");
    }

    [Fact]
    public void Nineteen_is_a_single_word()
    {
        NumbersToWords.ToWords(19).Should().Be("nineteen");
    }

    [Fact]
    public void Twenty_is_spelled_out()
    {
        NumbersToWords.ToWords(20).Should().Be("twenty");
    }

    [Fact]
    public void Twenty_one_is_hyphenated()
    {
        NumbersToWords.ToWords(21).Should().Be("twenty-one");
    }

    [Fact]
    public void Seventy_seven_is_hyphenated()
    {
        NumbersToWords.ToWords(77).Should().Be("seventy-seven");
    }

    [Fact]
    public void Ninety_nine_is_hyphenated()
    {
        NumbersToWords.ToWords(99).Should().Be("ninety-nine");
    }

    [Fact]
    public void One_hundred_names_the_leading_one()
    {
        NumbersToWords.ToWords(100).Should().Be("one hundred");
    }

    [Fact]
    public void Three_hundred_three_has_no_and()
    {
        NumbersToWords.ToWords(303).Should().Be("three hundred three");
    }

    [Fact]
    public void Five_hundred_fifty_five_keeps_the_hyphen_in_the_tens()
    {
        NumbersToWords.ToWords(555).Should().Be("five hundred fifty-five");
    }

    [Fact]
    public void Two_thousand_omits_trailing_zeros()
    {
        NumbersToWords.ToWords(2000).Should().Be("two thousand");
    }

    [Fact]
    public void Two_thousand_four_hundred_skips_the_tens_and_ones()
    {
        NumbersToWords.ToWords(2400).Should().Be("two thousand four hundred");
    }

    [Fact]
    public void Three_thousand_four_hundred_sixty_six_is_fully_spelled_out()
    {
        NumbersToWords.ToWords(3466).Should().Be("three thousand four hundred sixty-six");
    }
}
