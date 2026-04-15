class Match:
    def __init__(self) -> None:
        self._p1_points = 0

    def point_won_by(self, player: int) -> None:
        if player == 1:
            self._p1_points += 1

    def score(self) -> str:
        if self._p1_points == 1:
            return "15-Love"
        return "Love-Love"
