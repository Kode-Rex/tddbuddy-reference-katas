from __future__ import annotations

from dataclasses import dataclass

from video_club_rental import User


@dataclass
class Notification:
    user: User
    message: str


class RecordingNotifier:
    def __init__(self) -> None:
        self._sent: list[Notification] = []

    @property
    def sent(self) -> list[Notification]:
        return list(self._sent)

    def send(self, user: User, message: str) -> None:
        self._sent.append(Notification(user, message))

    def notifications_for(self, user: User) -> list[Notification]:
        return [n for n in self._sent if n.user is user]
