from code_breaker import Peg, Secret


class SecretBuilder:
    def __init__(self) -> None:
        self._pegs = (Peg.ONE, Peg.TWO, Peg.THREE, Peg.FOUR)

    def with_pegs(self, a: Peg, b: Peg, c: Peg, d: Peg) -> "SecretBuilder":
        self._pegs = (a, b, c, d)
        return self

    def build(self) -> Secret:
        return Secret(pegs=self._pegs)
