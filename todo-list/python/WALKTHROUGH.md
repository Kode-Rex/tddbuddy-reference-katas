# Todo List — Python Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to Python rather than re-arguing the design.

## Scope — In-Memory List Only

CSV persistence, CLI parsing, sub-tasks, and parent-completion guards are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Python Shape

- **`Task` is `@dataclass(frozen=True)`**. Frozen gives value-equality, immutability, and a useful `repr()` for failure diagnostics — the same three properties C# got from `record`. Field names use `snake_case` (`done`, `due`) — the shared vocabulary matches, the casing follows convention.
- **`TodoList` is a plain class** owning a list of tasks and the next-id counter. No dataclass — the aggregate has mutation semantics (tasks come and go over its lifetime) and a dataclass of mutable lists would add no value.
- **Due dates use `datetime.date`** rather than ISO strings. Unlike JS, Python's `date` is a clean date-only type — no time component, no timezone ambiguity. This is the idiomatic representation.
- **`TaskNotFoundError` extends `Exception`** with the message format `"task <id> not found"`, byte-identical to the C# and TypeScript spellings. Python has no catch-by-string pattern; tests use `pytest.raises(TaskNotFoundError, match=...)` to pin both type and message.
- **`replace(existing, done=True)`** from `dataclasses` is the Pythonic equivalent of C#'s `existing with { Done = true }` — a single-line copy-with-override that keeps the task value immutable.

## Why Ids Are Monotonic (Same as C#)

`_next_id` is a counter; removed ids are not reused. Same reasoning as the C# walkthrough.

## Why `TaskBuilder` Lives in `tests/`

Same F2 rationale as C#. Runtime task allocation happens through `todo.add()`; the builder is for the **assertion side**, letting tests write `assert task == TaskBuilder().with_id(1).titled("buy milk").build()` rather than a positional `Task(id=1, title="buy milk", done=False, due=None)` literal.

The builder is 32 lines in Python — at the upper edge of the 10–30 line F2 budget; Python's explicit `self` and type annotations cost about ten lines over the C# version, and that is the honest idiomatic shape.

`tests/__init__.py` exists so tests can `from tests.task_builder import TaskBuilder` — a conventional test-folder builder import, mirroring what C# gets through `namespace TodoList.Tests` and TS gets through relative import.

## Scenario Map

The thirteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/test_todo_list.py`, one function per scenario, with test names matching the scenario titles verbatim (modulo Python snake_case convention).

## How to Run

```bash
cd todo-list/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
