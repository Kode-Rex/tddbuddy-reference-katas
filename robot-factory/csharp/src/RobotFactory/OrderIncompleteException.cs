namespace RobotFactory;

public class OrderIncompleteException : Exception
{
    public OrderIncompleteException(string message) : base(message) { }
}
