from __future__ import annotations

from collections import Counter
from typing import Iterable

from .cell import Cell


class Grid:
    """
    Immutable set of living cells on an unbounded infinite plane.
    ``tick()`` applies the four GoL rules and returns the next generation.
    """

    def __init__(self, living_cells: Iterable[Cell] = ()) -> None:
        self._living_cells: frozenset[Cell] = frozenset(living_cells)

    def is_alive(self, row: int, col: int) -> bool:
        return Cell(row, col) in self._living_cells

    def living_cells(self) -> frozenset[Cell]:
        return self._living_cells

    def tick(self) -> Grid:
        neighbor_counts: Counter[Cell] = Counter()

        for cell in self._living_cells:
            for neighbor in cell.neighbors():
                neighbor_counts[neighbor] += 1

        next_generation: set[Cell] = set()

        for candidate, count in neighbor_counts.items():
            is_alive = candidate in self._living_cells
            if count == 3 or (count == 2 and is_alive):
                next_generation.add(candidate)

        return Grid(next_generation)
