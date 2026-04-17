from __future__ import annotations

from typing import Protocol


class MachineSelector(Protocol):
    def select_available(self) -> int: ...
