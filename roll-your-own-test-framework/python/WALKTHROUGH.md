# Roll Your Own Test Framework — Python Walkthrough

Same design as C# and TypeScript — same domain abstractions, same assertion messages — but discovery uses Python's `inspect` module with a `test_` naming convention, which is the closest to how real Python test frameworks (pytest, unittest) work.

## Discovery via `inspect.getmembers`

`TestRunner.run_all(test_class)` creates an instance of the test class, then uses `inspect.getmembers(instance, predicate=inspect.ismethod)` to find all bound methods. It filters for names starting with `test_` — the same convention pytest itself uses.

This is the most natural approach for Python: no decorators needed, no registration. If a method starts with `test_`, it's a test. The `inspect` module provides the introspection that C# gets from `System.Reflection` — but the filtering is by naming convention rather than by attribute.

See `src/roll_your_own_test_framework/test_runner.py`.

## The Builder Uses `type()` for Dynamic Class Creation

Python's `type(name, bases, dict)` built-in creates a class at runtime. The `TestSuiteBuilder` accumulates a dictionary of method names to callables, then calls `type("DynamicTestSuite", (), methods)` to produce a class.

The methods are regular functions that take `self` as the first argument — this is required for `inspect.ismethod` to classify them as bound methods on an instance. This is simpler than C#'s IL emission or TS's plain object approach — Python's metaprogramming makes dynamic class creation a one-liner.

## `AssertionFailedException` Inherits from `Exception`

Python convention is to inherit from `Exception` (not `BaseException`). The runner catches `AssertionFailedException` specifically for FAIL results and catches `Exception` broadly for ERROR results. The domain exception is just a class with a constructor — no special plumbing.

## Assertions as Module-Level Functions

Python idiom for stateless functions is module-level, not class methods. `assert_equal`, `assert_true`, and `assert_throws` are standalone functions in `assertions.py`, re-exported via `__init__.py`. Messages are byte-identical to C# and TypeScript:

- `assert_equal(5, 3)` raises `"expected 5 but got 3"`
- `assert_true(False)` raises `"expected true"`
- `assert_throws(lambda: None)` raises `"expected exception"`

## Divergence from C# and TypeScript

Python is the only implementation that:
- Discovers by naming convention (`test_` prefix) rather than attribute or object keys
- Uses `type()` for dynamic class creation (vs IL emission in C#, plain objects in TS)
- Uses `inspect.getmembers` for method introspection (built-in module, no reflection namespace)

## Scenario Map

Twelve scenarios from [`../SCENARIOS.md`](../SCENARIOS.md) across two test files: `test_runner.py` (scenarios 1-6) and `test_assertions.py` (scenarios 7-12).

## How to Run

```bash
cd roll-your-own-test-framework/python
.venv/bin/pytest
```
