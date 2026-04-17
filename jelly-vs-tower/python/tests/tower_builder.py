from __future__ import annotations

from jelly_vs_tower import ColorType, Tower


class TowerBuilder:
    def __init__(self) -> None:
        self._id = "tower-1"
        self._color = ColorType.Blue
        self._level = 1

    def with_id(self, id: str) -> TowerBuilder:
        self._id = id
        return self

    def with_color(self, color: ColorType) -> TowerBuilder:
        self._color = color
        return self

    def with_level(self, level: int) -> TowerBuilder:
        self._level = level
        return self

    def build(self) -> Tower:
        return Tower(self._id, self._color, self._level)
