namespace SnakeGame;

/// <summary>
/// A coordinate on the board. Value type with equality by (X, Y).
/// </summary>
public readonly record struct Position(int X, int Y);
