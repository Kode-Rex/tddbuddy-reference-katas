from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class Cell:
    """A coordinate on the infinite plane. Value type with equality by (row, col)."""

    row: int
    col: int

    def neighbors(self) -> list[Cell]:
        """Return the eight orthogonal and diagonal neighbors."""
        result: list[Cell] = []
        for dr in range(-1, 2):
            for dc in range(-1, 2):
                if dr == 0 and dc == 0:
                    continue
                result.append(Cell(self.row + dr, self.col + dc))
        return result
