using FluentAssertions;

namespace SnakeGame.Tests;

public class WallCollisionTests
{
    [Fact]
    public void Snake_hitting_the_right_wall_causes_game_over()
    {
        var game = new BoardBuilder()
            .WithSize(5, 5)
            .WithSnake(new SnakeBuilder().At(4, 0).MovingRight().Build())
            .Build();

        game.Tick();

        game.State.Should().Be(GameState.GameOver);
    }

    [Fact]
    public void Snake_hitting_the_bottom_wall_causes_game_over()
    {
        var game = new BoardBuilder()
            .WithSize(5, 5)
            .WithSnake(new SnakeBuilder().At(0, 4).MovingDown().Build())
            .Build();

        game.Tick();

        game.State.Should().Be(GameState.GameOver);
    }

    [Fact]
    public void Snake_hitting_the_left_wall_causes_game_over()
    {
        var game = new BoardBuilder()
            .WithSize(5, 5)
            .WithSnake(new SnakeBuilder().At(0, 0).MovingLeft().Build())
            .Build();

        game.Tick();

        game.State.Should().Be(GameState.GameOver);
    }

    [Fact]
    public void Snake_hitting_the_top_wall_causes_game_over()
    {
        var game = new BoardBuilder()
            .WithSize(5, 5)
            .WithSnake(new SnakeBuilder().At(0, 0).MovingUp().Build())
            .Build();

        game.Tick();

        game.State.Should().Be(GameState.GameOver);
    }
}
