from __future__ import annotations

from typing import TYPE_CHECKING, Protocol

if TYPE_CHECKING:
    from .user import User


class Notifier(Protocol):
    def send(self, user: "User", message: str) -> None: ...
