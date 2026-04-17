class NoStartCellException(Exception):
    def __init__(self) -> None:
        super().__init__("Maze must have exactly one start cell.")


class NoEndCellException(Exception):
    def __init__(self) -> None:
        super().__init__("Maze must have exactly one end cell.")


class MultipleStartCellsException(Exception):
    def __init__(self) -> None:
        super().__init__("Maze must have exactly one start cell.")


class MultipleEndCellsException(Exception):
    def __init__(self) -> None:
        super().__init__("Maze must have exactly one end cell.")
