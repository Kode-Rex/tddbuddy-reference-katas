from __future__ import annotations

from .exceptions import UnknownColumnError
from .row import Row


class CsvTable:
    def __init__(self, headers: list[str], rows: list[Row]) -> None:
        self.headers = headers
        self.rows = list(rows)

    def validate_column(self, column: str) -> None:
        if column not in self.headers:
            raise UnknownColumnError(column)
