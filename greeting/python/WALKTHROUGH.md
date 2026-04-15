# Greeting — Python Walkthrough

This is an **algorithmic string-formatting kata**: the input is a name, `None`, or a list of names, the output is a string, and there are no aggregates, value types, or collaborators — the inputs and outputs *are* the domain. The reference lands as a single commit: `src/greeting/greeting.py` defines `greet(name: str | list[str | None] | None) -> str`. The union signature handles the three input shapes in one function (the Python 3.11 `X | Y` syntax reads cleanly at the call site). Internally the function resolves `None` to `"my friend"`, partitions the names into normals and shouts, joins each partition with its own shape rule, and — when both partitions are non-empty — concatenates them with `". AND "`. The package `__init__.py` re-exports `greet` so tests import it as `from greeting import greet`. `tests/test_greeting.py` has one function per scenario in [`../SCENARIOS.md`](../SCENARIOS.md); each test name reads as a sentence from that spec.

**Shout detection.** A shout is a string with at least one alphabetic character whose uppercase form equals itself: `any(c.isalpha() for c in name) and name == name.upper()`. This avoids treating pure-punctuation or empty strings as shouts.

**Punctuation asymmetry — deliberate and spec-faithful.** The TDD Buddy prompt ends scenarios 1–2 with a trailing period but scenarios 4–5 without one; the reference honors those literals exactly rather than "fixing" the inconsistency.

**Inline literals — deliberate.** The strings `"Hello, "`, `"my friend"`, `". AND "`, and `"HELLO "` stay inline. F1 katas treat the literal rules as the rule itself.

The two bonus tasks from the TDD Buddy prompt are intentionally out of scope — see the kata [`README.md`](../README.md).
