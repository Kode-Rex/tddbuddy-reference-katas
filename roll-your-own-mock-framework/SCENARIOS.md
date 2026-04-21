# Roll Your Own Mock Framework — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Mock** | A proxy object that records all method calls and serves configured stub return values |
| **Stub** | A configured return value for a specific method name + argument combination |
| **CallRecord** | A recorded invocation: method name + arguments list |
| **when** | Entry point for stub configuration: `when(mock).method(args).thenReturn(value)` |
| **verify** | Entry point for verification: `verify(mock).method(args)` or `verify(mock).method.wasCalledTimes(n)` |
| **VerificationError** | Domain exception thrown when verification fails — message describes expected vs actual calls |
| **MockBuilder** | Factory function or class that creates a mock proxy (language-specific mechanism) |

## Domain Rules

- A mock is created via `createMock()` — it accepts any method call without throwing
- An unstubbed method returns `null` (C#), `undefined` (TS), or `None` (Python)
- `when(mock).method(args).thenReturn(value)` configures a stub — subsequent calls with matching args return the configured value
- Multiple stubs can be configured for the same method with different argument sets
- Every method call on the mock is recorded as a `CallRecord` (method name + args)
- `verify(mock).method.wasCalled()` passes if the method was called at least once; throws `VerificationError` if never called
- `verify(mock).method.wasCalledWith(args)` passes if the method was called with exactly those args; throws `VerificationError` with message `"expected <method>(<args>) to be called but was never called"` if the method was never called, or `"expected <method>(<expected args>) to be called but was called with (<actual args>)"` if called with different args
- `verify(mock).method.wasCalledTimes(n)` passes if the method was called exactly `n` times; throws `VerificationError` with message `"expected <method> to be called <n> times but was called <actual> times"`

## Test Scenarios

### Mock Creation

1. **Create mock — methods are callable without error** — calling any method name on the mock does not throw
2. **Unstubbed method returns null/undefined/None** — calling a method with no configured stub returns the language's null equivalent

### Stub Configuration

3. **Stub return value for matching args** — `when(mock).add(2, 3).thenReturn(5)` causes `mock.add(2, 3)` to return `5`
4. **Stub different arg sets return their own values** — stub `add(2, 3) -> 5` and `add(1, 1) -> 2`; each returns its configured value
5. **Unstubbed args return null even when other args are stubbed** — stub `add(2, 3) -> 5`; calling `add(9, 9)` returns null/undefined/None

### Verification — wasCalled

6. **Verify called method passes** — call `mock.add(2, 3)` then `verify(mock).add.wasCalled()` does not throw
7. **Verify uncalled method fails with message** — `verify(mock).add.wasCalled()` without calling `add` throws `VerificationError` with message `"expected add to be called but was never called"`

### Verification — wasCalledWith

8. **Verify called with correct args passes** — call `mock.add(2, 3)` then `verify(mock).add.wasCalledWith(2, 3)` does not throw
9. **Verify called with wrong args fails with message** — call `mock.add(2, 3)` then `verify(mock).add.wasCalledWith(1, 1)` throws `VerificationError` with message `"expected add(1, 1) to be called but was called with (2, 3)"`
10. **Verify wasCalledWith on uncalled method fails** — `verify(mock).add.wasCalledWith(1, 1)` without calling `add` throws `VerificationError` with message `"expected add(1, 1) to be called but was never called"`

### Verification — wasCalledTimes

11. **Verify call count matches** — call `mock.add(2, 3)` twice then `verify(mock).add.wasCalledTimes(2)` does not throw
12. **Verify call count mismatch fails with message** — call `mock.add(2, 3)` once then `verify(mock).add.wasCalledTimes(2)` throws `VerificationError` with message `"expected add to be called 2 times but was called 1 times"`
