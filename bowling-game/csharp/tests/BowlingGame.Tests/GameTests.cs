using FluentAssertions;

namespace BowlingGame.Tests;

public class GameTests
{
    [Fact]
    public void Gutter_game_scores_zero()
    {
        Game.Score(new int[20]).Should().Be(0);
    }
}
