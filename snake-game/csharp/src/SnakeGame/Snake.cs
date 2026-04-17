namespace SnakeGame;

/// <summary>
/// Ordered list of positions, head first. Immutable — movement returns a new Snake.
/// </summary>
public class Snake
{
    private readonly List<Position> _body;

    public Snake(Position head, Direction direction)
        : this(new List<Position> { head }, direction)
    {
    }

    public Snake(IReadOnlyList<Position> body, Direction direction)
    {
        _body = new List<Position>(body);
        Direction = direction;
    }

    public Position Head => _body[0];
    public Direction Direction { get; }
    public IReadOnlyList<Position> Body => _body;
    public int Length => _body.Count;

    public bool OccupiesPosition(Position position) => _body.Contains(position);

    public Snake Move(bool grow)
    {
        var newHead = Direction.Move(Head);
        var newBody = new List<Position> { newHead };
        newBody.AddRange(_body);

        if (!grow)
        {
            newBody.RemoveAt(newBody.Count - 1);
        }

        return new Snake(newBody, Direction);
    }

    public Snake ChangeDirection(Direction newDirection)
    {
        if (Direction.IsOpposite(newDirection))
        {
            return this;
        }

        return new Snake(_body, newDirection);
    }
}
