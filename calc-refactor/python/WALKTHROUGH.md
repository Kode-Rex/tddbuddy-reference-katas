# Calc Refactor — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Calculator Core Only

WPF UI, memory buttons, decimals, floating-point arithmetic are **out of scope**. See [`../README.md`](../README.md#scope--calculator-core-only) for the full list.

## The Python Shape

- **`Calculator` is a plain class with a `@property display`**. Identity through time, mutable state — no `@dataclass(frozen=True)` here (contrast with `password/` where `Policy` *is* frozen because a policy is a value). Fields use `snake_case`, method names use `press`/`press_keys` per PEP 8.
- **`KEYS` / `DISPLAY` / `ERRORS` are plain classes with class-level attributes.** No `StrEnum`, no module-level sprawl — one small namespace per concept. The strings stay byte-identical to the C# `Keys`/`DisplayStrings`/`ErrorMessages` and TS `Keys`/`DisplayStrings`/`ErrorMessages` values.
- **Integer division uses `int(a / b)`, not `a // b`.** This is the one tricky arithmetic divergence: Python's `//` floors toward negative infinity (`-7 // 2 == -4`), but the characterization set commits to truncate-toward-zero across languages (`-7 / 2 = -3`). `int(a / b)` performs a float divide and truncates toward zero — matching C# integer `/` and TypeScript `Math.trunc(a/b)`. A helper `_trunc_div` names this so the walkthrough note lives next to the code.
- **`ValueError` is the exception type** where C# throws `ArgumentException` and TS throws `Error`. The message string (`"unknown key: x"`) is identical across languages; the exception type is not.

## Why `CalculatorBuilder` Lives in `tests/`

Same F2 rationale as C# and TypeScript: scenarios need many key sequences, and without a builder each test opens with a loop or a run of `calculator.press("1"); calculator.press("+"); ...`. With the builder, setup is one line (`a_calculator().press_keys("1+2=")`) that reads as the user's hand on the calculator. The builder is 19 lines — within the 10–30 line F2 budget for Python.

`tests/__init__.py` exists so tests can `from tests.calculator_builder import a_calculator` — the conventional test-folder builder import, mirroring what C# gets through `namespace CalcRefactor.Tests` and TS gets through relative imports.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_calculator.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd calc-refactor/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

Expected: **20 passed.**
