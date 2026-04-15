from __future__ import annotations

from enum import Enum


class CopyStatus(Enum):
    AVAILABLE = "Available"
    CHECKED_OUT = "CheckedOut"
    RESERVED = "Reserved"
