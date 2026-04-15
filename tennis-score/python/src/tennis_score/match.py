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
        self._p1_games = 0
        self._p2_games = 0
        self._p1_sets = 0
        self._p2_sets = 0
        self._game_just_won_by: int | None = None
        self._set_just_won_by: int | None = None
        self._match_winner: int | None = None

    def point_won_by(self, player: int) -> None:
        if self._match_winner is not None:
            return

        self._game_just_won_by = None
        self._set_just_won_by = None

        if player == 1:
            self._p1_points += 1
        else:
            self._p2_points += 1

        p1, p2 = self._score_states()
        if p1 is Score.GAME:
            self._p1_games += 1
            self._p1_points = 0
            self._p2_points = 0
            self._game_just_won_by = 1
        elif p2 is Score.GAME:
            self._p2_games += 1
            self._p1_points = 0
            self._p2_points = 0
            self._game_just_won_by = 2

        if self._p1_games >= 6 and self._p1_games - self._p2_games >= 2:
            self._p1_sets += 1
            self._p1_games = 0
            self._p2_games = 0
            self._set_just_won_by = 1
            self._game_just_won_by = None
        elif self._p2_games >= 6 and self._p2_games - self._p1_games >= 2:
            self._p2_sets += 1
            self._p1_games = 0
            self._p2_games = 0
            self._set_just_won_by = 2
            self._game_just_won_by = None

        if self._p1_sets >= 2:
            self._match_winner = 1
            self._set_just_won_by = None
        elif self._p2_sets >= 2:
            self._match_winner = 2
            self._set_just_won_by = None

    def score(self) -> str:
        if self._match_winner == 1:
            return "Match Player 1"
        if self._match_winner == 2:
            return "Match Player 2"
        if self._set_just_won_by == 1:
            return "Set Player 1"
        if self._set_just_won_by == 2:
            return "Set Player 2"
        if self._game_just_won_by == 1:
            return "Game Player 1"
        if self._game_just_won_by == 2:
            return "Game Player 2"

        p1, p2 = self._score_states()
        if p1 is Score.ADVANTAGE:
            return "Advantage Player 1"
        if p2 is Score.ADVANTAGE:
            return "Advantage Player 2"
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
