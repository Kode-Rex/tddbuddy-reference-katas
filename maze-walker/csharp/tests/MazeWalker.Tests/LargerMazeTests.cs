using FluentAssertions;

namespace MazeWalker.Tests;

public class LargerMazeTests
{
    [Fact]
    public void Walker_solves_a_5x5_maze()
    {
        var maze = new MazeBuilder()
            .WithLayout("S.#..\n.#...\n...#.\n.#..E\n.....")
            .Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().NotBeEmpty();
        path[0].Should().Be(maze.Start);
        path[^1].Should().Be(maze.End);
    }

    [Fact]
    public void Walker_solves_a_maze_requiring_exploration()
    {
        var maze = new MazeBuilder()
            .WithLayout("S..#.\n##...\n...#E")
            .Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().NotBeEmpty();
        path[0].Should().Be(maze.Start);
        path[^1].Should().Be(maze.End);
    }
}
