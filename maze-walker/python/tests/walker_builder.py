from __future__ import annotations

from maze_walker import Maze, Walker
from tests.maze_builder import MazeBuilder


class WalkerBuilder:
    def __init__(self) -> None:
        self._maze: Maze = MazeBuilder().build()

    def with_maze(self, maze: Maze) -> WalkerBuilder:
        self._maze = maze
        return self

    def build(self) -> Walker:
        return Walker(self._maze)
