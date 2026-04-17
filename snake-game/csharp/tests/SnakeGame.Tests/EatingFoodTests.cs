using FluentAssertions;

namespace SnakeGame.Tests;

public class EatingFoodTests
{
    [Fact]
    public void Snake_eats_food_and_grows_by_one_segment()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 0).MovingRight().Build())
            .WithFoodAt(1, 0)
            .Build();

        game.Tick();

        game.Snake.Length.Should().Be(2);
        game.Snake.Head.Should().Be(new Position(1, 0));
        game.Snake.Body.Should().ContainInOrder(new Position(1, 0), new Position(0, 0));
    }

    [Fact]
    public void Eating_food_increments_the_score()
    {
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 0).MovingRight().Build())
            .WithFoodAt(1, 0)
            .Build();

        game.Tick();

        game.Score.Should().Be(1);
    }

    [Fact]
    public void New_food_spawns_after_eating_at_the_position_chosen_by_the_spawner()
    {
        var nextFoodPosition = new Position(3, 3);
        var game = new BoardBuilder()
            .WithSnake(new SnakeBuilder().At(0, 0).MovingRight().Build())
            .WithFoodAt(1, 0)
            .WithFoodSpawner(_ => nextFoodPosition)
            .Build();

        game.Tick();

        game.Food.Should().Be(nextFoodPosition);
    }
}
