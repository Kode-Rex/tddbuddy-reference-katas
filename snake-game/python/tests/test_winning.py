from snake_game.game import GameState
from tests.board_builder import BoardBuilder
from tests.snake_builder import SnakeBuilder


class TestWinning:
    def test_game_is_won_when_the_snake_fills_the_entire_board(self) -> None:
        game = (
            BoardBuilder()
            .with_size(2, 1)
            .with_snake(SnakeBuilder().at(0, 0).moving_right().build())
            .with_food_at(1, 0)
            .build()
        )

        game.tick()

        assert game.state == GameState.WON
        assert game.snake.length == 2
