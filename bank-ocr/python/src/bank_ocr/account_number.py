from __future__ import annotations

from dataclasses import dataclass, field
from typing import Iterable

from .constants import AccountLength
from .digit import Digit


@dataclass(frozen=True)
class AccountNumber:
    digits: tuple[Digit, ...] = field(default_factory=tuple)

    def __init__(self, digits: Iterable[Digit]) -> None:
        tup = tuple(digits)
        if len(tup) != AccountLength:
            raise ValueError(
                f"Account number must have exactly {AccountLength} digits."
            )
        object.__setattr__(self, "digits", tup)

    @property
    def is_legible(self) -> bool:
        return all(d.is_known for d in self.digits)

    @property
    def is_checksum_valid(self) -> bool:
        if not self.is_legible:
            return False
        total = 0
        for i, d in enumerate(self.digits):
            position = AccountLength - i  # d1 -> 9, d9 -> 1
            assert d.value is not None
            total += d.value * position
        return total % 11 == 0

    @property
    def number(self) -> str:
        return "".join(str(d) for d in self.digits)

    @property
    def status(self) -> str:
        if not self.is_legible:
            return f"{self.number} ILL"
        if not self.is_checksum_valid:
            return f"{self.number} ERR"
        return self.number
