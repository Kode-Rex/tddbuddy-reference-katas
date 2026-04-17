using FluentAssertions;

namespace MazeWalker.Tests;

public class MazeConstructionTests
{
    [Fact]
    public void A_maze_can_be_built_from_a_string_art_representation()
    {
        var maze = new MazeBuilder()
            .WithLayout("S.E")
            .Build();

        maze.Rows.Should().Be(1);
        maze.Cols.Should().Be(3);
        maze.Start.Should().Be(new Cell(0, 0));
        maze.End.Should().Be(new Cell(0, 2));
    }

    [Fact]
    public void A_maze_identifies_walls_correctly()
    {
        var maze = new MazeBuilder()
            .WithLayout("S#E")
            .Build();

        maze.CellTypeAt(0, 1).Should().Be(CellType.Wall);
    }

    [Fact]
    public void A_maze_without_a_start_cell_throws_NoStartCellException()
    {
        var act = () => new MazeBuilder()
            .WithLayout("..E")
            .Build();

        act.Should().Throw<NoStartCellException>()
            .WithMessage("Maze must have exactly one start cell.");
    }

    [Fact]
    public void A_maze_without_an_end_cell_throws_NoEndCellException()
    {
        var act = () => new MazeBuilder()
            .WithLayout("S..")
            .Build();

        act.Should().Throw<NoEndCellException>()
            .WithMessage("Maze must have exactly one end cell.");
    }

    [Fact]
    public void A_maze_with_multiple_start_cells_throws_MultipleStartCellsException()
    {
        var act = () => new MazeBuilder()
            .WithLayout("S.S\n..E")
            .Build();

        act.Should().Throw<MultipleStartCellsException>()
            .WithMessage("Maze must have exactly one start cell.");
    }

    [Fact]
    public void A_maze_with_multiple_end_cells_throws_MultipleEndCellsException()
    {
        var act = () => new MazeBuilder()
            .WithLayout("S.E\n..E")
            .Build();

        act.Should().Throw<MultipleEndCellsException>()
            .WithMessage("Maze must have exactly one end cell.");
    }
}
