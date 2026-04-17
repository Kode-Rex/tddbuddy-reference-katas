from __future__ import annotations

from collections import deque

from .cell import Cell
from .maze import Maze


class Walker:
    """Navigates a maze using BFS to find the shortest path from start to end."""

    def __init__(self, maze: Maze) -> None:
        self._maze = maze

    def find_path(self) -> list[Cell]:
        """
        Finds the shortest path from start to end using BFS.
        Returns an empty list when no path exists.
        """
        start = self._maze.start
        end = self._maze.end

        visited: set[Cell] = {start}
        queue: deque[Cell] = deque([start])
        came_from: dict[Cell, Cell] = {}

        while queue:
            current = queue.popleft()

            if current == end:
                return self._reconstruct_path(came_from, start, end)

            for neighbor in current.cardinal_neighbors():
                if not self._maze.is_walkable(neighbor):
                    continue
                if neighbor in visited:
                    continue

                visited.add(neighbor)
                came_from[neighbor] = current
                queue.append(neighbor)

        return []

    @staticmethod
    def _reconstruct_path(
        came_from: dict[Cell, Cell], start: Cell, end: Cell
    ) -> list[Cell]:
        path: list[Cell] = []
        current = end

        while current != start:
            path.append(current)
            current = came_from[current]

        path.append(start)
        path.reverse()
        return path
