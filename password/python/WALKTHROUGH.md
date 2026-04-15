# Password — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Policy Only

Credential store, password repository, email reset, token expiry, and password history are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list. This reference is scoped to **policy validation only**.

## The Python Shape

- **`Policy` and `ValidationResult` are `@dataclass(frozen=True)`**. Frozen gives value-equality, immutability, and a useful `repr()` for test-failure diagnostics — the same three properties C# got from `record` and TypeScript got from `readonly`-everything. Kwargs-only construction (`Policy(min_length=8, requires_digit=True)`) keeps call sites readable without positional-argument guesswork.
- **`RuleNames` is a plain class with class-level string attributes**. No `StrEnum`, no module-level constants scattered across files — one small namespace groups the five rule-name spellings and nothing else. The strings stay byte-identical to the C# `RuleNames` and TypeScript `RuleNames` values.
- **Character classes are `any(_is_xxx(c) for c in password)` with small predicate functions** rather than regex. Both are idiomatic Python; predicates read slightly closer to the rule text ("does the password contain any digit?") and avoid importing `re` for five one-shot checks. The definition of "symbol" stays consistent across languages: *anything that is not ASCII letter and not ASCII digit.*
- **Field names use `snake_case`**: `min_length`, `requires_digit`. The **rule-name strings are identical** across languages even though the *field* names differ by convention — the spec is the string, not the field.

## Why `PolicyBuilder` Lives in `tests/`

Same F2 rationale as C#: scenarios need many tiny policy variations, and without a builder each test opens with a five-keyword-argument `Policy(...)` call where four arguments default to `False`. With the builder, setup is one fluent line that names the variation. The builder is 30 lines in Python — at the upper edge of the 10–30 line F2 budget; Python's lack of a compact fluent-setter syntax costs about ten lines over the C# version, and that is the honest idiomatic shape.

`tests/__init__.py` exists so that tests can `from tests.policy_builder import PolicyBuilder` — a conventional test-folder builder import, mirroring what C# gets through `namespace Password.Tests` and TS gets through relative import.

## Scenario Map

The ten scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_policy.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd password/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
