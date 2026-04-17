# Social Network — Python Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Clock` as a Protocol

Python's `typing.Protocol` provides structural subtyping — `FixedClock` satisfies `Clock` without inheriting from it. This is the Python equivalent of C#'s `IClock` interface and TypeScript's `Clock` interface.

### `Post` as a Frozen Dataclass

`@dataclass(frozen=True)` gives structural equality and immutability. Tests can compare `Post` instances directly, and nothing mutates a post after creation.

### `frozenset` for Following

`User.following` returns a `frozenset[str]` — an immutable snapshot of the internal mutable `set`. This prevents callers from accidentally mutating the user's follow list while keeping the property cheap to access.

### `datetime` with Timezone

All timestamps use `datetime` with explicit `timezone.utc`. The builder constructs times with `tzinfo=timezone.utc` so comparisons are timezone-aware and deterministic.

### Package Layout

The `src/social_network/` layout with `pyproject.toml` follows the standard Python packaging convention. `__init__.py` re-exports `Clock`, `Network`, `Post`, and `User` so tests import from the package root.

## Scenario Map

Eighteen scenarios live in `tests/test_network.py`, one `test_` function per scenario. Test names match `SCENARIOS.md` verbatim in snake_case form.

## How to Run

```bash
cd social-network/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
