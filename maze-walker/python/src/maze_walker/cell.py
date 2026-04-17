from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class Cell:
    """A coordinate in the maze. Value type with equality by (row, col)."""

    row: int
    col: int

    def cardinal_neighbors(self) -> list[Cell]:
        """Return the four cardinal neighbors (up, down, left, right)."""
        return [
            Cell(self.row - 1, self.col),
            Cell(self.row + 1, self.col),
            Cell(self.row, self.col - 1),
            Cell(self.row, self.col + 1),
        ]
