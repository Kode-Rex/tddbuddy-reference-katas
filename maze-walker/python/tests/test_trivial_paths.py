from maze_walker import Cell
from tests.maze_builder import MazeBuilder
from tests.walker_builder import WalkerBuilder


def test_start_adjacent_to_end_finds_a_two_cell_path() -> None:
    maze = MazeBuilder().with_layout("SE").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert path == [Cell(0, 0), Cell(0, 1)]


def test_a_straight_horizontal_corridor_finds_the_path() -> None:
    maze = MazeBuilder().with_layout("S..E").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert path == [Cell(0, 0), Cell(0, 1), Cell(0, 2), Cell(0, 3)]


def test_a_straight_vertical_corridor_finds_the_path() -> None:
    maze = MazeBuilder().with_layout("S\n.\nE").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert path == [Cell(0, 0), Cell(1, 0), Cell(2, 0)]
