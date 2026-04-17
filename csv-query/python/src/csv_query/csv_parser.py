from __future__ import annotations

from .csv_table import CsvTable
from .row import Row


def parse_csv(csv: str) -> CsvTable:
    lines = [line for line in csv.split("\n") if line.strip()]
    if not lines:
        return CsvTable([], [])

    headers = _parse_fields(lines[0])
    rows: list[Row] = []

    for line in lines[1:]:
        fields = _parse_fields(line)
        values: dict[str, str] = {}
        for j, header in enumerate(headers):
            values[header] = fields[j] if j < len(fields) else ""
        rows.append(Row(values))

    return CsvTable(headers, rows)


def _parse_fields(line: str) -> list[str]:
    fields: list[str] = []
    current = ""
    in_quotes = False

    for ch in line:
        if ch == '"':
            in_quotes = not in_quotes
        elif ch == "," and not in_quotes:
            fields.append(current.strip())
            current = ""
        else:
            current += ch

    fields.append(current.strip())
    return fields
