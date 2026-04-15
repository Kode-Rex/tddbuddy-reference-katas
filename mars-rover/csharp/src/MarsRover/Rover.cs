using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MarsRover.Tests")]

namespace MarsRover;

public sealed class Rover
{
    public int X { get; }
    public int Y { get; }
    public Direction Heading { get; }
    public int GridWidth { get; }
    public int GridHeight { get; }
    public MovementStatus Status { get; }
    public (int X, int Y)? LastObstacle { get; }

    private readonly HashSet<(int X, int Y)> _obstacles;

    internal Rover(
        int x, int y,
        Direction heading,
        int gridWidth, int gridHeight,
        IReadOnlySet<(int X, int Y)> obstacles,
        MovementStatus status,
        (int X, int Y)? lastObstacle)
    {
        X = x; Y = y;
        Heading = heading;
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        _obstacles = new HashSet<(int, int)>(obstacles);
        Status = status;
        LastObstacle = lastObstacle;
    }

    public (int X, int Y) Position => (X, Y);

    public IReadOnlySet<(int X, int Y)> Obstacles => _obstacles;

    public Rover Execute(string commands)
    {
        var x = X;
        var y = Y;
        var heading = Heading;
        var status = Status;
        var lastObstacle = LastObstacle;

        foreach (var ch in commands)
        {
            var cmd = ParseCommand(ch);

            if (status == MovementStatus.Blocked) break;

            switch (cmd)
            {
                case Command.Left:
                    heading = RotateLeft(heading);
                    break;
                case Command.Right:
                    heading = RotateRight(heading);
                    break;
                case Command.Forward:
                case Command.Backward:
                    var (nx, ny) = Step(x, y, heading, cmd == Command.Forward ? 1 : -1);
                    if (_obstacles.Contains((nx, ny)))
                    {
                        status = MovementStatus.Blocked;
                        lastObstacle = (nx, ny);
                    }
                    else
                    {
                        x = nx; y = ny;
                    }
                    break;
            }
        }

        return new Rover(x, y, heading, GridWidth, GridHeight, _obstacles, status, lastObstacle);
    }

    private (int X, int Y) Step(int x, int y, Direction heading, int sign)
    {
        var (dx, dy) = heading switch
        {
            Direction.North => (0, -1),
            Direction.South => (0, 1),
            Direction.East  => (1, 0),
            Direction.West  => (-1, 0),
            _ => (0, 0),
        };
        var nx = Mod(x + dx * sign, GridWidth);
        var ny = Mod(y + dy * sign, GridHeight);
        return (nx, ny);
    }

    private static int Mod(int value, int modulus) => ((value % modulus) + modulus) % modulus;

    private static Direction RotateLeft(Direction d) => d switch
    {
        Direction.North => Direction.West,
        Direction.West  => Direction.South,
        Direction.South => Direction.East,
        Direction.East  => Direction.North,
        _ => d,
    };

    private static Direction RotateRight(Direction d) => d switch
    {
        Direction.North => Direction.East,
        Direction.East  => Direction.South,
        Direction.South => Direction.West,
        Direction.West  => Direction.North,
        _ => d,
    };

    private static Command ParseCommand(char ch) => ch switch
    {
        'F' => Command.Forward,
        'B' => Command.Backward,
        'L' => Command.Left,
        'R' => Command.Right,
        _ => throw new UnknownCommandException(),
    };
}
