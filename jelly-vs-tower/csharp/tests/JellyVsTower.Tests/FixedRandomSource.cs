namespace JellyVsTower.Tests;

public class FixedRandomSource : IRandomSource
{
    private readonly int _value;

    public FixedRandomSource(int value) => _value = value;

    public int Next(int minInclusive, int maxInclusive) => _value;
}
