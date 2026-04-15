namespace CodeBreaker;

public sealed class Guess
{
    public IReadOnlyList<Peg> Pegs { get; }

    public Guess(IReadOnlyList<Peg> pegs)
    {
        if (pegs.Count != CodeLength.Pegs)
            throw new ArgumentException(
                $"Guess must have exactly {CodeLength.Pegs} pegs.", nameof(pegs));
        Pegs = pegs;
    }
}
