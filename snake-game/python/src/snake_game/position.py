from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class Position:
    """A coordinate on the board. Value type with equality by (x, y)."""

    x: int
    y: int
