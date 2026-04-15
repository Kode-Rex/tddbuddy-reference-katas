from collections import Counter
from typing import Iterable, List, Tuple

from .card import Card
from .compare import Compare
from .exceptions import InvalidHandError
from .hand_rank import HandRank
from .rank import Rank
from .suit import Suit

HAND_SIZE = 5

_RANK_CODES = {
    "2": Rank.TWO, "3": Rank.THREE, "4": Rank.FOUR, "5": Rank.FIVE,
    "6": Rank.SIX, "7": Rank.SEVEN, "8": Rank.EIGHT, "9": Rank.NINE,
    "T": Rank.TEN, "J": Rank.JACK, "Q": Rank.QUEEN, "K": Rank.KING, "A": Rank.ACE,
}

_SUIT_CODES = {
    "C": Suit.CLUBS, "D": Suit.DIAMONDS, "H": Suit.HEARTS, "S": Suit.SPADES,
}


class Hand:
    def __init__(self, cards: Iterable[Card]) -> None:
        cards_list = list(cards)
        if len(cards_list) != HAND_SIZE:
            raise InvalidHandError(
                f"A hand must have exactly 5 cards (got {len(cards_list)})"
            )
        self._cards: Tuple[Card, ...] = tuple(cards_list)

    @property
    def cards(self) -> Tuple[Card, ...]:
        return self._cards

    @classmethod
    def from_string(cls, shorthand: str) -> "Hand":
        """Parse a hand from shorthand notation like "2H 3D 5S 9C KD".

        Each token is a two-character card: rank code (2-9, T, J, Q, K, A)
        plus suit code (C, D, H, S).
        """
        tokens = shorthand.split()
        return cls(_parse_card(token) for token in tokens)

    def evaluate(self) -> HandRank:
        groups = _rank_groups_by_count_descending(self._cards)
        counts = [count for _, count in groups]
        is_flush = len({c.suit for c in self._cards}) == 1
        is_straight = _is_straight(self._cards)

        if is_straight and is_flush:
            return HandRank.STRAIGHT_FLUSH
        if counts[0] == 4:
            return HandRank.FOUR_OF_A_KIND
        if counts[0] == 3 and len(counts) > 1 and counts[1] == 2:
            return HandRank.FULL_HOUSE
        if is_flush:
            return HandRank.FLUSH
        if is_straight:
            return HandRank.STRAIGHT
        if counts[0] == 3:
            return HandRank.THREE_OF_A_KIND
        if counts[0] == 2 and len(counts) > 1 and counts[1] == 2:
            return HandRank.TWO_PAIR
        if counts[0] == 2:
            return HandRank.PAIR
        return HandRank.HIGH_CARD

    def compare_to(self, other: "Hand") -> Compare:
        my_rank = self.evaluate()
        their_rank = other.evaluate()
        if my_rank > their_rank:
            return Compare.PLAYER1_WINS
        if my_rank < their_rank:
            return Compare.PLAYER2_WINS

        my_sig = _tie_break_signature(self._cards)
        their_sig = _tie_break_signature(other.cards)
        for mine, theirs in zip(my_sig, their_sig):
            if mine > theirs:
                return Compare.PLAYER1_WINS
            if mine < theirs:
                return Compare.PLAYER2_WINS
        return Compare.TIE


def _parse_card(token: str) -> Card:
    if len(token) != 2:
        raise InvalidHandError(f"Invalid card token '{token}'")
    rank = _RANK_CODES.get(token[0])
    if rank is None:
        raise InvalidHandError(f"Invalid rank code '{token[0]}'")
    suit = _SUIT_CODES.get(token[1])
    if suit is None:
        raise InvalidHandError(f"Invalid suit code '{token[1]}'")
    return Card(rank, suit)


def _rank_groups_by_count_descending(cards: Tuple[Card, ...]) -> List[Tuple[Rank, int]]:
    counts = Counter(c.rank for c in cards)
    return sorted(counts.items(), key=lambda g: (-g[1], -g[0]))


def _tie_break_signature(cards: Tuple[Card, ...]) -> List[Rank]:
    """Canonical tie-break signature: ranks ordered first by group size (descending),
    then by rank (descending). A positional compare correctly implements every
    same-rank tie-breaker rule in SCENARIOS.md.
    """
    signature: List[Rank] = []
    for rank, count in _rank_groups_by_count_descending(cards):
        signature.extend([rank] * count)
    return signature


def _is_straight(cards: Tuple[Card, ...]) -> bool:
    ordered = sorted(int(c.rank) for c in cards)
    return all(ordered[i] == ordered[i - 1] + 1 for i in range(1, len(ordered)))
