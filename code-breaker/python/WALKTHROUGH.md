# Code Breaker — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Feedback Engine Only

Random secret generation, attempt tracking, and the game loop are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## The Python Shape

- **`Peg` is an `IntEnum`** with values `1..6`. `IntEnum` closes the domain at construction time and also gives free equality and hashing — `Peg.ONE in [Peg.ONE, Peg.TWO]` works, `unmatched_secret.remove(peg)` works, without any custom `__eq__`. A plain `Enum` would have been equally closed but less convenient when the scoring algorithm treats pegs as values; an `IntEnum` is the honest, idiomatic choice here.
- **`Secret`, `Guess`, and `Feedback` are `@dataclass(frozen=True)`**. Frozen gives value-equality, immutability, and a useful `repr()` — the same three properties C# gets from `record` and TypeScript gets from `readonly`-everything. Kwargs-only construction (`Secret(pegs=(One, Two, Three, Four))`) keeps call sites from depending on positional ordering as the domain grows.
- **`Secret.__post_init__` enforces length**. The type hint `PegTuple = Tuple[Peg, Peg, Peg, Peg]` is unenforced at runtime in Python, so the dataclass checks `len(pegs) == CODE_LENGTH` on construction. Any `Secret` at rest is well-formed.
- **`Feedback.is_won` is a `@property`** rather than a stored field — one source of truth, derived from the two counts, same discipline as C# and TS.
- **The color-match pass uses `in` + `remove`**, which is `O(n)` in the small unmatched list. For a four-peg code this is trivially fast and reads more like the domain rule than a `Counter`-based version would.

## Why Two Builders Live in `tests/`

Same F2 rationale as C#: twelve scenarios need twelve secret+guess pairs, and without builders each test opens with two raw tuple literals. With `SecretBuilder` and `GuessBuilder`:

```python
secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
guess  = GuessBuilder().with_pegs(ONE, TWO, FOUR, THREE).build()
```

The builder method is `with_pegs` rather than `with` because `with` is a Python keyword. Both builders are roughly twelve lines — well inside the F2 Python budget.

`tests/__init__.py` exists so tests can `from tests.secret_builder import SecretBuilder` — a conventional test-folder builder import, mirroring what C# gets through `namespace CodeBreaker.Tests` and TS gets through relative import.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_feedback.py`, one function per scenario. Scenario 1's `5566` substitutes for the spec's `5678` to respect the 1–6 peg range; the feedback behavior is identical.

## How to Run

```bash
cd code-breaker/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
