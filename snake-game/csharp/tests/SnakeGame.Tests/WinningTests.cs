using FluentAssertions;

namespace SnakeGame.Tests;

public class WinningTests
{
    [Fact]
    public void Game_is_won_when_the_snake_fills_the_entire_board()
    {
        // 2x1 board: snake at (0,0), food at (1,0).
        // After one tick, snake eats food and fills the board.
        var game = new BoardBuilder()
            .WithSize(2, 1)
            .WithSnake(new SnakeBuilder().At(0, 0).MovingRight().Build())
            .WithFoodAt(1, 0)
            .Build();

        game.Tick();

        game.State.Should().Be(GameState.Won);
        game.Snake.Length.Should().Be(2);
    }
}
