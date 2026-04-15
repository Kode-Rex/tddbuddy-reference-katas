from __future__ import annotations

from enum import Enum
from typing import Optional, Set, Tuple


# Identical byte-for-byte across C#, TypeScript, and Python.
# The exception messages are the spec (see ../SCENARIOS.md).
class RoverMessages:
    UNKNOWN_COMMAND = "unknown command"


DEFAULT_GRID_WIDTH = 100
DEFAULT_GRID_HEIGHT = 100


class Direction(Enum):
    NORTH = "North"
    EAST = "East"
    SOUTH = "South"
    WEST = "West"


class Command(Enum):
    FORWARD = "Forward"
    BACKWARD = "Backward"
    LEFT = "Left"
    RIGHT = "Right"


class MovementStatus(Enum):
    MOVING = "Moving"
    BLOCKED = "Blocked"


class UnknownCommandError(ValueError):
    def __init__(self) -> None:
        super().__init__(RoverMessages.UNKNOWN_COMMAND)


_LEFT_OF = {
    Direction.NORTH: Direction.WEST,
    Direction.WEST: Direction.SOUTH,
    Direction.SOUTH: Direction.EAST,
    Direction.EAST: Direction.NORTH,
}

_RIGHT_OF = {
    Direction.NORTH: Direction.EAST,
    Direction.EAST: Direction.SOUTH,
    Direction.SOUTH: Direction.WEST,
    Direction.WEST: Direction.NORTH,
}

_STEP_OF = {
    Direction.NORTH: (0, -1),
    Direction.SOUTH: (0, 1),
    Direction.EAST: (1, 0),
    Direction.WEST: (-1, 0),
}


def _parse_command(ch: str) -> Command:
    if ch == "F":
        return Command.FORWARD
    if ch == "B":
        return Command.BACKWARD
    if ch == "L":
        return Command.LEFT
    if ch == "R":
        return Command.RIGHT
    raise UnknownCommandError()


class Rover:
    def __init__(
        self,
        x: int,
        y: int,
        heading: Direction,
        grid_width: int = DEFAULT_GRID_WIDTH,
        grid_height: int = DEFAULT_GRID_HEIGHT,
        obstacles: Optional[Set[Tuple[int, int]]] = None,
        status: MovementStatus = MovementStatus.MOVING,
        last_obstacle: Optional[Tuple[int, int]] = None,
    ) -> None:
        self._x = x
        self._y = y
        self._heading = heading
        self._grid_width = grid_width
        self._grid_height = grid_height
        self._obstacles: Set[Tuple[int, int]] = set(obstacles) if obstacles else set()
        self._status = status
        self._last_obstacle = last_obstacle

    @property
    def position(self) -> Tuple[int, int]:
        return (self._x, self._y)

    @property
    def heading(self) -> Direction:
        return self._heading

    @property
    def grid_width(self) -> int:
        return self._grid_width

    @property
    def grid_height(self) -> int:
        return self._grid_height

    @property
    def status(self) -> MovementStatus:
        return self._status

    @property
    def last_obstacle(self) -> Optional[Tuple[int, int]]:
        return self._last_obstacle

    def has_obstacle_at(self, x: int, y: int) -> bool:
        return (x, y) in self._obstacles

    def execute(self, commands: str) -> "Rover":
        x, y = self._x, self._y
        heading = self._heading
        status = self._status
        last_obstacle = self._last_obstacle

        for ch in commands:
            cmd = _parse_command(ch)
            if status is MovementStatus.BLOCKED:
                break

            if cmd is Command.LEFT:
                heading = _LEFT_OF[heading]
            elif cmd is Command.RIGHT:
                heading = _RIGHT_OF[heading]
            else:
                sign = 1 if cmd is Command.FORWARD else -1
                dx, dy = _STEP_OF[heading]
                nx = (x + dx * sign) % self._grid_width
                ny = (y + dy * sign) % self._grid_height
                if (nx, ny) in self._obstacles:
                    status = MovementStatus.BLOCKED
                    last_obstacle = (nx, ny)
                else:
                    x, y = nx, ny

        return Rover(
            x, y, heading,
            self._grid_width, self._grid_height,
            self._obstacles,
            status, last_obstacle,
        )
