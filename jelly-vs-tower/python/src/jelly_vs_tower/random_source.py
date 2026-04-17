from __future__ import annotations

from typing import Protocol


class RandomSource(Protocol):
    def next(self, min_inclusive: int, max_inclusive: int) -> int: ...
