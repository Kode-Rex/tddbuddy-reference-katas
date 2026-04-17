class FixedRandomSource:
    def __init__(self, value: int) -> None:
        self._value = value

    def next(self, _min_inclusive: int, _max_inclusive: int) -> int:
        return self._value
