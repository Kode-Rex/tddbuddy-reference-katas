namespace JellyVsTower;

public class InvalidHealthException : Exception
{
    public InvalidHealthException(int health)
        : base($"Health must be strictly positive, got {health}") { }
}
