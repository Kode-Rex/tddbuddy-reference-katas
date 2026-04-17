# Maze Walker — Python Walkthrough

Same design as the C# implementation. This walkthrough covers the Python-specific differences.

## Cell as Frozen Dataclass

`@dataclass(frozen=True)` gives value equality and hashability for free — essential since the BFS walker stores cells in a `set` for visited tracking and a `dict` for the parent map. The `cardinal_neighbors()` method returns a plain list of four cells.

## Maze Grid as Nested Lists

The maze stores its grid as `list[list[CellType]]`. Python's `Enum` provides the cell type enumeration. The `cell_type_at()` method returns `None` for out-of-bounds coordinates, matching C#'s nullable approach.

## Walker Uses `collections.deque`

Python's `deque` provides O(1) `popleft()` for BFS — the correct choice over a plain list where `pop(0)` is O(n). The `came_from` dictionary maps `Cell` to `Cell` directly since frozen dataclasses are hashable.

## Domain Exceptions Extend Exception

Each domain exception class extends Python's `Exception` base. The message strings are byte-identical across all three languages.

## How to Run

```bash
cd maze-walker/python
python -m venv .venv
source .venv/bin/activate
pip install -e ".[dev]"
pytest
```
