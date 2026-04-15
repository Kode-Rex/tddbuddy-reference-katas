using FluentAssertions;
using Xunit;

namespace PokerHands.Tests;

public class HandComparisonTests
{
    [Fact]
    public void Pair_beats_high_card()
    {
        var pair = HandBuilder.FromString("2H 2D 5C 9S KD");
        var highCard = HandBuilder.FromString("3H 5D 7C 9S AD");

        pair.CompareTo(highCard).Should().Be(Compare.Player1Wins);
    }

    [Fact]
    public void Flush_beats_straight()
    {
        var flush = HandBuilder.FromString("2H 5H 7H 9H KH");
        var straight = HandBuilder.FromString("2H 3D 4C 5S 6D");

        flush.CompareTo(straight).Should().Be(Compare.Player1Wins);
    }

    [Fact]
    public void Straight_flush_beats_four_of_a_kind()
    {
        var straightFlush = HandBuilder.FromString("2H 3H 4H 5H 6H");
        var fourOfAKind = HandBuilder.FromString("AH AD AC AS KD");

        straightFlush.CompareTo(fourOfAKind).Should().Be(Compare.Player1Wins);
    }
}
