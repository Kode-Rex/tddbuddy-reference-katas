namespace TennisScore;

public class Match
{
    private int _p1Points;
    private int _p2Points;

    public void PointWonBy(int player)
    {
        if (player == 1) _p1Points++;
        else _p2Points++;
    }

    public string Score()
    {
        var (p1, p2) = ScoreStates();

        if (p1 == ScoreState.Advantage) return "Advantage Player 1";
        if (p2 == ScoreState.Advantage) return "Advantage Player 2";
        if (p1 == ScoreState.Game) return "Game Player 1";
        if (p2 == ScoreState.Game) return "Game Player 2";
        if (p1 == ScoreState.Deuce) return "Deuce";
        return $"{Word(p1)}-{Word(p2)}";
    }

    private (ScoreState, ScoreState) ScoreStates()
    {
        if (_p1Points >= 3 && _p2Points >= 3)
        {
            if (_p1Points == _p2Points) return (ScoreState.Deuce, ScoreState.Deuce);
            if (_p1Points - _p2Points == 1) return (ScoreState.Advantage, ScoreState.Forty);
            if (_p2Points - _p1Points == 1) return (ScoreState.Forty, ScoreState.Advantage);
            if (_p1Points > _p2Points) return (ScoreState.Game, ScoreState.Forty);
            return (ScoreState.Forty, ScoreState.Game);
        }
        if (_p1Points >= 4) return (ScoreState.Game, PointsToScore(_p2Points));
        if (_p2Points >= 4) return (PointsToScore(_p1Points), ScoreState.Game);
        return (PointsToScore(_p1Points), PointsToScore(_p2Points));
    }

    private static ScoreState PointsToScore(int points) => points switch
    {
        0 => ScoreState.Love,
        1 => ScoreState.Fifteen,
        2 => ScoreState.Thirty,
        _ => ScoreState.Forty,
    };

    private static string Word(ScoreState s) => s switch
    {
        ScoreState.Love => "Love",
        ScoreState.Fifteen => "15",
        ScoreState.Thirty => "30",
        ScoreState.Forty => "40",
        _ => s.ToString(),
    };
}
