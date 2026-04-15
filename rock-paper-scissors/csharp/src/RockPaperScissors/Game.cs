namespace RockPaperScissors;

public static class Game
{
    public static Outcome Decide(Play p1, Play p2)
    {
        if (p1 == p2) return Outcome.Draw;
        return Beats(p1, p2) ? Outcome.Win : Outcome.Lose;
    }

    private static bool Beats(Play a, Play b) =>
        (a, b) switch
        {
            (Play.Rock, Play.Scissors) => true,
            (Play.Scissors, Play.Paper) => true,
            (Play.Paper, Play.Rock) => true,
            _ => false,
        };
}
