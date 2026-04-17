# Bank OCR — Python Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Digit` as a Frozen Dataclass

C# uses `readonly record struct`; TS uses a class with `equals`. Python's `@dataclass(frozen=True)` gives value equality and hashability for free — `Digit(3) == Digit(3)` works without custom methods. `Digit.unknown()` is a classmethod that returns `Digit(None)`.

### `Decimal` Not Needed

Unlike the banking/pricing F3 katas, Bank OCR works entirely in integers (digit values 0–9, checksum arithmetic). No `Decimal` import.

### Pattern Lookup via Dictionary

The 3×3 digit patterns are stored as a `dict[str, int]` mapping the concatenated three-row string to its digit value. This is the same shape as C#'s `Dictionary<string, int>` and TS's `Map<string, number>` — Python's dict literal syntax makes it the most compact of the three.

### `AccountNumber` Status

`AccountNumberStatus` is a `StrEnum` with values `"VALID"`, `"ERR"`, `"ILL"` — matching the spec's output format byte-for-byte. C# uses a plain `enum`; TS uses a string-literal union. Python's `StrEnum` gives both the type-safety and the string representation.

## Scenario Map

Twenty-two scenarios live in `tests/test_bank_ocr.py`, one function per scenario, names matching `SCENARIOS.md`.

## How to Run

```bash
cd bank-ocr/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
