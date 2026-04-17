from __future__ import annotations

from .csv_table import CsvTable
from .row import Row


def _is_numeric(value: str) -> bool:
    try:
        float(value)
        return True
    except ValueError:
        return False


def _evaluate(cell_value: str, op: str, filter_value: str) -> bool:
    if _is_numeric(cell_value) and _is_numeric(filter_value):
        cell = float(cell_value)
        filt = float(filter_value)
        return {
            "=": cell == filt,
            "!=": cell != filt,
            ">": cell > filt,
            "<": cell < filt,
            ">=": cell >= filt,
            "<=": cell <= filt,
        }.get(op, False)

    cmp = (cell_value > filter_value) - (cell_value < filter_value)
    return {
        "=": cmp == 0,
        "!=": cmp != 0,
        ">": cmp > 0,
        "<": cmp < 0,
        ">=": cmp >= 0,
        "<=": cmp <= 0,
    }.get(op, False)


class Query:
    def __init__(self, table: CsvTable) -> None:
        self._table = table
        self._rows: list[Row] = list(table.rows)

    def select(self, *columns: str) -> Query:
        for col in columns:
            self._table.validate_column(col)
        self._rows = [row.project(list(columns)) for row in self._rows]
        return self

    def where(self, column: str, op: str, value: str) -> Query:
        self._table.validate_column(column)
        self._rows = [r for r in self._rows if _evaluate(r.get(column), op, value)]
        return self

    def order_by(self, column: str, direction: str) -> Query:
        self._table.validate_column(column)
        ascending = direction.lower() == "asc"

        def sort_key(row: Row) -> tuple[int, float | str]:
            val = row.get(column)
            if _is_numeric(val):
                return (0, float(val))
            return (1, val)

        self._rows.sort(key=sort_key, reverse=not ascending)
        return self

    def limit(self, n: int) -> Query:
        self._rows = self._rows[:n]
        return self

    def count(self) -> int:
        return len(self._rows)

    @property
    def rows(self) -> list[Row]:
        return list(self._rows)
