namespace BowlingGame;

public static class Game
{
    public static int Score(IReadOnlyList<int> rolls)
    {
        var total = 0;
        foreach (var pins in rolls)
        {
            total += pins;
        }
        return total;
    }
}
