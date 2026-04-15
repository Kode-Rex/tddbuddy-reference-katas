namespace PokerHands;

public class InvalidHandException : Exception
{
    public InvalidHandException(string message) : base(message) { }
}
