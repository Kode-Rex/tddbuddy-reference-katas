namespace SnakeGame.Tests;

public class SnakeBuilder
{
    private Direction _direction = Direction.Right;
    private readonly List<Position> _body = new() { new Position(0, 0) };

    public SnakeBuilder At(int x, int y)
    {
        _body.Clear();
        _body.Add(new Position(x, y));
        return this;
    }

    public SnakeBuilder WithBodyAt(params (int X, int Y)[] positions)
    {
        _body.Clear();
        foreach (var (x, y) in positions)
            _body.Add(new Position(x, y));
        return this;
    }

    public SnakeBuilder MovingUp() { _direction = Direction.Up; return this; }
    public SnakeBuilder MovingDown() { _direction = Direction.Down; return this; }
    public SnakeBuilder MovingLeft() { _direction = Direction.Left; return this; }
    public SnakeBuilder MovingRight() { _direction = Direction.Right; return this; }

    public Snake Build() => new(_body, _direction);
}
