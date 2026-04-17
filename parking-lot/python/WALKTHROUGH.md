# Parking Lot — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md). This file notes what's idiomatic to Python and what deliberately diverges.

## Idiomatic Deltas

- **`VehicleType` and `SpotType` are `Enum` subclasses**, not string unions. Python's `enum.Enum` gives named constants with identity and iteration — the preference-list mapping uses enum members as keys, and test assertions read `ticket.spot_type == SpotType.MOTORCYCLE`.
- **`Vehicle` and `Ticket` are `@dataclass(frozen=True)`**, giving immutable value semantics with structural equality. `Ticket` equality is used directly in `process_exit` to validate that the presented ticket matches the stored one — no field-by-field comparison needed.
- **`Fee` wraps `Decimal`**, not `float` or `int`. Python's `decimal.Decimal` matches the C# `decimal` precision semantics and avoids floating-point surprises on monetary arithmetic. Test assertions use `Decimal("6")` string construction for exactness.
- **`Clock` is a `Protocol`**, not an ABC. Structural typing means `FixedClock` implements it by having a compatible `now()` method — no inheritance required. Same pattern as [`rate-limiter`](../../rate-limiter/python/WALKTHROUGH.md).
- **`timedelta` throughout**, not milliseconds or seconds as raw numbers. Python's `datetime` library makes durations a first-class concept; `math.ceil(total_seconds / 3600)` converts to billable hours at exit time.
- **Package layout**: `src/parking_lot/` with `__init__.py` re-exports, `pyproject.toml` using the src-layout `pytest` wiring established across the reference katas.

## Shared Design (see C# walkthrough for rationale)

- Smallest-fit-first spot allocation via preference tuples.
- `Clock` collaborator + `FixedClock` test double.
- `LotBuilder` returns `(Lot, FixedClock)` tuple.
- `VehicleBuilder` with `as_motorcycle()` / `as_car()` / `as_bus()` convenience methods.
- Four domain exception types with byte-identical messages.
- Seven test files mirroring the C# split.

## How to Run

```bash
cd parking-lot/python
python -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
