from snake_game.position import Position
from tests.board_builder import BoardBuilder
from tests.snake_builder import SnakeBuilder


class TestBasicMovement:
    def test_snake_moving_right_advances_x_by_one(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 0).moving_right().build())
            .build()
        )

        game.tick()

        assert game.snake.head == Position(1, 0)

    def test_snake_moving_down_advances_y_by_one(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 0).moving_down().build())
            .build()
        )

        game.tick()

        assert game.snake.head == Position(0, 1)

    def test_snake_moving_left_decreases_x_by_one(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(2, 0).moving_left().build())
            .build()
        )

        game.tick()

        assert game.snake.head == Position(1, 0)

    def test_snake_moving_up_decreases_y_by_one(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 2).moving_up().build())
            .build()
        )

        game.tick()

        assert game.snake.head == Position(0, 1)
