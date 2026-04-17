from __future__ import annotations

from jelly_vs_tower import ColorType, Jelly


class JellyBuilder:
    def __init__(self) -> None:
        self._id = "jelly-1"
        self._color = ColorType.Blue
        self._health = 10

    def with_id(self, id: str) -> JellyBuilder:
        self._id = id
        return self

    def with_color(self, color: ColorType) -> JellyBuilder:
        self._color = color
        return self

    def with_health(self, health: int) -> JellyBuilder:
        self._health = health
        return self

    def build(self) -> Jelly:
        return Jelly(self._id, self._color, self._health)
