namespace RobotFactory;

public class PartNotAvailableException : Exception
{
    public PartNotAvailableException(string message) : base(message) { }
}
