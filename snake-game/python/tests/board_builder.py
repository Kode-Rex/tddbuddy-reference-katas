from typing import Callable

from snake_game.position import Position
from snake_game.direction import Direction
from snake_game.snake import Snake
from snake_game.game import Game, GameState, FoodSpawner


class BoardBuilder:
    def __init__(self) -> None:
        self._width: int = 5
        self._height: int = 5
        self._snake: Snake | None = None
        self._food: Position | None = None
        self._food_spawner: FoodSpawner | None = None

    def with_size(self, width: int, height: int) -> "BoardBuilder":
        self._width = width
        self._height = height
        return self

    def with_snake(self, snake: Snake) -> "BoardBuilder":
        self._snake = snake
        return self

    def with_food_at(self, x: int, y: int) -> "BoardBuilder":
        self._food = Position(x, y)
        return self

    def with_food_spawner(self, spawner: FoodSpawner) -> "BoardBuilder":
        self._food_spawner = spawner
        return self

    def build(self) -> Game:
        snake = self._snake or Snake([Position(0, 0)], Direction.RIGHT)
        food = self._food or Position(self._width - 1, self._height - 1)
        spawner = self._food_spawner or (lambda cells: cells[0])

        return Game(
            self._width,
            self._height,
            spawner,
            snake=snake,
            food=food,
            score=0,
            state=GameState.PLAYING,
        )
