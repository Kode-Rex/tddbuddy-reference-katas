from __future__ import annotations


class StringSource:
    def __init__(self, payload: str) -> None:
        self._payload = payload
        self._index = 0
        self.read_count = 0

    def read(self) -> str:
        self.read_count += 1
        if self._index >= len(self._payload):
            return "\n"
        ch = self._payload[self._index]
        self._index += 1
        return ch
