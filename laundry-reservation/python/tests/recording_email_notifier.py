from __future__ import annotations

from dataclasses import dataclass


@dataclass
class EmailMessage:
    to: str
    subject: str
    body: str


class RecordingEmailNotifier:
    def __init__(self) -> None:
        self.sent: list[EmailMessage] = []

    def send(self, to: str, subject: str, body: str) -> None:
        self.sent.append(EmailMessage(to, subject, body))
