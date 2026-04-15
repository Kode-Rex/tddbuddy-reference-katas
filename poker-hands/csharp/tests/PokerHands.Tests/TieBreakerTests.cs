using FluentAssertions;
using Xunit;

namespace PokerHands.Tests;

public class TieBreakerTests
{
    [Fact]
    public void Higher_high_card_wins_when_neither_hand_has_a_ranked_combination()
    {
        var aceHigh = HandBuilder.FromString("2C 3H 4S 8C AH");
        var kingHigh = HandBuilder.FromString("2H 3D 5S 9C KD");

        aceHigh.CompareTo(kingHigh).Should().Be(Compare.Player1Wins);
    }

    [Fact]
    public void Higher_pair_wins_when_both_hands_have_a_pair()
    {
        var kingsPair = HandBuilder.FromString("KH KD 5C 9S 3D");
        var twosPair = HandBuilder.FromString("2H 2D 5C 9S AD");

        kingsPair.CompareTo(twosPair).Should().Be(Compare.Player1Wins);
    }

    [Fact]
    public void Higher_kicker_wins_when_both_hands_have_the_same_pair()
    {
        var sevensWithAceKicker = HandBuilder.FromString("7H 7D AC 4S 2D");
        var sevensWithKingKicker = HandBuilder.FromString("7C 7S KH 4D 2H");

        sevensWithAceKicker.CompareTo(sevensWithKingKicker).Should().Be(Compare.Player1Wins);
    }

    [Fact]
    public void Higher_of_two_pairs_wins_when_both_hands_have_two_pair_with_the_same_lower_pair()
    {
        var acesAndTwos = HandBuilder.FromString("AH AD 2C 2S KD");
        var kingsAndTwos = HandBuilder.FromString("KH KC 2H 2D QS");

        acesAndTwos.CompareTo(kingsAndTwos).Should().Be(Compare.Player1Wins);
    }
}
