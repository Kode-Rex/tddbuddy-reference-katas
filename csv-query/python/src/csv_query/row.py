from __future__ import annotations

from .exceptions import UnknownColumnError


class Row:
    def __init__(self, values: dict[str, str]) -> None:
        self._values = dict(values)

    def get(self, column: str) -> str:
        if column not in self._values:
            raise UnknownColumnError(column)
        return self._values[column]

    @property
    def columns(self) -> list[str]:
        return list(self._values.keys())

    @property
    def values(self) -> dict[str, str]:
        return dict(self._values)

    def project(self, columns: list[str]) -> Row:
        projected = {}
        for col in columns:
            projected[col] = self.get(col)
        return Row(projected)
