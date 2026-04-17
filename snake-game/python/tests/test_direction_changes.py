from snake_game.position import Position
from snake_game.direction import Direction
from tests.board_builder import BoardBuilder
from tests.snake_builder import SnakeBuilder


class TestDirectionChanges:
    def test_changing_direction_to_down_then_ticking_moves_the_snake_down(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(1, 1).moving_right().build())
            .build()
        )

        game.change_direction(Direction.DOWN)
        game.tick()

        assert game.snake.head == Position(1, 2)

    def test_cannot_reverse_direction_from_right_to_left(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(1, 0).moving_right().build())
            .build()
        )

        game.change_direction(Direction.LEFT)
        game.tick()

        assert game.snake.head == Position(2, 0)

    def test_cannot_reverse_direction_from_up_to_down(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 2).moving_up().build())
            .build()
        )

        game.change_direction(Direction.DOWN)
        game.tick()

        assert game.snake.head == Position(0, 1)

    def test_cannot_reverse_direction_from_left_to_right(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(2, 0).moving_left().build())
            .build()
        )

        game.change_direction(Direction.RIGHT)
        game.tick()

        assert game.snake.head == Position(1, 0)

    def test_cannot_reverse_direction_from_down_to_up(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 1).moving_down().build())
            .build()
        )

        game.change_direction(Direction.UP)
        game.tick()

        assert game.snake.head == Position(0, 2)
