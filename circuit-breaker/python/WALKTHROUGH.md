# Circuit Breaker — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (operation as callable, enum-based state machine, injected clock, lazy Open→HalfOpen transition, named domain exceptions).

This note captures only the Python deltas.

## `Clock` as a `Protocol`

The C# version uses an interface; TS uses a structural interface. Python uses `typing.Protocol` — structural typing without a runtime `isinstance` check. `FixedClock` in the tests doesn't inherit from `Clock`; it just exposes `now() -> datetime`, and the type checker accepts it.

See `src/circuit_breaker/clock.py`.

## `BreakerState` as an `Enum` with String Values

`enum.Enum` is the Pythonic spelling of "three named states." String values give debug-time readable `repr`, and `is` comparisons on enum members are the idiomatic identity check (`breaker.state is BreakerState.CLOSED`). The member names follow `UPPER_SNAKE_CASE` per PEP 8; the string values match the cross-language `"Closed" / "Open" / "HalfOpen"` canon.

See `src/circuit_breaker/breaker_state.py`.

## `timedelta` and `datetime`

Reset timeout is a `timedelta`; open-time is a `datetime`. The builder defaults to `datetime(2026, 1, 1, tzinfo=timezone.utc)` so the tests never construct naive-local times — timeout comparisons are pure UTC arithmetic. The domain constant `DEFAULT_RESET_TIMEOUT = timedelta(seconds=30)` lives alongside `DEFAULT_FAILURE_THRESHOLD = 5`.

## `execute(operation: Callable[[], T]) -> T`

`Callable[[], T]` is the type-system equivalent of C#'s `Func<T>` — a zero-arg function returning `T`. The generic flows through so `breaker.execute(lambda: client.fetch(id))` preserves the caller's type. Failure handling uses `try: … except BaseException: raise` to count the failure and re-raise the original exception without wrapping it — the stack trace points to the operation, not the breaker.

(Catching `BaseException` rather than `Exception` is deliberate: the kata's contract is *any* failure of the call is a circuit failure. In a production system a caller might scope it to `Exception` to leave `KeyboardInterrupt` alone; the reference uses the wider net to stay honest to "the breaker observes the call.")

See `src/circuit_breaker/breaker.py`.

## Domain Error Classes

`BreakerThresholdInvalidError`, `BreakerTimeoutInvalidError`, and `CircuitOpenError` subclass `Exception`, not `ValueError` or `RuntimeError`. The Full-Bake F3 convention is to name the rejection rather than lean on a general-purpose type. The messages — `"Failure threshold must be positive"`, `"Reset timeout must be positive"`, `"Circuit is open"` — are byte-identical to the C# and TypeScript implementations.

See `src/circuit_breaker/exceptions.py`.

## Scenario Map

Twenty scenarios across five test files:

- `tests/test_construction.py` — scenarios 1–3
- `tests/test_closed_state.py` — scenarios 4–9
- `tests/test_open_state.py` — scenarios 10–12
- `tests/test_transitions.py` — scenarios 13–18
- `tests/test_round_trip.py` — scenarios 19–20

## How to Run

```bash
cd circuit-breaker/python
python -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
