# Game of Life — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (set-based infinite grid, immutable Grid, neighbor-count approach in `tick()`, minimal GridBuilder).

This note captures only the Python deltas.

## `Cell` as a Frozen Dataclass

`@dataclass(frozen=True)` gives value equality and hashability for free — the same role as C#'s `readonly record struct`. A `Cell(0, 1) == Cell(0, 1)` is `True`, and cells can be stored in a `frozenset`. The `neighbors()` method lives on `Cell` just as in C#.

See `src/game_of_life/cell.py`.

## `frozenset` Instead of `HashSet`

Python's `frozenset` is immutable by construction — no need for a read-only wrapper. `Grid` stores its living cells as a `frozenset[Cell]`, and `tick()` returns a new `Grid` wrapping a new `frozenset`. Tests compare sets directly with `==`, which is cleaner than the C#/TS equivalents.

## `Counter` Replaces the Neighbor-Count Dictionary

Python's `collections.Counter` is a dictionary subclass purpose-built for counting. The C# version builds a `Dictionary<Cell, int>` manually; Python gets the same shape from `Counter[Cell]` with `+= 1` increments. The algorithm is identical — only the standard-library ergonomics differ.

See `src/game_of_life/grid.py`.

## No Domain Exceptions

Like the C# and TS implementations, GoL has no invariants to reject, so there are no domain exception types.

## Scenario Map

Eighteen scenarios across six test files:

- `tests/test_empty_and_trivial.py` — scenarios 1–2
- `tests/test_individual_rules.py` — scenarios 3–8
- `tests/test_still_lifes.py` — scenarios 9–10
- `tests/test_oscillators.py` — scenarios 11–13
- `tests/test_spaceships.py` — scenario 14
- `tests/test_grid_queries.py` — scenarios 15–18

## How to Run

```bash
cd game-of-life/python
python -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
