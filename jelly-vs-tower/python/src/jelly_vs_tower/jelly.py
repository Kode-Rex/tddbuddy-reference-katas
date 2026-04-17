from __future__ import annotations

from .color_type import ColorType
from .exceptions import InvalidHealthException


class Jelly:
    def __init__(self, id: str, color: ColorType, health: int) -> None:
        if health <= 0:
            raise InvalidHealthException(health)
        self._id = id
        self._color = color
        self._health = health

    @property
    def id(self) -> str:
        return self._id

    @property
    def color(self) -> ColorType:
        return self._color

    @property
    def health(self) -> int:
        return self._health

    @property
    def is_alive(self) -> bool:
        return self._health > 0

    def take_damage(self, damage: int) -> None:
        self._health -= damage
        if self._health < 0:
            self._health = 0
