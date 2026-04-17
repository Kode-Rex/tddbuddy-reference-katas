namespace JellyVsTower;

public class InvalidLevelException : Exception
{
    public InvalidLevelException(int level)
        : base($"Tower level must be between 1 and 4, got {level}") { }
}
