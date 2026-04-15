namespace VideoClubRental.Tests;

public class TitleBuilder
{
    private string _name = "The Godfather";
    private int _copies = 3;

    public TitleBuilder Named(string name) { _name = name; return this; }
    public TitleBuilder WithCopies(int copies) { _copies = copies; return this; }

    public Title Build() => new(_name, _copies);
}
