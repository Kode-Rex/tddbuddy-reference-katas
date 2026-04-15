from __future__ import annotations

from enum import Enum
from typing import List, Tuple


# Identical byte-for-byte across C#, TypeScript, and Python.
# The exception messages are the spec (see ../SCENARIOS.md).
class BoardMessages:
    CELL_OCCUPIED = "cell already occupied"
    OUT_OF_BOUNDS = "coordinates out of bounds"
    GAME_OVER = "game is already over"


BOARD_SIZE = 3


class Cell(Enum):
    EMPTY = "Empty"
    X = "X"
    O = "O"


class Outcome(Enum):
    IN_PROGRESS = "InProgress"
    X_WINS = "XWins"
    O_WINS = "OWins"
    DRAW = "Draw"


class CellOccupiedError(Exception):
    def __init__(self) -> None:
        super().__init__(BoardMessages.CELL_OCCUPIED)


class OutOfBoundsError(Exception):
    def __init__(self) -> None:
        super().__init__(BoardMessages.OUT_OF_BOUNDS)


class GameOverError(Exception):
    def __init__(self) -> None:
        super().__init__(BoardMessages.GAME_OVER)


_WINNING_LINES: Tuple[Tuple[Tuple[int, int], ...], ...] = (
    ((0, 0), (0, 1), (0, 2)),
    ((1, 0), (1, 1), (1, 2)),
    ((2, 0), (2, 1), (2, 2)),
    ((0, 0), (1, 0), (2, 0)),
    ((0, 1), (1, 1), (2, 1)),
    ((0, 2), (1, 2), (2, 2)),
    ((0, 0), (1, 1), (2, 2)),
    ((0, 2), (1, 1), (2, 0)),
)

Grid = List[List[Cell]]


def _empty_grid() -> Grid:
    return [[Cell.EMPTY for _ in range(BOARD_SIZE)] for _ in range(BOARD_SIZE)]


def _in_bounds(row: int, col: int) -> bool:
    return 0 <= row < BOARD_SIZE and 0 <= col < BOARD_SIZE


class Board:
    def __init__(self, grid: Grid | None = None) -> None:
        self._grid: Grid = grid if grid is not None else _empty_grid()

    def cell_at(self, row: int, col: int) -> Cell:
        if not _in_bounds(row, col):
            raise OutOfBoundsError()
        return self._grid[row][col]

    def current_turn(self) -> Cell:
        xs = self._count_of(Cell.X)
        os = self._count_of(Cell.O)
        return Cell.X if xs == os else Cell.O

    def outcome(self) -> Outcome:
        for line in _WINNING_LINES:
            (r0, c0), (r1, c1), (r2, c2) = line
            first = self._grid[r0][c0]
            if first is Cell.EMPTY:
                continue
            if first is self._grid[r1][c1] and first is self._grid[r2][c2]:
                return Outcome.X_WINS if first is Cell.X else Outcome.O_WINS
        if self._count_of(Cell.EMPTY) == 0:
            return Outcome.DRAW
        return Outcome.IN_PROGRESS

    def place(self, row: int, col: int) -> "Board":
        if self.outcome() is not Outcome.IN_PROGRESS:
            raise GameOverError()
        if not _in_bounds(row, col):
            raise OutOfBoundsError()
        if self._grid[row][col] is not Cell.EMPTY:
            raise CellOccupiedError()

        next_grid: Grid = [row_cells[:] for row_cells in self._grid]
        next_grid[row][col] = self.current_turn()
        return Board(next_grid)

    @classmethod
    def from_grid(cls, grid: Grid) -> "Board":
        """Test-folder builder hook; production code should use `place`."""
        return cls([row_cells[:] for row_cells in grid])

    def _count_of(self, mark: Cell) -> int:
        return sum(1 for r in range(BOARD_SIZE) for c in range(BOARD_SIZE) if self._grid[r][c] is mark)
