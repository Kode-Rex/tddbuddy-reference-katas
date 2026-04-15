# Pagination — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — PageRequest Only

SQL offset/limit helpers, cursor-based pagination, and first/last-page navigation sugar are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Python Shape

- **`PageRequest` is a `@dataclass(frozen=True)`** with the three inputs declared as regular fields and the five derived values declared as `field(init=False)` that `__post_init__` fills in via `object.__setattr__` (the standard escape hatch for frozen dataclasses). Eagerly computing the derived values keeps them as plain attributes — no `@property` shim, no lazy memoization — and frozen guarantees value equality and immutability for free.
- **Validation and clamping happen in `__post_init__`**. `total_items < 0` raises `ValueError("totalItems must be >= 0")`; `page_size < 1` raises `ValueError("pageSize must be >= 1")`. The message strings are **byte-identical** to the C# and TypeScript spellings — the strings are the spec, even though the exception type is language-idiomatic (`ValueError` vs `ArgumentException` vs `Error`).
- **`page_window(window_size)` is a regular method** rather than an eagerly computed field, because it takes a parameter. The algorithm is identical to C# and TypeScript: half the window left of the current page, half right, clip to `[1..total_pages]` shifting the other edge to keep the width constant.
- **Field names use `snake_case`** (`page_number`, `total_items`, `has_previous`, `page_window`). The rule-string spellings in error messages match the camelCase spec names because *the message is the spec*, not the field.

## Why `PageRequestBuilder` Lives in `tests/`

Same F2 rationale as C#: fourteen scenarios, fourteen slightly different requests, and each test's single line of setup names the one variation it cares about. The builder is ~28 lines — inside the F2 Python budget of 30–40, slightly tighter because there are only three fields.

`tests/__init__.py` exists so that tests can `from tests.page_request_builder import PageRequestBuilder` — a conventional test-folder builder import, mirroring what C# gets through `namespace Pagination.Tests` and TypeScript gets through relative import.

## Scenario Map

The fourteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_page_request.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python underscore convention).

## How to Run

```bash
cd pagination/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
