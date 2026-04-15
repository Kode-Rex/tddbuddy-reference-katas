using FluentAssertions;

namespace BowlingGame.Tests;

public class GameTests
{
    [Fact]
    public void Gutter_game_scores_zero()
    {
        Game.Score(new int[20]).Should().Be(0);
    }

    [Fact]
    public void All_ones_scores_twenty()
    {
        Game.Score(Enumerable.Repeat(1, 20).ToArray()).Should().Be(20);
    }
}
