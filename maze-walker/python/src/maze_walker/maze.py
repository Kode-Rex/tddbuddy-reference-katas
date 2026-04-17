from __future__ import annotations

from .cell import Cell
from .cell_type import CellType


class Maze:
    """Immutable rectangular grid of cells with exactly one start and one end."""

    def __init__(
        self, grid: list[list[CellType]], start: Cell, end: Cell
    ) -> None:
        self._grid = grid
        self.rows = len(grid)
        self.cols = len(grid[0]) if grid else 0
        self.start = start
        self.end = end

    def cell_type_at(self, row: int, col: int) -> CellType | None:
        """Returns the cell type at the given position, or None if out of bounds."""
        if row < 0 or row >= self.rows or col < 0 or col >= self.cols:
            return None
        return self._grid[row][col]

    def is_walkable(self, cell: Cell) -> bool:
        """Returns True if the given cell is within bounds and not a wall."""
        cell_type = self.cell_type_at(cell.row, cell.col)
        return cell_type is not None and cell_type != CellType.WALL
