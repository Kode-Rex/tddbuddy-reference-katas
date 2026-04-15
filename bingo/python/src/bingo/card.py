from __future__ import annotations

from dataclasses import dataclass
from typing import List, Optional


# Identical byte-for-byte across C#, TypeScript, and Python.
# The exception messages are the spec (see ../SCENARIOS.md).
class CardMessages:
    NUMBER_OUT_OF_RANGE = "called number must be between 1 and 75"


CARD_SIZE = 5
FREE_ROW = 2
FREE_COLUMN = 2
MIN_NUMBER = 1
MAX_NUMBER = 75


class NumberOutOfRangeError(ValueError):
    def __init__(self) -> None:
        super().__init__(CardMessages.NUMBER_OUT_OF_RANGE)


@dataclass(frozen=True)
class WinPattern:
    kind: str
    index: Optional[int] = None

    def __repr__(self) -> str:  # pragma: no cover - readability only
        if self.index is None:
            return f"WinPattern({self.kind})"
        return f"WinPattern({self.kind}, {self.index})"


class WinPatterns:
    NONE = WinPattern("None")
    DIAGONAL_MAIN = WinPattern("DiagonalMain")
    DIAGONAL_ANTI = WinPattern("DiagonalAnti")

    @staticmethod
    def row(index: int) -> WinPattern:
        return WinPattern("Row", index)

    @staticmethod
    def column(index: int) -> WinPattern:
        return WinPattern("Column", index)


NumberGrid = List[List[Optional[int]]]
MarkGrid = List[List[bool]]


class Card:
    def __init__(self, numbers: NumberGrid, marks: MarkGrid) -> None:
        """Test-folder constructor hook; production code should use a generator."""
        self._numbers: NumberGrid = [row[:] for row in numbers]
        self._marks: MarkGrid = [row[:] for row in marks]

    def number_at(self, row: int, col: int) -> Optional[int]:
        return self._numbers[row][col]

    def is_marked(self, row: int, col: int) -> bool:
        return self._marks[row][col]

    def mark(self, number: int) -> None:
        if number < MIN_NUMBER or number > MAX_NUMBER:
            raise NumberOutOfRangeError()
        for r in range(CARD_SIZE):
            for c in range(CARD_SIZE):
                if self._numbers[r][c] == number:
                    self._marks[r][c] = True

    def has_won(self) -> bool:
        return self.winning_pattern() != WinPatterns.NONE

    def winning_pattern(self) -> WinPattern:
        for r in range(CARD_SIZE):
            if self._row_marked(r):
                return WinPatterns.row(r)
        for c in range(CARD_SIZE):
            if self._column_marked(c):
                return WinPatterns.column(c)
        if self._main_diagonal_marked():
            return WinPatterns.DIAGONAL_MAIN
        if self._anti_diagonal_marked():
            return WinPatterns.DIAGONAL_ANTI
        return WinPatterns.NONE

    def _row_marked(self, r: int) -> bool:
        return all(self._marks[r][c] for c in range(CARD_SIZE))

    def _column_marked(self, c: int) -> bool:
        return all(self._marks[r][c] for r in range(CARD_SIZE))

    def _main_diagonal_marked(self) -> bool:
        return all(self._marks[i][i] for i in range(CARD_SIZE))

    def _anti_diagonal_marked(self) -> bool:
        return all(self._marks[i][CARD_SIZE - 1 - i] for i in range(CARD_SIZE))
