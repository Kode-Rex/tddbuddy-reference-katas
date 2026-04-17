from snake_game.position import Position
from snake_game.direction import Direction
from snake_game.game import GameState
from tests.board_builder import BoardBuilder


class TestInitialState:
    def test_a_new_game_has_the_snake_at_0_0_moving_right(self) -> None:
        game = BoardBuilder().build()

        assert game.snake.head == Position(0, 0)
        assert game.snake.direction == Direction.RIGHT

    def test_a_new_game_has_a_score_of_zero(self) -> None:
        game = BoardBuilder().build()

        assert game.score == 0

    def test_a_new_game_is_in_playing_state(self) -> None:
        game = BoardBuilder().build()

        assert game.state == GameState.PLAYING

    def test_a_new_game_has_food_on_the_board(self) -> None:
        game = BoardBuilder().with_food_at(3, 3).build()

        assert game.food == Position(3, 3)
