from __future__ import annotations

from collections import deque


class FixedPinGenerator:
    def __init__(self, *pins: int) -> None:
        self._pins: deque[int] = deque(pins)

    def generate(self) -> int:
        return self._pins.popleft()
