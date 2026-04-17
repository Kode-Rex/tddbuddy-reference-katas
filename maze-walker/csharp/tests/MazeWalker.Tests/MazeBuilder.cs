namespace MazeWalker.Tests;

public class MazeBuilder
{
    private string _layout = "SE";

    public MazeBuilder WithLayout(string layout)
    {
        _layout = layout;
        return this;
    }

    public Maze Build()
    {
        var lines = _layout.Split('\n');
        var rows = lines.Length;
        var cols = lines.Max(l => l.Length);
        var grid = new CellType[rows, cols];

        Cell? start = null;
        Cell? end = null;
        var startCount = 0;
        var endCount = 0;

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < lines[r].Length; c++)
            {
                var ch = lines[r][c];
                switch (ch)
                {
                    case '#':
                        grid[r, c] = CellType.Wall;
                        break;
                    case 'S':
                        grid[r, c] = CellType.Start;
                        start = new Cell(r, c);
                        startCount++;
                        break;
                    case 'E':
                        grid[r, c] = CellType.End;
                        end = new Cell(r, c);
                        endCount++;
                        break;
                    default:
                        grid[r, c] = CellType.Open;
                        break;
                }
            }
        }

        if (startCount == 0)
            throw new NoStartCellException();

        if (startCount > 1)
            throw new MultipleStartCellsException();

        if (endCount == 0)
            throw new NoEndCellException();

        if (endCount > 1)
            throw new MultipleEndCellsException();

        return new Maze(grid, start!.Value, end!.Value);
    }
}
