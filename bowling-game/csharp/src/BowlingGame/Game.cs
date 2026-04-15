namespace BowlingGame;

public static class Game
{
    public static int Score(IReadOnlyList<int> rolls)
    {
        var total = 0;
        var i = 0;
        while (i < rolls.Count - 1)
        {
            if (rolls[i] + rolls[i + 1] == 10)
            {
                total += 10 + rolls[i + 2];
            }
            else
            {
                total += rolls[i] + rolls[i + 1];
            }
            i += 2;
        }
        return total;
    }
}
