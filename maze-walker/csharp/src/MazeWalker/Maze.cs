namespace MazeWalker;

/// <summary>
/// Immutable rectangular grid of cells with exactly one start and one end.
/// </summary>
public class Maze
{
    private readonly CellType[,] _grid;

    public int Rows { get; }
    public int Cols { get; }
    public Cell Start { get; }
    public Cell End { get; }

    public Maze(CellType[,] grid, Cell start, Cell end)
    {
        _grid = grid;
        Rows = grid.GetLength(0);
        Cols = grid.GetLength(1);
        Start = start;
        End = end;
    }

    /// <summary>
    /// Returns the cell type at the given position, or null if out of bounds.
    /// </summary>
    public CellType? CellTypeAt(int row, int col)
    {
        if (row < 0 || row >= Rows || col < 0 || col >= Cols)
            return null;
        return _grid[row, col];
    }

    /// <summary>
    /// Returns true if the given cell is within bounds and not a wall.
    /// </summary>
    public bool IsWalkable(Cell cell)
    {
        var type = CellTypeAt(cell.Row, cell.Col);
        return type.HasValue && type.Value != CellType.Wall;
    }
}
