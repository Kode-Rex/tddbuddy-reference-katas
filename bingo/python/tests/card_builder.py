from bingo import CARD_SIZE, Card, FREE_COLUMN, FREE_ROW


class CardBuilder:
    """Test-folder synthesiser. Places specific numbers at specific coordinates
    without enforcing the B/I/N/G/O column ranges — tests set up the card
    state they need, not the card a real generator would produce.
    """

    def __init__(self) -> None:
        self._numbers = [[None for _ in range(CARD_SIZE)] for _ in range(CARD_SIZE)]
        self._marks = [[False for _ in range(CARD_SIZE)] for _ in range(CARD_SIZE)]
        # Free space is blank and pre-marked on every card.
        self._marks[FREE_ROW][FREE_COLUMN] = True

    def with_number_at(self, row: int, col: int, number: int) -> "CardBuilder":
        self._numbers[row][col] = number
        return self

    def with_mark_at(self, row: int, col: int) -> "CardBuilder":
        self._marks[row][col] = True
        return self

    def build(self) -> Card:
        return Card(self._numbers, self._marks)
