using FluentAssertions;

namespace MazeWalker.Tests;

public class PathPropertyTests
{
    private static (Maze maze, IReadOnlyList<Cell> path) SolveMaze(string layout)
    {
        var maze = new MazeBuilder().WithLayout(layout).Build();
        var walker = new WalkerBuilder().WithMaze(maze).Build();
        return (maze, walker.FindPath());
    }

    [Fact]
    public void The_path_starts_at_the_start_cell()
    {
        var (maze, path) = SolveMaze("S..E");

        path[0].Should().Be(maze.Start);
    }

    [Fact]
    public void The_path_ends_at_the_end_cell()
    {
        var (maze, path) = SolveMaze("S..E");

        path[^1].Should().Be(maze.End);
    }

    [Fact]
    public void Each_step_in_the_path_is_to_an_adjacent_cell()
    {
        var (_, path) = SolveMaze("S.#\n..#\n..E");

        for (var i = 1; i < path.Count; i++)
        {
            var dr = Math.Abs(path[i].Row - path[i - 1].Row);
            var dc = Math.Abs(path[i].Col - path[i - 1].Col);
            (dr + dc).Should().Be(1, "each step should move exactly one cell cardinally");
        }
    }

    [Fact]
    public void The_path_contains_no_walls()
    {
        var (maze, path) = SolveMaze("S.#\n..#\n..E");

        foreach (var cell in path)
        {
            maze.CellTypeAt(cell.Row, cell.Col).Should().NotBe(CellType.Wall);
        }
    }
}
