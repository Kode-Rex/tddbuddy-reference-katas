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
        string p1Word;
        if (_p1Points == 0) p1Word = "Love";
        else if (_p1Points == 1) p1Word = "15";
        else if (_p1Points == 2) p1Word = "30";
        else p1Word = "40";

        string p2Word;
        if (_p2Points == 0) p2Word = "Love";
        else if (_p2Points == 1) p2Word = "15";
        else if (_p2Points == 2) p2Word = "30";
        else p2Word = "40";

        return $"{p1Word}-{p2Word}";
    }
}
