namespace SnakeGame;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static bool IsOpposite(this Direction self, Direction other) =>
        (self == Direction.Up && other == Direction.Down) ||
        (self == Direction.Down && other == Direction.Up) ||
        (self == Direction.Left && other == Direction.Right) ||
        (self == Direction.Right && other == Direction.Left);

    public static Position Move(this Direction direction, Position position) =>
        direction switch
        {
            Direction.Up => position with { Y = position.Y - 1 },
            Direction.Down => position with { Y = position.Y + 1 },
            Direction.Left => position with { X = position.X - 1 },
            Direction.Right => position with { X = position.X + 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
}
