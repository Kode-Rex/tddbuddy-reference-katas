from __future__ import annotations

from typing import Protocol


class SmsNotifier(Protocol):
    def send(self, to: str, message: str) -> None: ...
