# Roll Your Own Mock Framework

**Meta-kata**: use TDD to build a mock framework. The SUT *is* a mock object system — you build proxy creation, stub configuration, call recording, and verification, all driven by the xUnit/Vitest/pytest runner that exercises them. The companion to `roll-your-own-test-framework/`: that one builds the runner, this one builds the test doubles.

## What this kata teaches

- **Language-specific proxy mechanisms** — this is NOT a "same shape, three idioms" kata. The domain concept (create proxy, record calls, configure stubs, verify invocations) is shared, but the mechanism diverges sharply per language:
  - **C#**: `DynamicObject` with `TryInvokeMember` override — intercepts method calls as dynamic dispatch, recording name + args
  - **TypeScript**: `Proxy` with `get` trap — the canonical JS metaprogramming mechanism; each property access returns a callable that records the invocation
  - **Python**: `__getattr__` magic method — intercepts attribute access on a class instance, returning a callable that records the invocation
- **Domain Exceptions** — `VerificationError` is a domain concept that verification methods throw when expected calls don't match recorded calls
- **Fluent API design** — `when(mock).method(args).thenReturn(value)` and `verify(mock).method(args)` chain naturally; the API itself is a teaching point
- **Call recording as data** — every proxy call becomes an entry in a list; stubs are a lookup table keyed by (method name, args); verification is a search over recorded calls

## No collaborators

This kata has no Clock, no Notifier, no injected dependencies. The domain is pure: proxy, record, stub, verify.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
