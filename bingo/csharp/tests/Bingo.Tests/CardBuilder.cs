using static Bingo.CardDimensions;

namespace Bingo.Tests;

// Test-folder synthesiser. Places specific numbers at specific coordinates
// without enforcing the B/I/N/G/O column ranges — tests set up the card
// state they need, not the card a real generator would produce.
public class CardBuilder
{
    private readonly int?[,] _numbers = new int?[CardSize, CardSize];
    private readonly bool[,] _marks = new bool[CardSize, CardSize];

    public CardBuilder()
    {
        // Free space is blank and pre-marked on every card.
        _numbers[FreeRow, FreeColumn] = null;
        _marks[FreeRow, FreeColumn] = true;
    }

    public CardBuilder WithNumberAt(int row, int col, int number)
    {
        _numbers[row, col] = number;
        return this;
    }

    public CardBuilder WithMarkAt(int row, int col)
    {
        _marks[row, col] = true;
        return this;
    }

    public Card Build()
    {
        var numbers = (int?[,])_numbers.Clone();
        var marks = (bool[,])_marks.Clone();
        return new Card(numbers, marks);
    }
}
