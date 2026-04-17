namespace MazeWalker;

public class NoStartCellException : Exception
{
    public NoStartCellException() : base("Maze must have exactly one start cell.") { }
}

public class NoEndCellException : Exception
{
    public NoEndCellException() : base("Maze must have exactly one end cell.") { }
}

public class MultipleStartCellsException : Exception
{
    public MultipleStartCellsException() : base("Maze must have exactly one start cell.") { }
}

public class MultipleEndCellsException : Exception
{
    public MultipleEndCellsException() : base("Maze must have exactly one end cell.") { }
}
