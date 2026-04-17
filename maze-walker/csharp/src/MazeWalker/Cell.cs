namespace MazeWalker;

/// <summary>
/// A coordinate in the maze. Value type with equality by (Row, Col).
/// </summary>
public readonly record struct Cell(int Row, int Col)
{
    /// <summary>
    /// Returns the four cardinal neighbors (up, down, left, right).
    /// </summary>
    public IEnumerable<Cell> CardinalNeighbors()
    {
        yield return new Cell(Row - 1, Col); // up
        yield return new Cell(Row + 1, Col); // down
        yield return new Cell(Row, Col - 1); // left
        yield return new Cell(Row, Col + 1); // right
    }
}
