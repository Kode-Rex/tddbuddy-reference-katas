from dataclasses import dataclass

from .rank import Rank
from .suit import Suit


@dataclass(frozen=True)
class Card:
    rank: Rank
    suit: Suit
