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
        if self._p1_points == 0:
            p1_word = "Love"
        elif self._p1_points == 1:
            p1_word = "15"
        elif self._p1_points == 2:
            p1_word = "30"
        else:
            p1_word = "40"

        if self._p2_points == 0:
            p2_word = "Love"
        elif self._p2_points == 1:
            p2_word = "15"
        elif self._p2_points == 2:
            p2_word = "30"
        else:
            p2_word = "40"

        return f"{p1_word}-{p2_word}"
