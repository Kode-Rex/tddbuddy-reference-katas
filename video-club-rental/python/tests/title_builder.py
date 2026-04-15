from __future__ import annotations

from video_club_rental import Title


class TitleBuilder:
    def __init__(self) -> None:
        self._name = "The Godfather"
        self._copies = 3

    def named(self, name: str) -> "TitleBuilder":
        self._name = name
        return self

    def with_copies(self, copies: int) -> "TitleBuilder":
        self._copies = copies
        return self

    def build(self) -> Title:
        return Title(self._name, self._copies)
