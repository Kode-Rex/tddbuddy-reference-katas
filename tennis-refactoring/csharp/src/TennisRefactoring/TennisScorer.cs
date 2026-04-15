namespace TennisRefactoring;

// Clean tennis scorer — characterization-equivalent to the legacy getScore
// function from the kata brief. Three named branches (equal / endgame /
// in-play) replace the original's nested if/else over raw point counts;
// Score.Name lifts the magic ["Love","Fifteen","Thirty","Forty"] array
// into a named state. See ../../SCENARIOS.md for the contract.
public static class TennisScorer
{
    // The first point count at which the game enters the endgame —
    // Advantage or Win. Below this threshold, unequal scores render as
    // "<Point>-<Point>" (in-play); at or above, the leader's name appears.
    private const int EndgameThreshold = 4;

    // The first point count at which equal scores collapse to "Deuce"
    // rather than "<Point>-All". Legacy: `p1Score >= 3` when p1Score == p2Score.
    private const int DeuceThreshold = 3;

    public static string GetScore(int p1Score, int p2Score, string p1Name, string p2Name)
    {
        if (p1Score == p2Score) return EqualScore(p1Score);
        if (p1Score >= EndgameThreshold || p2Score >= EndgameThreshold)
            return EndgameScore(p1Score, p2Score, p1Name, p2Name);
        return InPlayScore(p1Score, p2Score);
    }

    private static string EqualScore(int score)
    {
        if (score >= DeuceThreshold) return "Deuce";
        return $"{Score.Name(score)}-All";
    }

    private static string EndgameScore(int p1Score, int p2Score, string p1Name, string p2Name)
    {
        int diff = p1Score - p2Score;
        if (diff == 1) return $"Advantage {p1Name}";
        if (diff == -1) return $"Advantage {p2Name}";
        if (diff >= 2) return $"Win for {p1Name}";
        return $"Win for {p2Name}";
    }

    private static string InPlayScore(int p1Score, int p2Score)
        => $"{Score.Name(p1Score)}-{Score.Name(p2Score)}";
}

// The point-name lookup lifted out of the legacy's inline string array.
// Named because the words are the spec — "Love" / "Fifteen" / "Thirty" /
// "Forty", never the numerals.
internal static class Score
{
    private static readonly string[] Names = { "Love", "Fifteen", "Thirty", "Forty" };

    public static string Name(int points) => Names[points];
}
