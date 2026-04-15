from __future__ import annotations

from dataclasses import dataclass
from datetime import timedelta
from typing import Union


@dataclass(frozen=True)
class Allowed:
    pass


@dataclass(frozen=True)
class Rejected:
    retry_after: timedelta


Decision = Union[Allowed, Rejected]
