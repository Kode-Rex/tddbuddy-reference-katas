namespace SnakeGame.Tests;

public class BoardBuilder
{
    private int _width = 5;
    private int _height = 5;
    private Snake? _snake;
    private Position? _food;
    private Func<IReadOnlyList<Position>, Position>? _foodSpawner;

    public BoardBuilder WithSize(int width, int height)
    {
        _width = width;
        _height = height;
        return this;
    }

    public BoardBuilder WithSnake(Snake snake)
    {
        _snake = snake;
        return this;
    }

    public BoardBuilder WithFoodAt(int x, int y)
    {
        _food = new Position(x, y);
        return this;
    }

    public BoardBuilder WithFoodSpawner(Func<IReadOnlyList<Position>, Position> spawner)
    {
        _foodSpawner = spawner;
        return this;
    }

    public Game Build()
    {
        var snake = _snake ?? new Snake(new Position(0, 0), Direction.Right);
        var food = _food ?? new Position(_width - 1, _height - 1);
        var spawner = _foodSpawner ?? (emptyCells => emptyCells[0]);

        return new Game(_width, _height, spawner, snake, food, 0, GameState.Playing);
    }
}
