from __future__ import annotations

from maze_walker import (
    Cell,
    CellType,
    Maze,
    NoStartCellException,
    NoEndCellException,
    MultipleStartCellsException,
    MultipleEndCellsException,
)


class MazeBuilder:
    def __init__(self) -> None:
        self._layout = "SE"

    def with_layout(self, layout: str) -> MazeBuilder:
        self._layout = layout
        return self

    def build(self) -> Maze:
        lines = self._layout.split("\n")
        rows = len(lines)
        cols = max(len(line) for line in lines)
        grid: list[list[CellType]] = []

        start: Cell | None = None
        end: Cell | None = None
        start_count = 0
        end_count = 0

        for r in range(rows):
            row: list[CellType] = []
            for c in range(cols):
                ch = lines[r][c] if c < len(lines[r]) else "."
                if ch == "#":
                    row.append(CellType.WALL)
                elif ch == "S":
                    row.append(CellType.START)
                    start = Cell(r, c)
                    start_count += 1
                elif ch == "E":
                    row.append(CellType.END)
                    end = Cell(r, c)
                    end_count += 1
                else:
                    row.append(CellType.OPEN)
            grid.append(row)

        if start_count == 0:
            raise NoStartCellException()
        if start_count > 1:
            raise MultipleStartCellsException()
        if end_count == 0:
            raise NoEndCellException()
        if end_count > 1:
            raise MultipleEndCellsException()

        assert start is not None
        assert end is not None
        return Maze(grid, start, end)
