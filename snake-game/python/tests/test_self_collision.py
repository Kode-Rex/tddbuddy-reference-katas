from snake_game.direction import Direction
from snake_game.game import GameState
from tests.board_builder import BoardBuilder
from tests.snake_builder import SnakeBuilder


class TestSelfCollision:
    def test_snake_colliding_with_its_own_body_causes_game_over(self) -> None:
        game = (
            BoardBuilder()
            .with_size(5, 5)
            .with_snake(
                SnakeBuilder()
                .with_body_at((3, 0), (2, 0), (1, 0), (0, 0))
                .moving_down()
                .build()
            )
            .with_food_at(4, 4)
            .build()
        )

        game.tick()  # head moves to (3,1)
        game.change_direction(Direction.LEFT)
        game.tick()  # head moves to (2,1)
        game.change_direction(Direction.UP)
        game.tick()  # head would move to (2,0) — body collision

        assert game.state == GameState.GAME_OVER
