from __future__ import annotations

from itertools import count

from library_management import Member

_next_id = count(1000)


class MemberBuilder:
    def __init__(self) -> None:
        self._id = next(_next_id)
        self._name = "Alex Reader"

    def named(self, name: str) -> "MemberBuilder":
        self._name = name
        return self

    def with_id(self, id: int) -> "MemberBuilder":
        self._id = id
        return self

    def build(self) -> Member:
        return Member(self._id, self._name)
