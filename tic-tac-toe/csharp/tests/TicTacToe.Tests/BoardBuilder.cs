using static TicTacToe.BoardDimensions;

namespace TicTacToe.Tests;

public class BoardBuilder
{
    private readonly Cell[,] _cells = new Cell[BoardSize, BoardSize];

    public BoardBuilder WithXAt(int row, int col) { _cells[row, col] = Cell.X; return this; }
    public BoardBuilder WithOAt(int row, int col) { _cells[row, col] = Cell.O; return this; }

    public Board Build()
    {
        var copy = (Cell[,])_cells.Clone();
        return Board.FromCells(copy);
    }
}
