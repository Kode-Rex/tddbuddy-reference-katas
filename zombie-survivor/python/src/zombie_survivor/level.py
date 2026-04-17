from enum import Enum


class Level(Enum):
    BLUE = "Blue"
    YELLOW = "Yellow"
    ORANGE = "Orange"
    RED = "Red"


_LEVEL_ORDER = {Level.BLUE: 0, Level.YELLOW: 1, Level.ORANGE: 2, Level.RED: 3}


def level_for(xp: int) -> Level:
    if xp >= 43:
        return Level.RED
    if xp >= 19:
        return Level.ORANGE
    if xp >= 7:
        return Level.YELLOW
    return Level.BLUE


def max_level(a: Level, b: Level) -> Level:
    return a if _LEVEL_ORDER[a] >= _LEVEL_ORDER[b] else b
