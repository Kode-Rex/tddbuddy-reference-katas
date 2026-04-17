from maze_walker import Cell
from tests.maze_builder import MazeBuilder
from tests.walker_builder import WalkerBuilder


def test_walker_finds_the_shortest_path_around_a_wall() -> None:
    maze = MazeBuilder().with_layout("S.\n#.\nE.").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert path == [
        Cell(0, 0), Cell(0, 1),
        Cell(1, 1),
        Cell(2, 1), Cell(2, 0),
    ]


def test_walker_picks_the_shortest_of_two_possible_routes() -> None:
    maze = MazeBuilder().with_layout("S.E\n...").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert len(path) == 3
    assert path[0] == Cell(0, 0)
    assert path[2] == Cell(0, 2)


def test_walker_navigates_a_winding_corridor() -> None:
    maze = MazeBuilder().with_layout("S.#\n.#.\n..E").build()
    walker = WalkerBuilder().with_maze(maze).build()

    path = walker.find_path()

    assert path == [
        Cell(0, 0),
        Cell(1, 0),
        Cell(2, 0), Cell(2, 1), Cell(2, 2),
    ]
