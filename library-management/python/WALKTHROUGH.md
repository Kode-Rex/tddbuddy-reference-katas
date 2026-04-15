# Library Management — Python Walkthrough

Ships in **middle gear** — full implementation in one commit. The [C# walkthrough](../csharp/WALKTHROUGH.md) is the primary design-rationale document. This note captures the Python-specific adaptations.

## Same Design, Python Idioms

```
Library ──owns──> Book[] (title, author, Isbn, copies)
    │              └── Copy (id, status: CopyStatus)
    ├──owns──> Member[]
    ├──owns──> Loan[]        (member, copy, borrowed_on, due_on)
    ├──owns──> Reservation[] (member, isbn, reserved_on, notified_on)
    ├──collab──> Clock       — structural Protocol
    └──collab──> Notifier    — structural Protocol
```

## `Protocol` for Collaborators

`Clock` and `Notifier` are `typing.Protocol` types. Nothing explicitly implements them — `FixedClock` and `RecordingNotifier` satisfy the shape and Python's type checker accepts them. This is the Python idiom for dependency inversion: structural typing, no inheritance ceremony.

See `src/library_management/clock.py`, `notifier.py`.

## `Decimal` for `Money`

Fines accumulate over days. Binary floats drift; `Decimal` doesn't. `Money` wraps a `Decimal` in a `@dataclass(frozen=True)` so equality works out of the box — `Money("0.10") == Money("0.10")` — and `FINE_PER_DAY * 10 == Money("1.00")` is exact.

See `src/library_management/money.py`.

## Frozen Dataclasses for Value Types, Plain Classes for Entities

`Isbn` is a `@dataclass(frozen=True)` — it's a value, two ISBNs with the same string are equal. `Money` is the same shape.

`Book`, `Copy`, `Member`, `Loan`, `Reservation` are plain classes with mutable state. `Copy.status` transitions; `Loan.returned_on` is set on close; `Reservation.notified_on` is set when the queue head is notified. Freezing them would force the aggregate to allocate a new instance on every state transition — the wrong idiom for entities that have identity over time.

## Module-Level SCREAMING_CASE Constants

`LOAN_PERIOD_DAYS`, `FINE_PER_DAY`, `RESERVATION_EXPIRY_DAYS` live at module scope, not as class attributes. Tests import them so `clock.advance_days(LOAN_PERIOD_DAYS + 1)` reads as intent, not magic number.

`__init__.py` re-exports the public surface so tests can `from library_management import ...` without reaching into submodules.

## Scenario Map

- `tests/test_books.py` — scenarios 1–5
- `tests/test_members.py` — scenarios 6–7
- `tests/test_checkouts.py` — scenarios 8–10
- `tests/test_returns.py` — scenarios 11–15
- `tests/test_reservations.py` — scenarios 16–20

One `def test_*` per scenario.

## How to Run

```bash
cd library-management/python
pip install -e '.[dev]'
pytest
```
