from __future__ import annotations

from dataclasses import dataclass


@dataclass
class SmsMessage:
    to: str
    message: str


class RecordingSmsNotifier:
    def __init__(self) -> None:
        self.sent: list[SmsMessage] = []

    def send(self, to: str, message: str) -> None:
        self.sent.append(SmsMessage(to, message))
