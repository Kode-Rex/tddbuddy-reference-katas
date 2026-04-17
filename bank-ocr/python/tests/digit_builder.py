from __future__ import annotations

_CANONICAL: dict[int, list[str]] = {
    0: [" _ ", "| |", "|_|"],
    1: ["   ", "  |", "  |"],
    2: [" _ ", " _|", "|_ "],
    3: [" _ ", " _|", " _|"],
    4: ["   ", "|_|", "  |"],
    5: [" _ ", "|_ ", " _|"],
    6: [" _ ", "|_ ", "|_|"],
    7: [" _ ", "  |", "  |"],
    8: [" _ ", "|_|", "|_|"],
    9: [" _ ", "|_|", " _|"],
}


class DigitBuilder:
    def __init__(self) -> None:
        self._rows: list[str] = ["   ", "   ", "   "]

    def for_digit(self, value: int) -> "DigitBuilder":
        self._rows = list(_CANONICAL[value])
        return self

    def with_row(self, index: int, row: str) -> "DigitBuilder":
        self._rows[index] = row
        return self

    def build(self) -> list[str]:
        return list(self._rows)
