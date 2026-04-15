namespace CodeBreaker.Tests;

public class GuessBuilder
{
    private Peg[] _pegs = { Peg.One, Peg.Two, Peg.Three, Peg.Four };

    public GuessBuilder With(Peg a, Peg b, Peg c, Peg d)
    {
        _pegs = new[] { a, b, c, d };
        return this;
    }

    public Guess Build() => new(_pegs);
}
