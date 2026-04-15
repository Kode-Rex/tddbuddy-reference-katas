from tic_tac_toe import BOARD_SIZE, Board, Cell


class BoardBuilder:
    def __init__(self) -> None:
        self._grid = [[Cell.EMPTY for _ in range(BOARD_SIZE)] for _ in range(BOARD_SIZE)]

    def with_x_at(self, row: int, col: int) -> "BoardBuilder":
        self._grid[row][col] = Cell.X
        return self

    def with_o_at(self, row: int, col: int) -> "BoardBuilder":
        self._grid[row][col] = Cell.O
        return self

    def build(self) -> Board:
        return Board.from_grid(self._grid)
