namespace JellyVsTower.Tests;

public class TowerBuilder
{
    private string _id = "tower-1";
    private ColorType _color = ColorType.Blue;
    private int _level = 1;

    public TowerBuilder WithId(string id) { _id = id; return this; }
    public TowerBuilder WithColor(ColorType color) { _color = color; return this; }
    public TowerBuilder WithLevel(int level) { _level = level; return this; }

    public Tower Build() => new(_id, _color, _level);
}
