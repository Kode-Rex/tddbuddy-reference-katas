from snake_game.game import GameState
from tests.board_builder import BoardBuilder
from tests.snake_builder import SnakeBuilder


class TestWallCollisions:
    def test_snake_hitting_the_right_wall_causes_game_over(self) -> None:
        game = (
            BoardBuilder()
            .with_size(5, 5)
            .with_snake(SnakeBuilder().at(4, 0).moving_right().build())
            .build()
        )

        game.tick()

        assert game.state == GameState.GAME_OVER

    def test_snake_hitting_the_bottom_wall_causes_game_over(self) -> None:
        game = (
            BoardBuilder()
            .with_size(5, 5)
            .with_snake(SnakeBuilder().at(0, 4).moving_down().build())
            .build()
        )

        game.tick()

        assert game.state == GameState.GAME_OVER

    def test_snake_hitting_the_left_wall_causes_game_over(self) -> None:
        game = (
            BoardBuilder()
            .with_size(5, 5)
            .with_snake(SnakeBuilder().at(0, 0).moving_left().build())
            .build()
        )

        game.tick()

        assert game.state == GameState.GAME_OVER

    def test_snake_hitting_the_top_wall_causes_game_over(self) -> None:
        game = (
            BoardBuilder()
            .with_size(5, 5)
            .with_snake(SnakeBuilder().at(0, 0).moving_up().build())
            .build()
        )

        game.tick()

        assert game.state == GameState.GAME_OVER
