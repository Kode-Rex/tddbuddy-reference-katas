from __future__ import annotations

from typing import Sequence

from .account_number import AccountNumber
from .constants import AccountLength, DigitHeight, DigitWidth, RowWidth
from .digit import Digit
from .exceptions import InvalidAccountNumberFormatException

# Canonical glyphs: each entry is the 9-char flattened 3x3 grid (top | middle | bottom).
_GLYPHS: dict[str, int] = {
    " _ " + "| |" + "|_|": 0,
    "   " + "  |" + "  |": 1,
    " _ " + " _|" + "|_ ": 2,
    " _ " + " _|" + " _|": 3,
    "   " + "|_|" + "  |": 4,
    " _ " + "|_ " + " _|": 5,
    " _ " + "|_ " + "|_|": 6,
    " _ " + "  |" + "  |": 7,
    " _ " + "|_|" + "|_|": 8,
    " _ " + "|_|" + " _|": 9,
}


def parse_digit(three_row_block: Sequence[str]) -> Digit:
    if len(three_row_block) != DigitHeight:
        raise InvalidAccountNumberFormatException(
            f"Digit block must have exactly {DigitHeight} rows."
        )
    for row in three_row_block:
        if len(row) != DigitWidth:
            raise InvalidAccountNumberFormatException(
                f"Digit block rows must be exactly {DigitWidth} characters wide."
            )
    key = three_row_block[0] + three_row_block[1] + three_row_block[2]
    value = _GLYPHS.get(key)
    return Digit.unknown() if value is None else Digit.of(value)


def parse_account_number(three_rows: Sequence[str]) -> AccountNumber:
    if len(three_rows) != DigitHeight:
        raise InvalidAccountNumberFormatException(
            f"OCR block must have exactly {DigitHeight} rows."
        )
    for row in three_rows:
        if len(row) != RowWidth:
            raise InvalidAccountNumberFormatException(
                f"OCR block rows must be exactly {RowWidth} characters wide."
            )
    digits: list[Digit] = []
    for i in range(AccountLength):
        start = i * DigitWidth
        block = [
            three_rows[0][start:start + DigitWidth],
            three_rows[1][start:start + DigitWidth],
            three_rows[2][start:start + DigitWidth],
        ]
        digits.append(parse_digit(block))
    return AccountNumber(digits)
