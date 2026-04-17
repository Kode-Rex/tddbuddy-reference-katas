namespace GameOfLife.Tests;

public class GridBuilder
{
    private readonly HashSet<Cell> _livingCells = new();

    public GridBuilder WithLivingCellAt(int row, int col)
    {
        _livingCells.Add(new Cell(row, col));
        return this;
    }

    public GridBuilder WithLivingCellsAt(params (int Row, int Col)[] cells)
    {
        foreach (var (row, col) in cells)
            _livingCells.Add(new Cell(row, col));
        return this;
    }

    public Grid Build() => new(_livingCells);
}
