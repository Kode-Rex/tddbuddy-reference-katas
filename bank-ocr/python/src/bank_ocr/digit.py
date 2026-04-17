from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class Digit:
    value: int | None

    @classmethod
    def of(cls, value: int) -> Digit:
        if not isinstance(value, int) or value < 0 or value > 9:
            raise ValueError("Digit value must be an integer 0-9.")
        return cls(value)

    @classmethod
    def unknown(cls) -> Digit:
        return cls(None)

    @property
    def is_known(self) -> bool:
        return self.value is not None

    def __str__(self) -> str:
        return str(self.value) if self.is_known else "?"
