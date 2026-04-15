# Rate Limiter — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md). This file notes what's idiomatic to Python and what deliberately diverges.

## Idiomatic Deltas

- **`Decision` is a `Union[Allowed, Rejected]`** of two `@dataclass(frozen=True)` types. Tests discriminate with `isinstance(decision, Allowed)` or assert equality (`decision == Rejected(timedelta(seconds=7))`). Pattern-matching via `match` statements would also work; the `isinstance` form is most familiar to readers of existing pytest suites.
- **`Rule` uses `@dataclass(frozen=True)` with `__post_init__`** for validation. Frozen dataclasses give value equality and immutability without the `readonly record struct` boilerplate.
- **`Clock` is a `Protocol`**, not an ABC. Structural typing means `FixedClock` implements it by having a compatible `now()` method — no inheritance required.
- **`timedelta` throughout**, not milliseconds. Python's `datetime` library makes durations a first-class concept; converting to an integer unit would be a step down.
- **Per-key state mutates in place** — `state.count += 1` — where the C# version uses `with` to produce a new record. Python has no immutable value structs at the domain-state layer, and the `dict[str, _WindowState]` is already a private implementation detail.
- **Package layout**: `src/rate_limiter/` with `__init__.py` re-exports, `pyproject.toml` using the src-layout `pytest` wiring established in [`memory-cache`](../../memory-cache/python/).

## Shared Design (see C# walkthrough for rationale)

- Fixed-window algorithm, not sliding window.
- `Clock` collaborator + `FixedClock` test double.
- `LimiterBuilder` returns `(Limiter, FixedClock)` tuple.
- `LimiterRuleInvalidError` with byte-identical messages: `"Max requests must be positive"`, `"Window must be positive"`.
- Five test files mirroring the C# split.

## How to Run

```bash
cd rate-limiter/python
python -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
