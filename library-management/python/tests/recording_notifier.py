from __future__ import annotations

from dataclasses import dataclass

from library_management import Member


@dataclass
class Notification:
    member: Member
    message: str


class RecordingNotifier:
    def __init__(self) -> None:
        self._sent: list[Notification] = []

    @property
    def sent(self) -> list[Notification]:
        return list(self._sent)

    def send(self, member: Member, message: str) -> None:
        self._sent.append(Notification(member, message))

    def notifications_for(self, member: Member) -> list[Notification]:
        return [n for n in self._sent if n.member is member]

    def availability_notifications_for(self, member: Member) -> list[Notification]:
        return [n for n in self.notifications_for(member) if "available" in n.message]

    def expiration_notifications_for(self, member: Member) -> list[Notification]:
        return [n for n in self.notifications_for(member) if "expired" in n.message]
