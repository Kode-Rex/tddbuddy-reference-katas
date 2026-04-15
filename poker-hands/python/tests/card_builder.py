from poker_hands import Card, Rank, Suit


class CardBuilder:
    def __init__(self) -> None:
        self._rank: Rank = Rank.TWO
        self._suit: Suit = Suit.CLUBS

    def of_rank(self, rank: Rank) -> "CardBuilder":
        self._rank = rank
        return self

    def of_suit(self, suit: Suit) -> "CardBuilder":
        self._suit = suit
        return self

    def build(self) -> Card:
        return Card(self._rank, self._suit)
