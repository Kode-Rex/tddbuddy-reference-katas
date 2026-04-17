namespace JellyVsTower.Tests;

public class JellyBuilder
{
    private string _id = "jelly-1";
    private ColorType _color = ColorType.Blue;
    private int _health = 10;

    public JellyBuilder WithId(string id) { _id = id; return this; }
    public JellyBuilder WithColor(ColorType color) { _color = color; return this; }
    public JellyBuilder WithHealth(int health) { _health = health; return this; }

    public Jelly Build() => new(_id, _color, _health);
}
