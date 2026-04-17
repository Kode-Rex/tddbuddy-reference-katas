class InvalidHealthException(Exception):
    def __init__(self, health: int) -> None:
        super().__init__(f"Health must be strictly positive, got {health}")


class InvalidLevelException(Exception):
    def __init__(self, level: int) -> None:
        super().__init__(f"Tower level must be between 1 and 4, got {level}")
