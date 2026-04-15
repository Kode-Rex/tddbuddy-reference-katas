namespace TicTacToe;

public class CellOccupiedException : InvalidOperationException
{
    public CellOccupiedException() : base(BoardMessages.CellOccupied) { }
}

public class OutOfBoundsException : ArgumentOutOfRangeException
{
    public OutOfBoundsException() : base(null, BoardMessages.OutOfBounds) { }
}

public class GameOverException : InvalidOperationException
{
    public GameOverException() : base(BoardMessages.GameOver) { }
}
