from __future__ import annotations

from csv_query import Row


class RowBuilder:
    def __init__(self) -> None:
        self._name = "Alice"
        self._age = "35"
        self._city = "London"
        self._salary = "75000"

    def with_name(self, name: str) -> RowBuilder:
        self._name = name
        return self

    def with_age(self, age: str) -> RowBuilder:
        self._age = age
        return self

    def with_city(self, city: str) -> RowBuilder:
        self._city = city
        return self

    def with_salary(self, salary: str) -> RowBuilder:
        self._salary = salary
        return self

    def build(self) -> Row:
        return Row({
            "name": self._name,
            "age": self._age,
            "city": self._city,
            "salary": self._salary,
        })
