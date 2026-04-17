from tests.maze_builder import MazeBuilder
from tests.walker_builder import WalkerBuilder


def test_a_wall_between_start_and_end_returns_an_empty_path() -> None:
    maze = MazeBuilder().with_layout("S#E").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert path == []


def test_a_maze_with_no_reachable_end_returns_an_empty_path() -> None:
    maze = MazeBuilder().with_layout("S.#\n.##\n##E").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert path == []
