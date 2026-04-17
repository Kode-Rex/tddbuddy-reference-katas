from __future__ import annotations

from .position import Position
from .direction import Direction


class Snake:
    """Ordered list of positions, head first. Immutable — movement returns a new Snake."""

    def __init__(self, body: list[Position], direction: Direction) -> None:
        self._body = list(body)
        self._direction = direction

    @property
    def head(self) -> Position:
        return self._body[0]

    @property
    def direction(self) -> Direction:
        return self._direction

    @property
    def body(self) -> list[Position]:
        return list(self._body)

    @property
    def length(self) -> int:
        return len(self._body)

    def occupies_position(self, position: Position) -> bool:
        return position in self._body

    def move(self, grow: bool) -> Snake:
        new_head = self._direction.move(self.head)
        new_body = [new_head] + self._body

        if not grow:
            new_body.pop()

        return Snake(new_body, self._direction)

    def change_direction(self, new_direction: Direction) -> Snake:
        if self._direction.is_opposite(new_direction):
            return self
        return Snake(self._body, new_direction)
