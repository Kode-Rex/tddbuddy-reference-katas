namespace PokerHands.Tests;

public class CardBuilder
{
    private Rank _rank = Rank.Two;
    private Suit _suit = Suit.Clubs;

    public CardBuilder OfRank(Rank rank) { _rank = rank; return this; }
    public CardBuilder OfSuit(Suit suit) { _suit = suit; return this; }

    public Card Build() => new(_rank, _suit);
}
