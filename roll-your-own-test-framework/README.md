# Roll Your Own Test Framework

**Meta-kata**: use TDD to build a TDD tool. The SUT *is* a test framework — you build test discovery, assertions, and result reporting, all driven by the very xUnit/Vitest/pytest runner that exercises them. The irony is deliberate and is the teaching point.

## What this kata teaches

- **Language-specific discovery mechanisms** — this is NOT a "same shape, three idioms" kata. The domain concept (discover tests, run them, report results) is shared, but the mechanism diverges sharply per language:
  - **C#**: reflection (`MethodInfo` + `[Test]` attribute) discovers test methods on a `Type`
  - **TypeScript**: function registration via a plain object (`Record<string, () => void>`) — no runtime reflection
  - **Python**: `inspect.getmembers` + `test_` naming convention discovers test methods on a class
- **Domain Exceptions** — `AssertionFailedException` is a domain concept that the runner catches to distinguish FAIL from ERROR
- **Test Data Builders** — `TestSuiteBuilder` is a fluent way to construct a test class/suite with specific passing, failing, and erroring tests, avoiding boilerplate in every test
- **Assertions as domain types** — `assertEqual`, `assertTrue`, `assertThrows` all throw a named domain exception on failure; the runner catches it

## No collaborators

This kata has no Clock, no Notifier, no injected dependencies. The domain is pure: discover, run, report.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
