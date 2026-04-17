from __future__ import annotations

from enum import Enum

from .position import Position


class Direction(Enum):
    UP = "Up"
    DOWN = "Down"
    LEFT = "Left"
    RIGHT = "Right"

    def is_opposite(self, other: Direction) -> bool:
        return _OPPOSITES[self] == other

    def move(self, position: Position) -> Position:
        dx, dy = _DELTAS[self]
        return Position(position.x + dx, position.y + dy)


_OPPOSITES: dict[Direction, Direction] = {
    Direction.UP: Direction.DOWN,
    Direction.DOWN: Direction.UP,
    Direction.LEFT: Direction.RIGHT,
    Direction.RIGHT: Direction.LEFT,
}

_DELTAS: dict[Direction, tuple[int, int]] = {
    Direction.UP: (0, -1),
    Direction.DOWN: (0, 1),
    Direction.LEFT: (-1, 0),
    Direction.RIGHT: (1, 0),
}
