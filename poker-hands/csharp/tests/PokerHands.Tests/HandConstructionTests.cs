using FluentAssertions;
using Xunit;

namespace PokerHands.Tests;

public class HandConstructionTests
{
    [Fact]
    public void A_hand_with_five_cards_is_valid()
    {
        var aceOfSpades = new CardBuilder().OfRank(Rank.Ace).OfSuit(Suit.Spades).Build();
        var hand = new HandBuilder()
            .With(aceOfSpades)
            .With(new Card(Rank.King, Suit.Spades))
            .With(new Card(Rank.Queen, Suit.Spades))
            .With(new Card(Rank.Jack, Suit.Spades))
            .With(new Card(Rank.Ten, Suit.Spades))
            .Build();

        hand.Cards.Should().HaveCount(5);
        hand.Cards[0].Should().Be(aceOfSpades);
    }

    [Fact]
    public void A_hand_with_fewer_than_five_cards_is_rejected()
    {
        var act = () => HandBuilder.FromString("2H 3D 5S 9C");

        act.Should().Throw<InvalidHandException>()
            .WithMessage("A hand must have exactly 5 cards (got 4)");
    }

    [Fact]
    public void A_hand_with_more_than_five_cards_is_rejected()
    {
        var act = () => HandBuilder.FromString("2H 3D 5S 9C KD AH");

        act.Should().Throw<InvalidHandException>()
            .WithMessage("A hand must have exactly 5 cards (got 6)");
    }
}
