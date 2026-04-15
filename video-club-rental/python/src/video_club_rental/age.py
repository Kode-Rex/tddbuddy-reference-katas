from __future__ import annotations

from dataclasses import dataclass

AGE_ADULT_MINIMUM = 18


@dataclass(frozen=True)
class Age:
    years: int

    @property
    def is_adult(self) -> bool:
        return self.years >= AGE_ADULT_MINIMUM
