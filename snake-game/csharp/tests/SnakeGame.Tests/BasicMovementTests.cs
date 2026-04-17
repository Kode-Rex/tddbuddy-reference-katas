using FluentAssertions;

namespace SnakeGame.Tests;

public class BasicMovementTests
{
    [Fact]
    public void Snake_moving_right_advances_x_by_one()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 0).MovingRight().Build())
            .Build();

        game.Tick();

        game.Snake.Head.Should().Be(new Position(1, 0));
    }

    [Fact]
    public void Snake_moving_down_advances_y_by_one()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 0).MovingDown().Build())
            .Build();

        game.Tick();

        game.Snake.Head.Should().Be(new Position(0, 1));
    }

    [Fact]
    public void Snake_moving_left_decreases_x_by_one()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(2, 0).MovingLeft().Build())
            .Build();

        game.Tick();

        game.Snake.Head.Should().Be(new Position(1, 0));
    }

    [Fact]
    public void Snake_moving_up_decreases_y_by_one()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 2).MovingUp().Build())
            .Build();

        game.Tick();

        game.Snake.Head.Should().Be(new Position(0, 1));
    }
}
