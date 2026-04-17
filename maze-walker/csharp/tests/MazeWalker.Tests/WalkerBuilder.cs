namespace MazeWalker.Tests;

public class WalkerBuilder
{
    private Maze _maze = new MazeBuilder().Build();

    public WalkerBuilder WithMaze(Maze maze)
    {
        _maze = maze;
        return this;
    }

    public Walker Build() => new(_maze);
}
