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
        if (_p1Points >= 4 && _p1Points - _p2Points == 1) return "Advantage Player 1";
        if (_p2Points >= 4 && _p2Points - _p1Points == 1) return "Advantage Player 2";
        if (_p1Points >= 3 && _p1Points == _p2Points) return "Deuce";
        return $"{ScoreWord(_p1Points)}-{ScoreWord(_p2Points)}";
    }

    private static string ScoreWord(int points) => points switch
    {
        0 => "Love",
        1 => "15",
        2 => "30",
        _ => "40",
    };
}
