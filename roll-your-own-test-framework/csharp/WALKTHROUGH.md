# Roll Your Own Test Framework — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Rather than stepping through red/green cycles, this walkthrough explains **why the design came out the shape it did** and where the meta-kata's language-specific concerns live.

## The Meta-Irony

We're using xUnit + FluentAssertions to test a test framework. The SUT's `Assertions.AssertEqual` is exercised by xUnit's `[Fact]` methods, and failures are verified via FluentAssertions' `.Should().Throw<>()`. This layering is deliberate — the kata's teaching point is precisely this: the framework under test is a domain like any other, and the "real" test framework verifies it the same way it would verify a bank account.

## Discovery via Reflection

C# is the only language of the three where runtime reflection gives us genuine method discovery. `TestRunner.RunAll(Type testClass)` uses `GetMethods(BindingFlags.Public | BindingFlags.Instance)` filtered by the presence of a `[Test]` attribute. This is the same mechanism real frameworks like NUnit use.

The `[Test]` attribute is a one-line `[AttributeUsage(AttributeTargets.Method)]` marker — no logic, just metadata. The runner reads it, creates an instance of the test class per method, and invokes via `MethodInfo.Invoke`. Because `Invoke` wraps exceptions in `TargetInvocationException`, the runner unwraps: `AssertionFailedException` inner → FAIL, anything else → ERROR.

See `src/RollYourOwnTestFramework/TestRunner.cs` and `src/RollYourOwnTestFramework/TestAttribute.cs`.

## Why `AssertionFailedException` Is a Domain Exception

The runner needs to distinguish "test made an assertion that failed" from "test threw an unexpected exception." A shared base `Exception` type would force the runner to check message strings. A named domain exception type — `AssertionFailedException` — lets the runner pattern-match on the type itself:

```csharp
catch (TargetInvocationException ex) when (ex.InnerException is AssertionFailedException afe)
```

This is the **domain exception** pattern from F3 conventions: name the rejection, not just the mechanism.

## Why `TestSuiteBuilder` Uses Dynamic Assembly Emission

The builder needs to create a `Type` at runtime with methods marked `[Test]` that execute specific `Action` delegates. C#'s type system requires actual `MethodInfo` instances — you can't fake them with a dictionary. So the builder uses `AssemblyBuilder` / `TypeBuilder` / `MethodBuilder` to emit a dynamic type with IL that loads a static `Action` field and invokes it.

This is the most complex builder in the reference katas, but it earns its keep: without it, every test would manually define an inner class with `[Test]` methods. The builder collapses that to `.WithPassingTest("name").WithFailingTest("name", "message").Build()`.

## The Three Assertion Types

- `AssertEqual(expected, actual)` — uses `Equals(expected, actual)` for comparison; message: `"expected {expected} but got {actual}"`
- `AssertTrue(condition)` — message: `"expected true"`
- `AssertThrows(action)` — wraps in try/catch; message: `"expected exception"`

All three throw `AssertionFailedException` with byte-identical messages across the three language implementations.

## Divergence from TypeScript and Python

C# is the only implementation that:
- Uses a custom `[Test]` attribute for discovery (TS uses object keys; Python uses `test_` prefix)
- Requires dynamic assembly emission in the builder (TS builds a plain object; Python builds a class with `type()`)
- Unwraps `TargetInvocationException` in the runner (reflection's wrapping behavior is C#-specific)

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live across two test files: `TestRunnerTests.cs` (scenarios 1–6) and `AssertionsTests.cs` (scenarios 7–12), one `[Fact]` per scenario.

## How to Run

```bash
cd roll-your-own-test-framework/csharp
dotnet test
```
