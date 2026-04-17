from snake_game.game import GameState
from tests.board_builder import BoardBuilder
from tests.snake_builder import SnakeBuilder


class TestGameOverBehavior:
    def test_tick_has_no_effect_after_game_over(self) -> None:
        game = (
            BoardBuilder()
            .with_size(5, 5)
            .with_snake(SnakeBuilder().at(4, 0).moving_right().build())
            .build()
        )

        game.tick()  # game over — hit right wall
        assert game.state == GameState.GAME_OVER

        head_after_game_over = game.snake.head
        score_after_game_over = game.score

        game.tick()  # should have no effect

        assert game.snake.head == head_after_game_over
        assert game.score == score_after_game_over
        assert game.state == GameState.GAME_OVER
