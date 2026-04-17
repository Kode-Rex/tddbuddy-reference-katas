using FluentAssertions;

namespace MazeWalker.Tests;

public class ShortestPathTests
{
    [Fact]
    public void Walker_finds_the_shortest_path_around_a_wall()
    {
        var maze = new MazeBuilder()
            .WithLayout("S.\n#.\nE.")
            .Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().Equal(
            new Cell(0, 0), new Cell(0, 1),
            new Cell(1, 1),
            new Cell(2, 1), new Cell(2, 0));
    }

    [Fact]
    public void Walker_picks_the_shortest_of_two_possible_routes()
    {
        var maze = new MazeBuilder()
            .WithLayout("S.E\n...")
            .Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().HaveCount(3);
        path[0].Should().Be(new Cell(0, 0));
        path[2].Should().Be(new Cell(0, 2));
    }

    [Fact]
    public void Walker_navigates_a_winding_corridor()
    {
        var maze = new MazeBuilder()
            .WithLayout("S.#\n.#.\n..E")
            .Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().Equal(
            new Cell(0, 0),
            new Cell(1, 0),
            new Cell(2, 0), new Cell(2, 1), new Cell(2, 2));
    }
}
