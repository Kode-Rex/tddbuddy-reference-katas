from poker_hands import Card, Hand


class HandBuilder:
    """Two-shape builder: fluent `.with_card(card)` for tests that assemble a hand
    card-by-card, plus the static `HandBuilder.from_string("2H 3D 5S 9C KD")`
    shorthand (delegates to `Hand.from_string`) for hand-level evaluation tests.
    """

    def __init__(self) -> None:
        self._cards: list[Card] = []

    def with_card(self, card: Card) -> "HandBuilder":
        self._cards.append(card)
        return self

    def build(self) -> Hand:
        return Hand(self._cards)

    @staticmethod
    def from_string(shorthand: str) -> Hand:
        return Hand.from_string(shorthand)
