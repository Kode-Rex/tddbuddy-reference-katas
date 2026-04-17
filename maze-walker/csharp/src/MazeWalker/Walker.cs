namespace MazeWalker;

/// <summary>
/// Navigates a maze using BFS to find the shortest path from start to end.
/// </summary>
public class Walker
{
    private readonly Maze _maze;

    public Walker(Maze maze)
    {
        _maze = maze;
    }

    /// <summary>
    /// Finds the shortest path from start to end using BFS.
    /// Returns an empty list when no path exists.
    /// </summary>
    public IReadOnlyList<Cell> FindPath()
    {
        var start = _maze.Start;
        var end = _maze.End;

        var visited = new HashSet<Cell> { start };
        var queue = new Queue<Cell>();
        queue.Enqueue(start);

        var cameFrom = new Dictionary<Cell, Cell>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == end)
                return ReconstructPath(cameFrom, start, end);

            foreach (var neighbor in current.CardinalNeighbors())
            {
                if (!_maze.IsWalkable(neighbor) || visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                cameFrom[neighbor] = current;
                queue.Enqueue(neighbor);
            }
        }

        return Array.Empty<Cell>();
    }

    private static List<Cell> ReconstructPath(
        Dictionary<Cell, Cell> cameFrom, Cell start, Cell end)
    {
        var path = new List<Cell>();
        var current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }
}
