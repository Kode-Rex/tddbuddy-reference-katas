namespace TennisScore;

public class Match
{
    private int _p1Points;
    private int _p2Points;
    private int _p1Games;
    private int _p2Games;
    private int? _gameJustWonBy;
    private int? _setJustWonBy;

    public void PointWonBy(int player)
    {
        _gameJustWonBy = null;
        _setJustWonBy = null;

        if (player == 1) _p1Points++;
        else _p2Points++;

        var (p1, p2) = ScoreStates();
        if (p1 == ScoreState.Game) { _p1Games++; _p1Points = 0; _p2Points = 0; _gameJustWonBy = 1; }
        else if (p2 == ScoreState.Game) { _p2Games++; _p1Points = 0; _p2Points = 0; _gameJustWonBy = 2; }

        if (_p1Games >= 6 && _p1Games - _p2Games >= 2) { _setJustWonBy = 1; _gameJustWonBy = null; }
        else if (_p2Games >= 6 && _p2Games - _p1Games >= 2) { _setJustWonBy = 2; _gameJustWonBy = null; }
    }

    public string Score()
    {
        if (_setJustWonBy == 1) return "Set Player 1";
        if (_setJustWonBy == 2) return "Set Player 2";
        if (_gameJustWonBy == 1) return "Game Player 1";
        if (_gameJustWonBy == 2) return "Game Player 2";

        var (p1, p2) = ScoreStates();
        if (p1 == ScoreState.Advantage) return "Advantage Player 1";
        if (p2 == ScoreState.Advantage) return "Advantage Player 2";
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
