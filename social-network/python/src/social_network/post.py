from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime


@dataclass(frozen=True)
class Post:
    author: str
    content: str
    timestamp: datetime
