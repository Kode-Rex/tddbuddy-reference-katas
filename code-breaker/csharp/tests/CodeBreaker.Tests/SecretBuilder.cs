namespace CodeBreaker.Tests;

public class SecretBuilder
{
    private Peg[] _pegs = { Peg.One, Peg.Two, Peg.Three, Peg.Four };

    public SecretBuilder With(Peg a, Peg b, Peg c, Peg d)
    {
        _pegs = new[] { a, b, c, d };
        return this;
    }

    public Secret Build() => new(_pegs);
}
