from enum import Enum


class CellType(Enum):
    """The kind of cell in a maze grid."""

    OPEN = "Open"
    WALL = "Wall"
    START = "Start"
    END = "End"
