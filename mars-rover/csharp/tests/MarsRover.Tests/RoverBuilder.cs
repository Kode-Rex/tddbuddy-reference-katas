using static MarsRover.GridDefaults;

namespace MarsRover.Tests;

public class RoverBuilder
{
    private int _x = 0;
    private int _y = 0;
    private Direction _heading = Direction.North;
    private int _width = DefaultGridWidth;
    private int _height = DefaultGridHeight;
    private readonly HashSet<(int X, int Y)> _obstacles = new();

    public RoverBuilder At(int x, int y) { _x = x; _y = y; return this; }
    public RoverBuilder Facing(Direction heading) { _heading = heading; return this; }
    public RoverBuilder OnGrid(int width, int height) { _width = width; _height = height; return this; }
    public RoverBuilder WithObstacleAt(int x, int y) { _obstacles.Add((x, y)); return this; }

    public Rover Build() =>
        new(_x, _y, _heading, _width, _height, _obstacles, MovementStatus.Moving, null);
}
