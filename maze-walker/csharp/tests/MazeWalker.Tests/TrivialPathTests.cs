using FluentAssertions;

namespace MazeWalker.Tests;

public class TrivialPathTests
{
    [Fact]
    public void Start_adjacent_to_end_finds_a_two_cell_path()
    {
        var maze = new MazeBuilder().WithLayout("SE").Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().Equal(new Cell(0, 0), new Cell(0, 1));
    }

    [Fact]
    public void A_straight_horizontal_corridor_finds_the_path()
    {
        var maze = new MazeBuilder().WithLayout("S..E").Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().Equal(
            new Cell(0, 0), new Cell(0, 1), new Cell(0, 2), new Cell(0, 3));
    }

    [Fact]
    public void A_straight_vertical_corridor_finds_the_path()
    {
        var maze = new MazeBuilder().WithLayout("S\n.\nE").Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();

        var path = walker.FindPath();

        path.Should().Equal(
            new Cell(0, 0), new Cell(1, 0), new Cell(2, 0));
    }
}
