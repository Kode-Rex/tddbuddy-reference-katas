from __future__ import annotations

from typing import Protocol


class PinGenerator(Protocol):
    def generate(self) -> int: ...
