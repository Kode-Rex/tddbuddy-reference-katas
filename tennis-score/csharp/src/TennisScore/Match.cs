namespace TennisScore;

public class Match
{
    private int _p1Points;

    public void PointWonBy(int player)
    {
        if (player == 1) _p1Points++;
    }

    public string Score()
    {
        if (_p1Points == 1) return "15-Love";
        return "Love-Love";
    }
}
