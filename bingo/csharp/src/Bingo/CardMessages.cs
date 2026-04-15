namespace Bingo;

// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
public static class CardMessages
{
    public const string NumberOutOfRange = "called number must be between 1 and 75";
}

public static class CardDimensions
{
    public const int CardSize = 5;
    public const int FreeRow = 2;
    public const int FreeColumn = 2;
    public const int MinNumber = 1;
    public const int MaxNumber = 75;
    public const int NumbersPerColumn = 15;
}
