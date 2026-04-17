using FluentAssertions;

namespace SnakeGame.Tests;

public class SelfCollisionTests
{
    [Fact]
    public void Snake_colliding_with_its_own_body_causes_game_over()
    {
        // Snake body: (3,0) -> (2,0) -> (1,0) -> (0,0), moving down.
        // After tick 1: head at (3,1), body: (3,1) -> (3,0) -> (2,0) -> (1,0)
        // Change to left. After tick 2: head at (2,1), body: (2,1) -> (3,1) -> (3,0) -> (2,0)
        // Change to up. After tick 3: head would be (2,0), which is part of the body.
        var game = new BoardBuilder()
            .WithSize(5, 5)
            .WithSnake(new SnakeBuilder()
                .WithBodyAt((3, 0), (2, 0), (1, 0), (0, 0))
                .MovingDown()
                .Build())
            .WithFoodAt(4, 4)
            .Build();

        game.Tick(); // head moves to (3,1)
        game.ChangeDirection(Direction.Left);
        game.Tick(); // head moves to (2,1)
        game.ChangeDirection(Direction.Up);
        game.Tick(); // head would move to (2,0) — body collision

        game.State.Should().Be(GameState.GameOver);
    }
}
