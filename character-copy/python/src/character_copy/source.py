from __future__ import annotations

from typing import Protocol


class Source(Protocol):
    def read(self) -> str: ...
