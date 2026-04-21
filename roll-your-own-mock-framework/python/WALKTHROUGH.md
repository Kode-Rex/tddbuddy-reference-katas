# Roll Your Own Mock Framework — Python Walkthrough

Same design as C# and TypeScript — same domain abstractions, same verification messages — but the proxy mechanism uses Python's `__getattr__` magic method, which is the most natural interception point in the language.

## The Proxy Mechanism: `__getattr__`

Python calls `__getattr__` on an object whenever a normal attribute lookup fails. The `Mock` class defines `__getattr__` to return a callable for any attribute name that doesn't start with `_`:

```python
def __getattr__(self, name: str) -> Any:
    if name.startswith("_"):
        raise AttributeError(name)

    def method_proxy(*args: Any) -> Any:
        self._calls.append(CallRecord(method_name=name, args=args))
        # ... stub lookup ...
        return None

    return method_proxy
```

When test code calls `mock.add(2, 3)`, Python first looks for `add` in the instance dict and class dict. Finding nothing, it calls `__getattr__("add")`, which returns a closure that records the call and looks up stubs.

This is the same mechanism that Python's own `unittest.mock.MagicMock` uses internally. The guard on `_`-prefixed names prevents infinite recursion when Python accesses internal attributes like `_calls` and `_stubs`.

See `src/roll_your_own_mock_framework/mock.py`.

## The Fluent API: Three Proxy Classes

Like the TS implementation, the API chains through three proxy-like objects:

1. **`Mock`** — uses `__getattr__` to intercept and record method calls
2. **`_WhenProxy`** — uses `__getattr__` to capture the method name, returning a function that captures args and produces a `_StubConfiguration`
3. **`_VerifyProxy`** — uses `__getattr__` to capture the method name, returning a `_MethodVerification` object

The `when` and `verify` chains read naturally:

```python
when(mock).add(2, 3).then_return(5)       # WhenProxy → getattr("add") → call(2, 3) → stub config
verify(mock).add.was_called_with(2, 3)     # VerifyProxy → getattr("add") → MethodVerification → assert
```

Python's `__getattr__` makes this the simplest implementation of the three — no `DynamicObject` inheritance, no `Proxy` constructor, just a magic method.

## `VerificationError` Inherits from `Exception`

Python convention is to inherit from `Exception` (not `BaseException`). The verification methods raise `VerificationError` with messages byte-identical to C# and TypeScript:

- `"expected add to be called but was never called"`
- `"expected add(1, 1) to be called but was called with (2, 3)"`
- `"expected add to be called 2 times but was called 1 times"`

## `CallRecord` as a Frozen Dataclass

Call records are `@dataclass(frozen=True)` with `method_name: str` and `args: tuple[Any, ...]`. Using a tuple (not a list) for args makes records hashable and immutable — matching Python's preference for immutable value types.

## Divergence from C# and TypeScript

Python is the only implementation that:
- Uses `__getattr__` for proxy interception (C# uses `DynamicObject.TryInvokeMember`; TS uses `Proxy` with `get` trap)
- Guards on `_`-prefixed names to avoid recursion with internal state (C# and TS don't have this concern)
- Stores state directly on the mock instance (TS uses `WeakMap`; C# uses instance fields on a `DynamicObject` subclass)
- Uses snake_case for API methods (`then_return`, `was_called`, `was_called_with`, `was_called_times`)

## Scenario Map

All twelve scenarios from [`../SCENARIOS.md`](../SCENARIOS.md) live in a single test file: `test_mock.py`, one test method per scenario.

## How to Run

```bash
cd roll-your-own-mock-framework/python
.venv/bin/pytest
```
