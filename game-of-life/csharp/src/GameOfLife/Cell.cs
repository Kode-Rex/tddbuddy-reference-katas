namespace GameOfLife;

/// <summary>
/// A coordinate on the infinite plane. Value type with equality by (Row, Col).
/// </summary>
public readonly record struct Cell(int Row, int Col)
{
    public IEnumerable<Cell> Neighbors()
    {
        for (var dr = -1; dr <= 1; dr++)
        for (var dc = -1; dc <= 1; dc++)
        {
            if (dr == 0 && dc == 0) continue;
            yield return new Cell(Row + dr, Col + dc);
        }
    }
}
