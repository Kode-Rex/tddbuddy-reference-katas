namespace MarsRover;

public class UnknownCommandException : ArgumentException
{
    public UnknownCommandException() : base(RoverMessages.UnknownCommand) { }
}
