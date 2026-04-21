# Roll Your Own Mock Framework — TypeScript Walkthrough

Same design as the C# implementation — same domain abstractions, same verification messages — but the proxy mechanism is fundamentally different because TypeScript uses the ES6 `Proxy` object, which is the canonical JavaScript metaprogramming primitive.

## The Proxy Mechanism: ES6 `Proxy` with `get` Trap

JavaScript's `Proxy` constructor takes a target object and a handler with traps. The `get` trap intercepts *every property access* on the proxy — there is no separate "invoke member" trap like C#'s `TryInvokeMember`. Instead, the `get` trap returns a function, and the caller invokes that function:

```typescript
const proxy = new Proxy({}, {
  get(_target, prop: string) {
    return (...args: unknown[]) => {
      // prop = "add", args = [2, 3]
    };
  },
});

proxy.add(2, 3);  // get trap fires for "add", returns a function, caller invokes it
```

This two-step (access property, invoke result) is the standard JS pattern. Real mock libraries like `jest.fn()` and `sinon.stub()` use the same mechanism internally. The proxy is invisible to the caller — `mock.add(2, 3)` reads like a normal method call.

See `src/Mock.ts`.

## `WeakMap` for State Association

Each mock proxy needs associated state (call records, stubs), but the proxy itself is a transparent wrapper. We use a `WeakMap<MockProxy, MockState>` to associate state without polluting the proxy's property space. This means `when(mock)` and `verify(mock)` can look up the state for any mock without the mock having visible `.calls` or `.stubs` properties.

This is a TS-specific pattern — C# stores state as instance fields on the `DynamicObject` subclass, and Python stores it as `_` prefixed attributes on the mock instance. The `WeakMap` approach is the most idiomatic for JS/TS because it preserves the proxy's clean surface.

## The Fluent API: Three Proxies Deep

The mock framework uses proxies at three levels:

1. **`createMock()`** — returns a `Proxy` that records calls and serves stubs
2. **`when(mock)`** — returns a `Proxy` whose `get` trap returns functions that produce `StubConfiguration` objects with `.thenReturn(value)`
3. **`verify(mock)`** — returns a `Proxy` whose `get` trap returns `MethodVerification` objects with `.wasCalled()`, `.wasCalledWith(...)`, `.wasCalledTimes(n)`

This reads naturally in tests:

```typescript
when(mock).add(2, 3).thenReturn(5);       // proxy → get("add") → call(2, 3) → stub config
verify(mock).add.wasCalledWith(2, 3);      // proxy → get("add") → MethodVerification → assert
```

Unlike C#, TS does not need separate `TryInvokeMember` and `TryGetMember` overrides — the `get` trap handles both cases. The `when` proxy returns callable functions (for capturing args); the `verify` proxy returns verification objects directly.

## Divergence from C# and Python

TypeScript is the only implementation that:
- Uses `WeakMap` for state association (C# uses instance fields; Python uses `_` attributes)
- Has a single `get` trap for all proxy interception (C# distinguishes `TryInvokeMember` from `TryGetMember`)
- Requires no explicit casts in tests — the proxy's type system is transparent to callers

## Scenario Map

All twelve scenarios from [`../SCENARIOS.md`](../SCENARIOS.md) live in a single test file: `mock.test.ts`, one `it()` per scenario.

## How to Run

```bash
cd roll-your-own-mock-framework/typescript
npm install
npx vitest run
```
