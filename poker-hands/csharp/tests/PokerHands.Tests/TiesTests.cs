using FluentAssertions;
using Xunit;

namespace PokerHands.Tests;

public class TiesTests
{
    [Fact]
    public void Two_hands_with_identical_ranks_and_kickers_tie()
    {
        var player1 = HandBuilder.FromString("2H 3D 5S 9C KD");
        var player2 = HandBuilder.FromString("2D 3H 5C 9S KH");

        player1.CompareTo(player2).Should().Be(Compare.Tie);
    }
}
