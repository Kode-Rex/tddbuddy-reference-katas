using FluentAssertions;

namespace SnakeGame.Tests;

public class InitialStateTests
{
    [Fact]
    public void A_new_game_has_the_snake_at_0_0_moving_right()
    {
        var game = new BoardBuilder().Build();

        game.Snake.Head.Should().Be(new Position(0, 0));
        game.Snake.Direction.Should().Be(Direction.Right);
    }

    [Fact]
    public void A_new_game_has_a_score_of_zero()
    {
        var game = new BoardBuilder().Build();

        game.Score.Should().Be(0);
    }

    [Fact]
    public void A_new_game_is_in_playing_state()
    {
        var game = new BoardBuilder().Build();

        game.State.Should().Be(GameState.Playing);
    }

    [Fact]
    public void A_new_game_has_food_on_the_board()
    {
        var game = new BoardBuilder()
            .WithFoodAt(3, 3)
            .Build();

        game.Food.Should().Be(new Position(3, 3));
    }
}
