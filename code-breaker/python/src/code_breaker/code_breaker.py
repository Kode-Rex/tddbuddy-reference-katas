from dataclasses import dataclass
from enum import IntEnum
from typing import List, Tuple


class Peg(IntEnum):
    """The six playable values in a Mastermind-style code.

    Modeled as an ``IntEnum`` so invalid pegs cannot be constructed while
    equality, hashing, and list membership all behave like the underlying
    integer — which is what the scoring algorithm wants.
    """

    ONE = 1
    TWO = 2
    THREE = 3
    FOUR = 4
    FIVE = 5
    SIX = 6


CODE_LENGTH = 4


PegTuple = Tuple[Peg, Peg, Peg, Peg]


@dataclass(frozen=True)
class Secret:
    pegs: PegTuple

    def __post_init__(self) -> None:
        if len(self.pegs) != CODE_LENGTH:
            raise ValueError(f"Secret must have exactly {CODE_LENGTH} pegs.")

    def score_against(self, guess: "Guess") -> "Feedback":
        return _compute_feedback(self.pegs, guess.pegs)


@dataclass(frozen=True)
class Guess:
    pegs: PegTuple

    def __post_init__(self) -> None:
        if len(self.pegs) != CODE_LENGTH:
            raise ValueError(f"Guess must have exactly {CODE_LENGTH} pegs.")


@dataclass(frozen=True)
class Feedback:
    exact_matches: int
    color_matches: int

    @property
    def is_won(self) -> bool:
        return self.exact_matches == CODE_LENGTH

    def render(self) -> str:
        return "+" * self.exact_matches + "-" * self.color_matches

    def __str__(self) -> str:
        return self.render()


def _compute_feedback(secret: PegTuple, guess: PegTuple) -> Feedback:
    """Two-pass scoring.

    Exact matches consume positions first; remaining guess pegs then pair
    with remaining secret pegs by value, each secret peg consumed at most
    once so duplicates never double-count.
    """
    exact_matches = 0
    unmatched_secret: List[Peg] = []
    unmatched_guess: List[Peg] = []

    for i in range(CODE_LENGTH):
        if secret[i] == guess[i]:
            exact_matches += 1
        else:
            unmatched_secret.append(secret[i])
            unmatched_guess.append(guess[i])

    color_matches = 0
    for peg in unmatched_guess:
        if peg in unmatched_secret:
            unmatched_secret.remove(peg)
            color_matches += 1

    return Feedback(exact_matches=exact_matches, color_matches=color_matches)
