namespace CodeBreaker;

public sealed record Feedback(int ExactMatches, int ColorMatches)
{
    public bool IsWon => ExactMatches == CodeLength.Pegs;

    public string Render() =>
        new string('+', ExactMatches) + new string('-', ColorMatches);

    public override string ToString() => Render();

    // Two-pass scoring: exact matches consume positions first, then the
    // remaining secret pegs are matched against the remaining guess pegs
    // by value, respecting multiplicity.
    public static Feedback Compute(Secret secret, Guess guess)
    {
        var secretPegs = secret.Pegs;
        var guessPegs = guess.Pegs;

        var exactMatches = 0;
        var unmatchedSecret = new List<Peg>();
        var unmatchedGuess = new List<Peg>();

        for (var i = 0; i < CodeLength.Pegs; i++)
        {
            if (secretPegs[i] == guessPegs[i])
            {
                exactMatches++;
            }
            else
            {
                unmatchedSecret.Add(secretPegs[i]);
                unmatchedGuess.Add(guessPegs[i]);
            }
        }

        var colorMatches = 0;
        foreach (var peg in unmatchedGuess)
        {
            var index = unmatchedSecret.IndexOf(peg);
            if (index >= 0)
            {
                unmatchedSecret.RemoveAt(index);
                colorMatches++;
            }
        }

        return new Feedback(exactMatches, colorMatches);
    }
}
