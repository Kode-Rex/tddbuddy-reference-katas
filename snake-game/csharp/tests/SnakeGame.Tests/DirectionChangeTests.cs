using FluentAssertions;

namespace SnakeGame.Tests;

public class DirectionChangeTests
{
    [Fact]
    public void Changing_direction_to_down_then_ticking_moves_the_snake_down()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(1, 1).MovingRight().Build())
            .Build();

        game.ChangeDirection(Direction.Down);
        game.Tick();

        game.Snake.Head.Should().Be(new Position(1, 2));
    }

    [Fact]
    public void Cannot_reverse_direction_from_right_to_left()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(1, 0).MovingRight().Build())
            .Build();

        game.ChangeDirection(Direction.Left);
        game.Tick();

        game.Snake.Head.Should().Be(new Position(2, 0));
    }

    [Fact]
    public void Cannot_reverse_direction_from_up_to_down()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 2).MovingUp().Build())
            .Build();

        game.ChangeDirection(Direction.Down);
        game.Tick();

        game.Snake.Head.Should().Be(new Position(0, 1));
    }

    [Fact]
    public void Cannot_reverse_direction_from_left_to_right()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(2, 0).MovingLeft().Build())
            .Build();

        game.ChangeDirection(Direction.Right);
        game.Tick();

        game.Snake.Head.Should().Be(new Position(1, 0));
    }

    [Fact]
    public void Cannot_reverse_direction_from_down_to_up()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 1).MovingDown().Build())
            .Build();

        game.ChangeDirection(Direction.Up);
        game.Tick();

        game.Snake.Head.Should().Be(new Position(0, 2));
    }
}
