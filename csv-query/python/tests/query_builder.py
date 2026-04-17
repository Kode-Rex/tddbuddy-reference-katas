from __future__ import annotations

from csv_query import Query, parse_csv

DEFAULT_CSV = (
    "name,age,city,salary\n"
    "Alice,35,London,75000\n"
    "Bob,28,Paris,55000\n"
    "Charlie,42,London,90000\n"
    "Diana,31,Berlin,65000\n"
    "Eve,28,Paris,60000"
)


class QueryBuilder:
    def __init__(self) -> None:
        self._csv = DEFAULT_CSV

    def with_csv(self, csv: str) -> QueryBuilder:
        self._csv = csv
        return self

    def build(self) -> Query:
        table = parse_csv(self._csv)
        return Query(table)
