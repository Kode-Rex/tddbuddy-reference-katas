from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class Customer:
    email: str
    cell_phone: str
