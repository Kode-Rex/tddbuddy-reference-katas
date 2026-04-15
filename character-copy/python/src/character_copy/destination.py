from __future__ import annotations

from typing import Protocol


class Destination(Protocol):
    def write(self, ch: str) -> None: ...
