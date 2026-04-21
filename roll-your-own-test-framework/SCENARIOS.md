# Roll Your Own Test Framework — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **TestResult** | Outcome of a single test: PASS, FAIL, or ERROR — plus the test name and optional message |
| **TestSummary** | Aggregate report: counts (run, passed, failed, errors) + list of individual results |
| **Assertions** | Domain functions (`assertEqual`, `assertTrue`, `assertThrows`) that throw `AssertionFailedException` on failure |
| **AssertionFailedException** | Domain exception thrown by assertions — the runner catches this to record FAIL; any other exception becomes ERROR |
| **TestRunner** | Discovers tests in a class/module/suite, executes each independently, returns a `TestSummary` |
| **TestSuiteBuilder** | Fluent builder (in tests/) that constructs a test class/suite with specific passing, failing, and erroring tests |

## Domain Rules

- The runner discovers tests using a **language-specific mechanism**: reflection (C#), object keys (TS), or `inspect` + naming convention (Python)
- Each discovered test runs **independently** — a failure in one does not prevent others from running
- A test that completes without throwing is **PASS**
- A test that throws `AssertionFailedException` is **FAIL** — the message from the exception is recorded
- A test that throws any other exception is **ERROR** — the exception info is recorded
- `assertEqual(expected, actual)` throws `AssertionFailedException` with message `"expected <expected> but got <actual>"` when values differ
- `assertTrue(condition)` throws `AssertionFailedException` with message `"expected true"` when condition is false
- `assertThrows(fn)` throws `AssertionFailedException` with message `"expected exception"` when the function does NOT throw
- The summary reports total run, passed, failed, and error counts

## Test Scenarios

### Discovery

1. **Empty suite discovers zero tests** — runner returns a summary with 0 tests run
2. **Suite with three tests discovers and runs all three** — summary shows 3 tests run

### Execution and Reporting

3. **Passing test is reported as PASS** — test that does not throw is recorded with PASS status
4. **Failing assertion is reported as FAIL with message** — test that throws `AssertionFailedException` is recorded with FAIL status and the exception message
5. **Unhandled exception is reported as ERROR with exception info** — test that throws a non-assertion exception is recorded with ERROR status and the exception message
6. **Multiple tests with mixed results produce correct summary counts** — a suite with 2 passing, 1 failing, 1 erroring reports run=4, passed=2, failed=1, errors=1

### Assertions — assertEqual

7. **assertEqual with equal values passes** — `assertEqual(5, 5)` does not throw
8. **assertEqual with different values fails** — `assertEqual(5, 3)` throws `AssertionFailedException` with message `"expected 5 but got 3"`

### Assertions — assertTrue

9. **assertTrue with true passes** — `assertTrue(true)` does not throw
10. **assertTrue with false fails** — `assertTrue(false)` throws `AssertionFailedException` with message `"expected true"`

### Assertions — assertThrows

11. **assertThrows with throwing function passes** — `assertThrows(fn)` where `fn` throws does not throw
12. **assertThrows with non-throwing function fails** — `assertThrows(fn)` where `fn` does not throw throws `AssertionFailedException` with message `"expected exception"`
