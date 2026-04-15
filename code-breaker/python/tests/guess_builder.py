from code_breaker import Guess, Peg


class GuessBuilder:
    def __init__(self) -> None:
        self._pegs = (Peg.ONE, Peg.TWO, Peg.THREE, Peg.FOUR)

    def with_pegs(self, a: Peg, b: Peg, c: Peg, d: Peg) -> "GuessBuilder":
        self._pegs = (a, b, c, d)
        return self

    def build(self) -> Guess:
        return Guess(pegs=self._pegs)
