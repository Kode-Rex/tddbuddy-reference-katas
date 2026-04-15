from __future__ import annotations


class RecordingDestination:
    def __init__(self) -> None:
        self._buffer: list[str] = []

    def write(self, ch: str) -> None:
        self._buffer.append(ch)

    @property
    def written(self) -> str:
        return "".join(self._buffer)
