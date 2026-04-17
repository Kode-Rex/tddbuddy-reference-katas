from tests.maze_builder import MazeBuilder
from tests.walker_builder import WalkerBuilder


def test_walker_solves_a_5x5_maze() -> None:
    maze = MazeBuilder().with_layout("S.#..\n.#...\n...#.\n.#..E\n.....").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert len(path) > 0
    assert path[0] == maze.start
    assert path[-1] == maze.end


def test_walker_solves_a_maze_requiring_exploration() -> None:
    maze = MazeBuilder().with_layout("S..#.\n##...\n...#E").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert len(path) > 0
    assert path[0] == maze.start
    assert path[-1] == maze.end
