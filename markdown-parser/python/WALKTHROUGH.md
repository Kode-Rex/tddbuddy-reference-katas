# Markdown Parser — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — direct-to-HTML, block-then-inline two-pass architecture. This document covers only what diverges in Python idiom.

## Module-Level Function

Like TypeScript, Python exports a module-level `parse` function rather than wrapping it in a class. `from markdown_parser import parse` is idiomatic Python — a stateless transformation does not need a class to house it.

## `re.sub` with Callable for Inline Code

Python's `re.sub` accepts a callable as the replacement argument. The inline-code extraction uses this to capture code spans into a list and return placeholder strings. This is cleaner than the C# `Regex.Replace` with a `MatchEvaluator` delegate — Python's lambda/closure model makes the capture-and-index pattern read naturally.

## `DocumentBuilder` Returns `DocumentBuilder`, Not `Self`

Python 3.11 supports `typing.Self` but the builder explicitly returns `DocumentBuilder` for readability and compatibility with the broader Python ecosystem. This matches the bank-account Python builder's convention.

## `__init__.py` Re-Export

The package's `__init__.py` re-exports `parse` so tests import `from markdown_parser import parse` — one level of indirection, matching the domain vocabulary directly rather than requiring callers to know about `markdown_parser.parser`.

## How to Run

```bash
cd markdown-parser/python
python3 -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
