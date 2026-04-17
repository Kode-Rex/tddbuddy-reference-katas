using FluentAssertions;

namespace SnakeGame.Tests;

public class GameOverBehaviorTests
{
    [Fact]
    public void Tick_has_no_effect_after_game_over()
    {
        var game = new BoardBuilder()
            .WithSize(5, 5)
            .WithSnake(new SnakeBuilder().At(4, 0).MovingRight().Build())
            .Build();

        game.Tick(); // game over — hit right wall
        game.State.Should().Be(GameState.GameOver);

        var headAfterGameOver = game.Snake.Head;
        var scoreAfterGameOver = game.Score;

        game.Tick(); // should have no effect

        game.Snake.Head.Should().Be(headAfterGameOver);
        game.Score.Should().Be(scoreAfterGameOver);
        game.State.Should().Be(GameState.GameOver);
    }
}
