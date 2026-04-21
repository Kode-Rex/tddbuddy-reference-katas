# Roll Your Own Mock Framework — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Rather than stepping through red/green cycles, this walkthrough explains **why the design came out the shape it did** and where the meta-kata's language-specific concerns live.

## The Proxy Mechanism: `DynamicObject`

C#'s `DynamicObject` class is the heart of this implementation. `Mock` extends `DynamicObject` and overrides `TryInvokeMember` — the method the DLR (Dynamic Language Runtime) calls when code invokes a method on a `dynamic` reference.

```csharp
dynamic mock = CreateMock();
mock.add(2, 3);  // DLR calls TryInvokeMember(binder: "add", args: [2, 3])
```

This is the same mechanism that real C# mocking libraries (Moq, NSubstitute) use under the hood — they just wrap it behind Castle.DynamicProxy's interface-implementing layer. For this kata, `DynamicObject` is the right teaching choice because the proxy mechanism is *visible*: the reader can see `TryInvokeMember` intercept each call.

See `src/RollYourOwnMockFramework/Mock.cs`.

## Why `VerificationError` Is a Domain Exception

The framework needs to distinguish "verification failed" from "code threw an unexpected error." A named domain exception — `VerificationError` — lets consumers catch the specific type. This follows the F3 convention: name the rejection, not just the mechanism.

The messages are byte-identical across all three language implementations:
- `"expected add to be called but was never called"`
- `"expected add(1, 1) to be called but was called with (2, 3)"`
- `"expected add to be called 2 times but was called 1 times"`

## The Fluent API: `When` and `Verify`

The stubbing API chains through `DynamicObject` twice:

1. `When(mock)` returns a `WhenClause` (also a `DynamicObject`)
2. Calling `.add(2, 3)` on the `WhenClause` triggers `TryInvokeMember`, which captures the method name and args and returns a `StubConfiguration`
3. `.ThenReturn(5)` on the `StubConfiguration` registers the stub on the mock

Verification uses `TryGetMember` instead of `TryInvokeMember`:

1. `Verify(mock)` returns a `VerifyClause` (a `DynamicObject`)
2. Accessing `.add` triggers `TryGetMember`, returning a `MethodVerification`
3. `.WasCalled()`, `.WasCalledWith(...)`, or `.WasCalledTimes(n)` execute the verification

This two-layer dynamic dispatch is the most complex part of the C# implementation. TypeScript and Python achieve the same fluency more naturally because their proxy/attribute mechanisms don't distinguish member access from member invocation.

## Call Recording and Stub Lookup

Every call to `TryInvokeMember` records a `CallRecord(methodName, args)` in a list. Stubs are stored in a `Dictionary<string, List<(object?[] Args, object? ReturnValue)>>` keyed by method name. On each call, the mock walks the stub list for that method name and returns the first match by argument equality (using `Equals`). No match returns `null`.

## Divergence from TypeScript and Python

C# is the only implementation that:
- Uses `DynamicObject` / DLR dynamic dispatch for proxy behavior (TS uses `Proxy`; Python uses `__getattr__`)
- Requires explicit casts from `dynamic` to `StubConfiguration` / `MethodVerification` in tests (the other two languages handle this transparently)
- Separates `TryInvokeMember` (mock calls, stub setup) from `TryGetMember` (verification property access) — this distinction does not exist in TS/Python proxies

## Scenario Map

All twelve scenarios from [`../SCENARIOS.md`](../SCENARIOS.md) live in a single test file: `MockTests.cs`, one `[Fact]` per scenario.

## How to Run

```bash
cd roll-your-own-mock-framework/csharp
dotnet test
```
