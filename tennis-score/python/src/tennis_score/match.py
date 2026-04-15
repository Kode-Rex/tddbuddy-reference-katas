from enum import StrEnum


class Score(StrEnum):
    LOVE = "Love"
    FIFTEEN = "Fifteen"
    THIRTY = "Thirty"
    FORTY = "Forty"
    DEUCE = "Deuce"
    ADVANTAGE = "Advantage"
    GAME = "Game"


_WORDS = {
    Score.LOVE: "Love",
    Score.FIFTEEN: "15",
    Score.THIRTY: "30",
    Score.FORTY: "40",
}


def _points_to_score(points: int) -> Score:
    if points == 0:
        return Score.LOVE
    if points == 1:
        return Score.FIFTEEN
    if points == 2:
        return Score.THIRTY
    return Score.FORTY


class Match:
    def __init__(self) -> None:
        self._p1_points = 0
        self._p2_points = 0

    def point_won_by(self, player: int) -> None:
        if player == 1:
            self._p1_points += 1
        else:
            self._p2_points += 1

    def score(self) -> str:
        p1, p2 = self._score_states()

        if p1 is Score.ADVANTAGE:
            return "Advantage Player 1"
        if p2 is Score.ADVANTAGE:
            return "Advantage Player 2"
        if p1 is Score.GAME:
            return "Game Player 1"
        if p2 is Score.GAME:
            return "Game Player 2"
        if p1 is Score.DEUCE:
            return "Deuce"
        return f"{_WORDS[p1]}-{_WORDS[p2]}"

    def _score_states(self) -> tuple[Score, Score]:
        a, b = self._p1_points, self._p2_points
        if a >= 3 and b >= 3:
            if a == b:
                return Score.DEUCE, Score.DEUCE
            if a - b == 1:
                return Score.ADVANTAGE, Score.FORTY
            if b - a == 1:
                return Score.FORTY, Score.ADVANTAGE
            if a > b:
                return Score.GAME, Score.FORTY
            return Score.FORTY, Score.GAME
        if a >= 4:
            return Score.GAME, _points_to_score(b)
        if b >= 4:
            return _points_to_score(a), Score.GAME
        return _points_to_score(a), _points_to_score(b)
