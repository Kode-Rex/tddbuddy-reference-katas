# Laundry Reservation — Python Walkthrough

Ships in **middle gear** — full implementation in one commit. The [C# walkthrough](../csharp/WALKTHROUGH.md) is the primary design-rationale document. This note captures the Python-specific adaptations.

## Same Design, Python Idioms

```
ReservationService
  ├── create_reservation(slot, customer) → Reservation
  └── claim_reservation(machine_number, pin) → bool

MachineApi
  ├── lock(reservation_id, machine_number, slot, pin) → bool
  └── unlock(machine_number, reservation_id)

Collaborators (Protocols):
  ReservationRepository, EmailNotifier, SmsNotifier,
  PinGenerator, MachineSelector, MachineDevice
```

## Protocol for Structural Typing

All six collaborator contracts use `typing.Protocol` rather than abstract base classes. This gives structural subtyping: the recording fakes in `tests/` satisfy the protocol without inheriting from it. No `ABC`, no `@abstractmethod`, no registration.

This is the Python equivalent of C#'s `interface` and TypeScript's `interface`, but enforced at type-check time (mypy/pyright) rather than at runtime.

## `@dataclass(frozen=True)` for Value Types

`Customer` is a frozen dataclass — immutable and equality-by-value. This matches C#'s `readonly record struct Customer` and TypeScript's `readonly` constructor parameters.

## `uuid.uuid4()` for Reservation IDs

Python's standard library `uuid` module generates reservation IDs via `str(uuid.uuid4())`. No external dependency needed — equivalent to C#'s `Guid.NewGuid()` and Node's `crypto.randomUUID()`.

## PIN Formatting

PINs are formatted with `f"{pin:05d}"` — Python's zero-padded integer formatting. The SMS message `"Your new Wunda Wash PIN is 67890."` is byte-identical across all three languages.

## Scenario Map

- `tests/test_create_reservation.py` — scenarios 1–7
- `tests/test_machine_api.py` — scenarios 8–12
- `tests/test_claim_reservation.py` — scenarios 13–21

One `def test_...` per scenario.

## How to Run

```bash
cd laundry-reservation/python
python -m venv .venv
source .venv/bin/activate
pip install -e ".[dev]"
pytest
```
