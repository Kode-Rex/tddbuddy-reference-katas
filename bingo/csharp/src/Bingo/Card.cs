using System.Runtime.CompilerServices;
using static Bingo.CardDimensions;

[assembly: InternalsVisibleTo("Bingo.Tests")]

namespace Bingo;

public sealed class Card
{
    // Cells hold either a number (1..75) or null for the free space.
    private readonly int?[,] _numbers;
    private readonly bool[,] _marks;

    internal Card(int?[,] numbers, bool[,] marks)
    {
        _numbers = numbers;
        _marks = marks;
    }

    public int? NumberAt(int row, int col) => _numbers[row, col];

    public bool IsMarked(int row, int col) => _marks[row, col];

    public void Mark(int number)
    {
        if (number < MinNumber || number > MaxNumber)
            throw new NumberOutOfRangeException();

        for (var r = 0; r < CardSize; r++)
            for (var c = 0; c < CardSize; c++)
                if (_numbers[r, c] == number)
                    _marks[r, c] = true;
    }

    public bool HasWon() => WinningPattern().Kind != WinPatternKind.None;

    public WinPattern WinningPattern()
    {
        for (var r = 0; r < CardSize; r++)
            if (RowMarked(r)) return WinPattern.Row(r);

        for (var c = 0; c < CardSize; c++)
            if (ColumnMarked(c)) return WinPattern.Column(c);

        if (MainDiagonalMarked()) return WinPattern.DiagonalMain;
        if (AntiDiagonalMarked()) return WinPattern.DiagonalAnti;

        return WinPattern.None;
    }

    private bool RowMarked(int r)
    {
        for (var c = 0; c < CardSize; c++)
            if (!_marks[r, c]) return false;
        return true;
    }

    private bool ColumnMarked(int c)
    {
        for (var r = 0; r < CardSize; r++)
            if (!_marks[r, c]) return false;
        return true;
    }

    private bool MainDiagonalMarked()
    {
        for (var i = 0; i < CardSize; i++)
            if (!_marks[i, i]) return false;
        return true;
    }

    private bool AntiDiagonalMarked()
    {
        for (var i = 0; i < CardSize; i++)
            if (!_marks[i, CardSize - 1 - i]) return false;
        return true;
    }
}
