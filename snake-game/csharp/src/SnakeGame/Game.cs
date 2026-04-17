namespace SnakeGame;

/// <summary>
/// Aggregate: board + snake + food + score + state.
/// <c>Tick()</c> advances the game by one step.
/// </summary>
public class Game
{
    private readonly int _width;
    private readonly int _height;
    private readonly Func<IReadOnlyList<Position>, Position> _foodSpawner;

    public Game(int width, int height, Func<IReadOnlyList<Position>, Position> foodSpawner)
    {
        _width = width;
        _height = height;
        _foodSpawner = foodSpawner;
        Snake = new Snake(new Position(0, 0), Direction.Right);
        Score = 0;
        State = GameState.Playing;
        Food = foodSpawner(EmptyCells());
    }

    internal Game(int width, int height,
                  Func<IReadOnlyList<Position>, Position> foodSpawner,
                  Snake snake, Position food, int score, GameState state)
    {
        _width = width;
        _height = height;
        _foodSpawner = foodSpawner;
        Snake = snake;
        Food = food;
        Score = score;
        State = state;
    }

    public Snake Snake { get; private set; }
    public Position Food { get; private set; }
    public int Score { get; private set; }
    public GameState State { get; private set; }

    public void ChangeDirection(Direction newDirection)
    {
        if (State != GameState.Playing) return;
        Snake = Snake.ChangeDirection(newDirection);
    }

    public void Tick()
    {
        if (State != GameState.Playing) return;

        var newHead = Snake.Direction.Move(Snake.Head);

        if (IsOutOfBounds(newHead) || IsBodyCollision(newHead))
        {
            State = GameState.GameOver;
            return;
        }

        var eatsFood = newHead == Food;
        Snake = Snake.Move(eatsFood);

        if (eatsFood)
        {
            Score++;

            if (Snake.Length == _width * _height)
            {
                State = GameState.Won;
                return;
            }

            Food = _foodSpawner(EmptyCells());
        }
    }

    private bool IsOutOfBounds(Position position) =>
        position.X < 0 || position.X >= _width ||
        position.Y < 0 || position.Y >= _height;

    private bool IsBodyCollision(Position position) =>
        Snake.OccupiesPosition(position);

    private IReadOnlyList<Position> EmptyCells()
    {
        var empty = new List<Position>();
        for (var x = 0; x < _width; x++)
        for (var y = 0; y < _height; y++)
        {
            var pos = new Position(x, y);
            if (!Snake.OccupiesPosition(pos))
                empty.Add(pos);
        }
        return empty;
    }
}
