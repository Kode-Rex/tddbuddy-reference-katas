from __future__ import annotations

from bank_ocr import AccountLength, DigitHeight

from .digit_builder import DigitBuilder


class AccountNumberBuilder:
    def __init__(self) -> None:
        self._digits: list[list[str]] = [
            DigitBuilder().for_digit(0).build() for _ in range(AccountLength)
        ]

    def from_string(self, digits: str) -> "AccountNumberBuilder":
        if len(digits) != AccountLength:
            raise ValueError(
                f"Expected {AccountLength} digits, got {len(digits)}."
            )
        self._digits = [
            DigitBuilder().for_digit(int(ch)).build() for ch in digits
        ]
        return self

    def with_digit_at(self, position: int, glyph: list[str]) -> "AccountNumberBuilder":
        self._digits[position] = list(glyph)
        return self

    def build_rows(self) -> list[str]:
        rows: list[str] = []
        for r in range(DigitHeight):
            rows.append("".join(d[r] for d in self._digits))
        return rows
