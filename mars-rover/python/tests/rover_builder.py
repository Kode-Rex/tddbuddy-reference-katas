from mars_rover import (
    DEFAULT_GRID_HEIGHT,
    DEFAULT_GRID_WIDTH,
    Direction,
    MovementStatus,
    Rover,
)


class RoverBuilder:
    def __init__(self) -> None:
        self._x = 0
        self._y = 0
        self._heading = Direction.NORTH
        self._grid_width = DEFAULT_GRID_WIDTH
        self._grid_height = DEFAULT_GRID_HEIGHT
        self._obstacles: set[tuple[int, int]] = set()

    def at(self, x: int, y: int) -> "RoverBuilder":
        self._x = x
        self._y = y
        return self

    def facing(self, heading: Direction) -> "RoverBuilder":
        self._heading = heading
        return self

    def on_grid(self, width: int, height: int) -> "RoverBuilder":
        self._grid_width = width
        self._grid_height = height
        return self

    def with_obstacle_at(self, x: int, y: int) -> "RoverBuilder":
        self._obstacles.add((x, y))
        return self

    def build(self) -> Rover:
        return Rover(
            self._x, self._y, self._heading,
            self._grid_width, self._grid_height,
            set(self._obstacles),
            MovementStatus.MOVING, None,
        )
