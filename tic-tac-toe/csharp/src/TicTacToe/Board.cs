using System.Runtime.CompilerServices;
using static TicTacToe.BoardDimensions;

[assembly: InternalsVisibleTo("TicTacToe.Tests")]

namespace TicTacToe;

public sealed class Board
{
    private static readonly (int, int)[][] WinningLines =
    {
        new[] { (0, 0), (0, 1), (0, 2) },
        new[] { (1, 0), (1, 1), (1, 2) },
        new[] { (2, 0), (2, 1), (2, 2) },
        new[] { (0, 0), (1, 0), (2, 0) },
        new[] { (0, 1), (1, 1), (2, 1) },
        new[] { (0, 2), (1, 2), (2, 2) },
        new[] { (0, 0), (1, 1), (2, 2) },
        new[] { (0, 2), (1, 1), (2, 0) },
    };

    private readonly Cell[,] _cells;

    public Board() : this(new Cell[BoardSize, BoardSize]) { }

    private Board(Cell[,] cells)
    {
        _cells = cells;
    }

    public Cell CellAt(int row, int col)
    {
        RequireInBounds(row, col);
        return _cells[row, col];
    }

    public Cell CurrentTurn()
    {
        var xs = CountOf(Cell.X);
        var os = CountOf(Cell.O);
        return xs == os ? Cell.X : Cell.O;
    }

    public Outcome Outcome()
    {
        foreach (var line in WinningLines)
        {
            var a = _cells[line[0].Item1, line[0].Item2];
            if (a == Cell.Empty) continue;
            if (a == _cells[line[1].Item1, line[1].Item2] &&
                a == _cells[line[2].Item1, line[2].Item2])
            {
                return a == Cell.X ? TicTacToe.Outcome.XWins : TicTacToe.Outcome.OWins;
            }
        }

        return CountOf(Cell.Empty) == 0
            ? TicTacToe.Outcome.Draw
            : TicTacToe.Outcome.InProgress;
    }

    public Board Place(int row, int col)
    {
        if (Outcome() != TicTacToe.Outcome.InProgress)
            throw new GameOverException();
        RequireInBounds(row, col);
        if (_cells[row, col] != Cell.Empty)
            throw new CellOccupiedException();

        var next = (Cell[,])_cells.Clone();
        next[row, col] = CurrentTurn();
        return new Board(next);
    }

    internal static Board FromCells(Cell[,] cells) => new(cells);

    private static void RequireInBounds(int row, int col)
    {
        if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
            throw new OutOfBoundsException();
    }

    private int CountOf(Cell mark)
    {
        var count = 0;
        for (var r = 0; r < BoardSize; r++)
            for (var c = 0; c < BoardSize; c++)
                if (_cells[r, c] == mark) count++;
        return count;
    }
}
