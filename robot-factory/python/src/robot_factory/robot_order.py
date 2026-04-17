from __future__ import annotations

from .order_incomplete_error import OrderIncompleteError
from .part_option import PartOption
from .part_type import PartType


class RobotOrder:
    def __init__(self) -> None:
        self._parts: dict[PartType, PartOption] = {}

    @property
    def parts(self) -> dict[PartType, PartOption]:
        return dict(self._parts)

    def configure(self, part_type: PartType, option: PartOption) -> None:
        self._parts[part_type] = option

    def validate(self) -> None:
        missing = [t for t in PartType if t not in self._parts]
        if missing:
            names = ", ".join(t.value for t in missing)
            raise OrderIncompleteError(
                f"Order is missing part types: {names}"
            )
