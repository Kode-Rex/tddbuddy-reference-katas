# Poker Hands — Python Walkthrough

This walkthrough is a **delta** from the [C# walkthrough](../csharp/WALKTHROUGH.md). The design is the same — read that document first for the rationale behind `Card`/`Rank`/`Suit` as types, the 5-card invariant, the two-builder shape, and the canonical tie-break signature. This file covers what's idiomatic to Python.

## Same Design, Different Idiom

- **`Rank` and `HandRank` are `IntEnum`** — so `Rank.ACE > Rank.KING` and `HandRank.FLUSH > HandRank.STRAIGHT` just work. `IntEnum` is the direct Python equivalent of a backed C# enum; a plain `Enum` would need a `__lt__` override to be ordered.
- **`Suit` and `Compare` are `Enum`** — unordered by design. String values make debug output readable.
- **`Card` is a `@dataclass(frozen=True)`** — immutable, value-equal on `rank` and `suit`, hashable. This is the natural Python shape for a pure value type.
- **`Hand` is a regular class** because constructor validation is the Pythonic way to enforce the 5-card invariant. Internal storage is a `tuple[Card, ...]` so the cards can't be mutated after construction.
- **`InvalidHandError` extends `Exception`** — domain-specific exception type per F3 convention. Message string `"A hand must have exactly 5 cards (got N)"` is byte-identical to the C# and TypeScript versions.

## Package Layout

`src/poker_hands/` is the installable package. `pyproject.toml` declares it via `setuptools.find`; tests import as `from poker_hands import Hand, Rank, ...`. The package's `__init__.py` re-exports every public symbol so callers never reach into submodules.

Tests live outside the package in `tests/`, with their own builders (`tests/card_builder.py`, `tests/hand_builder.py`) imported as sibling modules. Builders don't ship with the library — they're test infrastructure.

## Builders

- **`CardBuilder`** — `.of_rank(Rank.ACE).of_suit(Suit.SPADES).build()`. Fluent, defaults to Two of Clubs.
- **`HandBuilder`** — fluent `.with_card(card)` accumulator plus `HandBuilder.from_string("2H 3D 5S 9C KD")` static that delegates to `Hand.from_string`. Same two-shape decision as the C# and TypeScript implementations.

`with_card` rather than `with` because `with` is a Python keyword.

## Scenario Map

Twenty scenarios across five `test_*.py` files in `tests/`:

- `test_hand_construction.py` — scenarios 1–3
- `test_hand_ranking.py` — scenarios 4–12
- `test_hand_comparison.py` — scenarios 13–15
- `test_tie_breakers.py` — scenarios 16–19
- `test_ties.py` — scenario 20

## How to Run

```bash
cd poker-hands/python
python3 -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
