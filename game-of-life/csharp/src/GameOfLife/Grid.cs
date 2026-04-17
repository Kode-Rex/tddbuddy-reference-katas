namespace GameOfLife;

/// <summary>
/// Immutable set of living cells on an unbounded infinite plane.
/// <c>Tick()</c> applies the four GoL rules and returns the next generation.
/// </summary>
public class Grid
{
    private readonly HashSet<Cell> _livingCells;

    public Grid(IEnumerable<Cell> livingCells)
    {
        _livingCells = new HashSet<Cell>(livingCells);
    }

    public bool IsAlive(int row, int col) => _livingCells.Contains(new Cell(row, col));

    public IReadOnlySet<Cell> LivingCells => _livingCells;

    public Grid Tick()
    {
        // Count neighbors for every cell adjacent to a living cell.
        var neighborCounts = new Dictionary<Cell, int>();

        foreach (var cell in _livingCells)
        {
            foreach (var neighbor in cell.Neighbors())
            {
                neighborCounts.TryGetValue(neighbor, out var count);
                neighborCounts[neighbor] = count + 1;
            }
        }

        var nextGeneration = new HashSet<Cell>();

        foreach (var (candidate, count) in neighborCounts)
        {
            var isAlive = _livingCells.Contains(candidate);

            // Survival: live cell with 2 or 3 neighbors.
            // Reproduction: dead cell with exactly 3 neighbors.
            if (count == 3 || (count == 2 && isAlive))
            {
                nextGeneration.Add(candidate);
            }
        }

        return new Grid(nextGeneration);
    }
}
