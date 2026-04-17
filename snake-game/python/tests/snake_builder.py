from snake_game.position import Position
from snake_game.direction import Direction
from snake_game.snake import Snake


class SnakeBuilder:
    def __init__(self) -> None:
        self._body: list[Position] = [Position(0, 0)]
        self._direction: Direction = Direction.RIGHT

    def at(self, x: int, y: int) -> "SnakeBuilder":
        self._body = [Position(x, y)]
        return self

    def with_body_at(self, *positions: tuple[int, int]) -> "SnakeBuilder":
        self._body = [Position(x, y) for x, y in positions]
        return self

    def moving_up(self) -> "SnakeBuilder":
        self._direction = Direction.UP
        return self

    def moving_down(self) -> "SnakeBuilder":
        self._direction = Direction.DOWN
        return self

    def moving_left(self) -> "SnakeBuilder":
        self._direction = Direction.LEFT
        return self

    def moving_right(self) -> "SnakeBuilder":
        self._direction = Direction.RIGHT
        return self

    def build(self) -> Snake:
        return Snake(self._body, self._direction)
