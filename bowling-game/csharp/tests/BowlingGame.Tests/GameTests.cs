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

    [Fact]
    public void One_spare_scores_the_spare_bonus()
    {
        var rolls = new[] { 5, 5, 3, 0 }.Concat(new int[16]).ToArray();
        Game.Score(rolls).Should().Be(16);
    }

    [Fact]
    public void One_strike_scores_the_strike_bonus()
    {
        var rolls = new[] { 10, 3, 4 }.Concat(new int[16]).ToArray();
        Game.Score(rolls).Should().Be(24);
    }

    [Fact]
    public void Perfect_game_scores_three_hundred()
    {
        Game.Score(Enumerable.Repeat(10, 12).ToArray()).Should().Be(300);
    }
}
