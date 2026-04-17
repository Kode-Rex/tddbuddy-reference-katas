from maze_walker import CellType
from tests.maze_builder import MazeBuilder
from tests.walker_builder import WalkerBuilder


def _solve_maze(layout: str):
    maze = MazeBuilder().with_layout(layout).build()
    walker = WalkerBuilder().with_maze(maze).build()
    return maze, walker.find_path()


def test_the_path_starts_at_the_start_cell() -> None:
    maze, path = _solve_maze("S..E")

    assert path[0] == maze.start


def test_the_path_ends_at_the_end_cell() -> None:
    maze, path = _solve_maze("S..E")

    assert path[-1] == maze.end


def test_each_step_in_the_path_is_to_an_adjacent_cell() -> None:
    _, path = _solve_maze("S.#\n..#\n..E")

    for i in range(1, len(path)):
        dr = abs(path[i].row - path[i - 1].row)
        dc = abs(path[i].col - path[i - 1].col)
        assert dr + dc == 1, "each step should move exactly one cell cardinally"


def test_the_path_contains_no_walls() -> None:
    maze, path = _solve_maze("S.#\n..#\n..E")

    for cell in path:
        assert maze.cell_type_at(cell.row, cell.col) != CellType.WALL
