namespace JellyVsTower;

public class Tower
{
    public string Id { get; }
    public ColorType Color { get; }
    public int Level { get; }

    public Tower(string id, ColorType color, int level)
    {
        if (level < 1 || level > 4) throw new InvalidLevelException(level);
        Id = id;
        Color = color;
        Level = level;
    }
}
