from __future__ import annotations

from enum import Enum
from typing import Callable

from .position import Position
from .direction import Direction
from .snake import Snake


class GameState(Enum):
    PLAYING = "Playing"
    GAME_OVER = "GameOver"
    WON = "Won"


FoodSpawner = Callable[[list[Position]], Position]


class Game:
    """
    Aggregate: board + snake + food + score + state.
    ``tick()`` advances the game by one step.
    """

    def __init__(
        self,
        width: int,
        height: int,
        food_spawner: FoodSpawner,
        snake: Snake | None = None,
        food: Position | None = None,
        score: int = 0,
        state: GameState = GameState.PLAYING,
    ) -> None:
        self._width = width
        self._height = height
        self._food_spawner = food_spawner
        self.snake = snake or Snake([Position(0, 0)], Direction.RIGHT)
        self.score = score
        self.state = state
        self.food = food if food is not None else food_spawner(self._empty_cells())

    def change_direction(self, new_direction: Direction) -> None:
        if self.state != GameState.PLAYING:
            return
        self.snake = self.snake.change_direction(new_direction)

    def tick(self) -> None:
        if self.state != GameState.PLAYING:
            return

        new_head = self.snake.direction.move(self.snake.head)

        if self._is_out_of_bounds(new_head) or self._is_body_collision(new_head):
            self.state = GameState.GAME_OVER
            return

        eats_food = new_head == self.food
        self.snake = self.snake.move(eats_food)

        if eats_food:
            self.score += 1

            if self.snake.length == self._width * self._height:
                self.state = GameState.WON
                return

            self.food = self._food_spawner(self._empty_cells())

    def _is_out_of_bounds(self, position: Position) -> bool:
        return (
            position.x < 0
            or position.x >= self._width
            or position.y < 0
            or position.y >= self._height
        )

    def _is_body_collision(self, position: Position) -> bool:
        return self.snake.occupies_position(position)

    def _empty_cells(self) -> list[Position]:
        empty: list[Position] = []
        for x in range(self._width):
            for y in range(self._height):
                pos = Position(x, y)
                if not self.snake.occupies_position(pos):
                    empty.append(pos)
        return empty
