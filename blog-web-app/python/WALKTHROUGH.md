# Blog Web App — Python Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to Python.

## Python-Specific Notes

### `Clock` as a Protocol

Python's `typing.Protocol` provides structural subtyping — `FixedClock` satisfies `Clock` without inheriting from it. This is the Python equivalent of C#'s `IClock` interface and TypeScript's `Clock` interface.

### `Comment` as a Frozen Dataclass

`@dataclass(frozen=True)` gives structural equality and immutability. Comments are created once and never mutated — the only operation is deletion, which is handled by the owning `Post`.

### `UnauthorizedOperationError`

Python uses a domain-specific exception class extending `Exception`. The message strings are byte-identical to the C# and TypeScript implementations. Tests use `pytest.raises` with `match=` to assert on both the exception type and the message.

### `frozenset` for Tags

`Post.tags` returns a `frozenset[str]` — an immutable snapshot of the internal mutable `set`. This prevents callers from accidentally mutating the post's tag set while keeping the property cheap to access.

### `Blog.all_tags_for_user` Returns `frozenset`

The query returns a `frozenset` rather than a mutable `set` to signal that the result is a snapshot, not a live reference to internal state.

### Package Layout

The `src/blog_web_app/` layout with `pyproject.toml` follows the standard Python packaging convention. `__init__.py` re-exports `Blog`, `Clock`, `Comment`, `Post`, `UnauthorizedOperationError`, and `User` so tests import from the package root.

## Scenario Map

Twenty-five scenarios live in `tests/test_blog.py`, one `test_` function per scenario. Test names match `SCENARIOS.md` verbatim in snake_case form.

## How to Run

```bash
cd blog-web-app/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
