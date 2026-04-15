from enum import StrEnum


class Play(StrEnum):
    ROCK = "rock"
    PAPER = "paper"
    SCISSORS = "scissors"


class Outcome(StrEnum):
    WIN = "win"
    LOSE = "lose"
    DRAW = "draw"


_BEATS: dict[Play, Play] = {
    Play.ROCK: Play.SCISSORS,
    Play.SCISSORS: Play.PAPER,
    Play.PAPER: Play.ROCK,
}


def decide(p1: Play, p2: Play) -> Outcome:
    if p1 == p2:
        return Outcome.DRAW
    return Outcome.WIN if _BEATS[p1] == p2 else Outcome.LOSE
