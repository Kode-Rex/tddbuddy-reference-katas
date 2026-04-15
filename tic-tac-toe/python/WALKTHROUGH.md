# Tic-Tac-Toe — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Pure Domain Only

No UI, no CLI, no persistence, no AI opponent. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Python Shape

- **`Cell` and `Outcome` are `Enum` subclasses** with string values (`Cell.EMPTY.value == "Empty"`). This gives identity-based equality (`cell is Cell.X`) which reads well in tests, pretty-printing that matches the byte-identical string values used in C# and TS, and exhaustiveness that mypy can check.
- **`Board` is a plain class with a mutable-under-the-hood grid** defended by a `place` that always returns a fresh `Board`. `@dataclass(frozen=True)` is tempting for symmetry with the `password` reference, but frozen dataclasses with a nested list field do not compose cleanly (the list is still mutable) and the class has a few methods whose presentation suffers from the dataclass-generated `__repr__` showing the whole 3x3 grid on every assertion failure.
- **`BoardMessages` is a plain class with class-level string attributes**. No `StrEnum`, no module-level globals. The strings stay byte-identical to the C# and TypeScript spellings.
- **`Board.from_grid` is a test-folder hook** documented as such. Python has no equivalent of C#'s `InternalsVisibleTo` or TS's `@internal` JSDoc; the convention is the docstring and the class-method naming that signals "constructor alternative."
- **Field and method names use `snake_case`**: `place`, `cell_at`, `current_turn`, `with_x_at`. Rule-name *strings* stay identical across languages even though *method* names differ by convention — the spec is the exception message, not the method name.
- **Identity comparisons (`is`) for `Enum` values** rather than `==`. Both work on enums; `is` matches the "these are singletons" mental model and is faster.

## Why `BoardBuilder` Lives in `tests/`

Same F2 rationale as C#: twelve scenarios need a dozen slightly different boards, and without a builder each test would reach into `Board()` and replay moves. With the builder, setup is a fluent chain that reads as the literal board. The builder is 16 lines — comfortably within the 10–30 line F2 budget (Python has more per-line overhead for `self` and type annotations, so builders here often sit near the top of the range).

`tests/__init__.py` exists so that tests can `from tests.board_builder import BoardBuilder` — mirroring the C# `namespace TicTacToe.Tests` and TS relative import.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_board.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd tic-tac-toe/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
