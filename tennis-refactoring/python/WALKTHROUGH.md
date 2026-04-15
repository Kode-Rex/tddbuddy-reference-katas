# Tennis Refactoring — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Single-Game Scoring Only

No `TennisGame` class, no tiebreak, no set or match tracking. See [`../README.md`](../README.md#scope--single-game-scoring-only) for the full list.

## Relationship to `tennis-score/`

[`../../tennis-score/python/`](../../tennis-score/python/) is the Pedagogy tennis kata; this is the refactoring mirror. Different katas, overlapping outputs. See the [top-level README](../README.md#relationship-to-tennis-score) for which one to pick.

## The Python Shape

- **`get_score` is a module-level function**, not a method on a class. The legacy was a free function; the refactor preserves that shape. No entity has identity here.
- **Three named helpers** — `_equal_score`, `_endgame_score`, `_in_play_score` — live in the same module. Leading underscore marks them as implementation detail; `__init__.py` re-exports only `get_score`. Python idiom: one module, several related functions, a narrow `__all__`.
- **`POINT_NAMES` is a module-level tuple.** Immutable by construction — no `@dataclass(frozen=True)` needed for four string literals. `_point_name(score)` is a helper rather than a direct index so the walkthrough's note about the lookup lives next to the code.
- **Named constants for domain numbers.** `ENDGAME_THRESHOLD = 4` and `DEUCE_THRESHOLD = 3` match the C# / TS constants — the F2 style convention to prefer named business numbers.
- **No builder.** This kata is classified *characterization test set only*. A builder for a four-argument function would be longer than the call site.

## Scenario Map

The sixteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_tennis_scorer.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd tennis-refactoring/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

Expected: **16 passed.**
