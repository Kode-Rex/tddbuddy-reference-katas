from __future__ import annotations

from typing import TYPE_CHECKING, Protocol

if TYPE_CHECKING:
    from .member import Member


class Notifier(Protocol):
    def send(self, member: "Member", message: str) -> None: ...
