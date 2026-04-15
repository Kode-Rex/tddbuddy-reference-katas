# Clam Card — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Pure Domain Only

Daily per-zone cap only. No weekly/monthly caps, no return-journey discount, no bank-account collaborator. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Python Shape

- **`Zone` is an `Enum`.** Python's `Enum` gives name/value distinction and first-class identity comparisons; `zone == Zone.B` reads the same as the spec. A `Literal["A", "B"]` type alias would work but discards the type itself as a runtime value — no `isinstance(zone, Zone)` for callers to rely on.
- **`Ride` is a `@dataclass(frozen=True)`.** Immutable, structural equality, cheap to construct. The fields are snake-cased (`from_station`, `to_station`) because `from` is a reserved word. Structural equality makes `card.rides() == [expected_b, expected_a]` work as expected.
- **`UnknownStationError` subclasses `ValueError`.** Python idiom: invalid-argument errors inherit from `ValueError`; tests still catch the specific subclass.
- **`Decimal` for money.** Fare constants are `Decimal("2.50")` et al. Python floats would be fine here too (all sums of `.00` and `.50` are exact in IEEE 754), but `Decimal` is the Python-idiomatic choice for currency and keeps the door open for weekly/monthly cap math at odd cent amounts.
- **`JourneyStart` is a small dataclass** with a reference back to the `Card`. Python has no C#-style nested-class idiom; a module-level class with a `_` prefix on the card reference carries the same "this is a two-step fluent chain" signal.
- **Two parallel running totals on the card** (`_charged_zone_a_today`, `_charged_zone_b_today`). Same model as C# and TS. `total_charged()` is the sum.
- **Method names use `snake_case`**: `travel_from`, `total_charged`, `rides`, `with_zone`, `on_day`. Rule-name *strings* (the exception message) stay byte-identical across C#, TS, and Python.

## Why the Builders Live in `tests/card_builder.py`

Same F2 rationale as C#: tests read as the card and rides under test, not as a call sequence that happens to produce the right state. `CardBuilder` stages the zone→stations map (`with_zone`) and accepts an `on_day` date for scenario readability (no behaviour at F2 scope). `RideBuilder` synthesises `Ride` literals for the equality scenario — the test asserts the card's ride history against builder-constructed ride literals.

`CardBuilder` lands at ~22 lines, `RideBuilder` at ~30 — comfortably inside the 30–40 line Python F2 budget. The `tests/__init__.py` marker exists so tests can `from tests.card_builder import ...`.

## Why `from_station` Instead of `from`

`from` is a Python keyword. `Ride.from_station` and `RideBuilder.from_station(...)` are the idiomatic spellings. The *spec* vocabulary stays "from"; the code maps to a legal name.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_card.py`, one function per scenario.

## How to Run

```bash
cd clam-card/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
