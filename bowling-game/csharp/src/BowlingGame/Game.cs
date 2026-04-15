namespace BowlingGame;

public static class Game
{
    public static int Score(IReadOnlyList<int> rolls)
    {
        var total = 0;
        var rollIndex = 0;
        for (var frameIndex = 0; frameIndex < 10; frameIndex++)
        {
            if (rolls[rollIndex] == 10)
            {
                total += 10 + rolls[rollIndex + 1] + rolls[rollIndex + 2];
                rollIndex += 1;
            }
            else if (rolls[rollIndex] + rolls[rollIndex + 1] == 10)
            {
                total += 10 + rolls[rollIndex + 2];
                rollIndex += 2;
            }
            else
            {
                total += rolls[rollIndex] + rolls[rollIndex + 1];
                rollIndex += 2;
            }
        }
        return total;
    }
}
