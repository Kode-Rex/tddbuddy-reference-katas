namespace PokerHands.Tests;

public class HandBuilder
{
    private readonly List<Card> _cards = new();

    public HandBuilder With(Card card) { _cards.Add(card); return this; }

    public Hand Build() => new(_cards);

    /// <summary>
    /// Shorthand factory: `HandBuilder.FromString("2H 3D 5S 9C KD")`. Delegates to `Hand.Parse`,
    /// which is the canonical poker-hands literal. Tests that care about hand-level evaluation
    /// use this form; tests that care about individual-card properties use CardBuilder.
    /// </summary>
    public static Hand FromString(string shorthand) => Hand.Parse(shorthand);
}
