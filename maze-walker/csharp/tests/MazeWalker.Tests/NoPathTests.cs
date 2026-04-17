using FluentAssertions;

namespace MazeWalker.Tests;

public class NoPathTests
{
    [Fact]
    public void A_wall_between_start_and_end_returns_an_empty_path()
    {
        var maze = new MazeBuilder().WithLayout("S#E").Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().BeEmpty();
    }

    [Fact]
    public void A_maze_with_no_reachable_end_returns_an_empty_path()
    {
        var maze = new MazeBuilder()
            .WithLayout("S.#\n.##\n##E")
            .Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().BeEmpty();
    }
}
