from __future__ import annotations


class User:
    def __init__(self, name: str) -> None:
        self.name = name
        self._following: set[str] = set()

    @property
    def following(self) -> frozenset[str]:
        return frozenset(self._following)

    def follow(self, user_name: str) -> bool:
        if user_name == self.name:
            return False
        if user_name in self._following:
            return False
        self._following.add(user_name)
        return True
