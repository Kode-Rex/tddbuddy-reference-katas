from __future__ import annotations

from .color_type import ColorType
from .exceptions import InvalidLevelException


class Tower:
    def __init__(self, id: str, color: ColorType, level: int) -> None:
        if level < 1 or level > 4:
            raise InvalidLevelException(level)
        self._id = id
        self._color = color
        self._level = level

    @property
    def id(self) -> str:
        return self._id

    @property
    def color(self) -> ColorType:
        return self._color

    @property
    def level(self) -> int:
        return self._level
