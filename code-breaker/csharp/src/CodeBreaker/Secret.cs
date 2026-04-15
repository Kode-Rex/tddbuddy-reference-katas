namespace CodeBreaker;

public sealed class Secret
{
    public IReadOnlyList<Peg> Pegs { get; }

    public Secret(IReadOnlyList<Peg> pegs)
    {
        if (pegs.Count != CodeLength.Pegs)
            throw new ArgumentException(
                $"Secret must have exactly {CodeLength.Pegs} pegs.", nameof(pegs));
        Pegs = pegs;
    }

    public Feedback ScoreAgainst(Guess guess) => Feedback.Compute(this, guess);
}
