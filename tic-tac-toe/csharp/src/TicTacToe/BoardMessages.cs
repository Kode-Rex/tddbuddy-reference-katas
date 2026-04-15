namespace TicTacToe;

// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
public static class BoardMessages
{
    public const string CellOccupied = "cell already occupied";
    public const string OutOfBounds = "coordinates out of bounds";
    public const string GameOver = "game is already over";
}

public static class BoardDimensions
{
    public const int BoardSize = 3;
}
