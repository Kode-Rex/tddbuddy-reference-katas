from __future__ import annotations

from game_of_life import Cell, Grid


class GridBuilder:
    def __init__(self) -> None:
        self._living_cells: list[Cell] = []

    def with_living_cell_at(self, row: int, col: int) -> GridBuilder:
        self._living_cells.append(Cell(row, col))
        return self

    def with_living_cells_at(self, *cells: tuple[int, int]) -> GridBuilder:
        for row, col in cells:
            self._living_cells.append(Cell(row, col))
        return self

    def build(self) -> Grid:
        return Grid(self._living_cells)
