def _score_word(points: int) -> str:
    if points == 0:
        return "Love"
    if points == 1:
        return "15"
    if points == 2:
        return "30"
    return "40"


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
        if self._p1_points == 3 and self._p2_points == 3:
            return "Deuce"
        return f"{_score_word(self._p1_points)}-{_score_word(self._p2_points)}"
