# String Transformer — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — Eight Transformations

`cipher`, `slug`, `mask`, and immutable branching are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## The Python Shape

- **`Transformation` is a `typing.Protocol`**. That is the Pythonic interface — structural typing, no inheritance, no registration. Any class with an `apply(self, input: str) -> str` method satisfies it. The concrete transformations do not declare `class Capitalise(Transformation)` because they do not need to; `Pipeline.steps: List[Transformation]` accepts them because they match the shape. This is the same load-bearing decision C# made with `interface ITransformation`, delivered through Python's structural-typing idiom.
- **Parametric transformations (`Truncate`, `Repeat`, `Replace`) are `@dataclass(frozen=True)`**. Frozen gives value equality, immutability, and a useful `repr()` for test-failure diagnostics. Parameter-less transformations (`Capitalise`, `Reverse`, `RemoveWhitespace`, `SnakeCase`, `CamelCase`) are plain classes — the dataclass machinery would earn nothing with no fields.
- **`Pipeline` is a `@dataclass(frozen=True)` holding a `List[Transformation]`**. `run` folds `apply` across the steps with a plain `for` loop — `functools.reduce` is available but the loop reads more plainly.
- **Character classes use Python built-ins**: `c.isspace()` for whitespace, literal range checks for ASCII letters. `re` is not imported; the predicates are one line each.
- **`__init__.py` re-exports the full surface** so tests and downstream callers can `from string_transformer import Pipeline, Capitalise, …` without reaching into `pipeline.py` directly.

## Why `PipelineBuilder` Lives in `tests/`

Same F2 rationale as C#: fifteen scenarios need fifteen pipelines, and without a builder each test opens with a `Pipeline(steps=[Capitalise(), Reverse()])` that mixes construction with the scenario's variation. The builder is about 50 lines — above the C#/TS budget, at the upper edge of Python's 10–30-line F2 range, and that is the honest idiomatic shape: Python's explicit `self`, return-type annotations, and one-method-per-line cost about ten lines over the C# fluent pattern. Don't golf it down to fit a C# target.

`tests/__init__.py` exists so that tests can `from tests.pipeline_builder import PipelineBuilder` — mirroring what C# gets through `namespace StringTransformer.Tests` and TS gets through relative import.

## Scenario Map

The fifteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_pipeline.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd string-transformer/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
