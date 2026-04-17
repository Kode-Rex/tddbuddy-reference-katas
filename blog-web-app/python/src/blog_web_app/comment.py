from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime


@dataclass(frozen=True)
class Comment:
    id: int
    author: str
    body: str
    timestamp: datetime
