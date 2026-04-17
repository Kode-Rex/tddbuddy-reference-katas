from snake_game.position import Position
from tests.board_builder import BoardBuilder
from tests.snake_builder import SnakeBuilder


class TestEatingFood:
    def test_snake_eats_food_and_grows_by_one_segment(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 0).moving_right().build())
            .with_food_at(1, 0)
            .build()
        )

        game.tick()

        assert game.snake.length == 2
        assert game.snake.head == Position(1, 0)
        assert game.snake.body == [Position(1, 0), Position(0, 0)]

    def test_eating_food_increments_the_score(self) -> None:
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 0).moving_right().build())
            .with_food_at(1, 0)
            .build()
        )

        game.tick()

        assert game.score == 1

    def test_new_food_spawns_after_eating_at_the_position_chosen_by_the_spawner(self) -> None:
        next_food_position = Position(3, 3)
        game = (
            BoardBuilder()
            .with_snake(SnakeBuilder().at(0, 0).moving_right().build())
            .with_food_at(1, 0)
            .with_food_spawner(lambda _: next_food_position)
            .build()
        )

        game.tick()

        assert game.food == next_food_position
