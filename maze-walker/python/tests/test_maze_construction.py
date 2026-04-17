import pytest

from maze_walker import (
    Cell,
    CellType,
    NoStartCellException,
    NoEndCellException,
    MultipleStartCellsException,
    MultipleEndCellsException,
)
from tests.maze_builder import MazeBuilder


def test_a_maze_can_be_built_from_a_string_art_representation() -> None:
    maze = MazeBuilder().with_layout("S.E").build()

    assert maze.rows == 1
    assert maze.cols == 3
    assert maze.start == Cell(0, 0)
    assert maze.end == Cell(0, 2)


def test_a_maze_identifies_walls_correctly() -> None:
    maze = MazeBuilder().with_layout("S#E").build()

    assert maze.cell_type_at(0, 1) == CellType.WALL


def test_a_maze_without_a_start_cell_throws_no_start_cell_exception() -> None:
    with pytest.raises(NoStartCellException, match="Maze must have exactly one start cell."):
        MazeBuilder().with_layout("..E").build()


def test_a_maze_without_an_end_cell_throws_no_end_cell_exception() -> None:
    with pytest.raises(NoEndCellException, match="Maze must have exactly one end cell."):
        MazeBuilder().with_layout("S..").build()


def test_a_maze_with_multiple_start_cells_throws_multiple_start_cells_exception() -> None:
    with pytest.raises(MultipleStartCellsException, match="Maze must have exactly one start cell."):
        MazeBuilder().with_layout("S.S\n..E").build()


def test_a_maze_with_multiple_end_cells_throws_multiple_end_cells_exception() -> None:
    with pytest.raises(MultipleEndCellsException, match="Maze must have exactly one end cell."):
        MazeBuilder().with_layout("S.E\n..E").build()
