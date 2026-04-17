# Weather Station — TypeScript Walkthrough

This kata ships in **middle/high gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `Reading` and `Statistics` as Classes with `readonly` Fields

C# uses `record struct` for value types with automatic equality. TypeScript has no value types, so `Reading` and `Statistics` are plain classes with `readonly` constructor parameters. Tests compare individual fields rather than relying on structural equality — idiomatic for TypeScript where deep equality comparison is explicit.

### `AlertThresholds` as an Interface with Optional Fields

C# uses a `record struct` with nullable fields. TypeScript's `?` optional properties serve the same purpose more idiomatically. An empty `{}` means no thresholds configured — no sentinel values or null checks required.

### `Clock` as an Interface

TypeScript interfaces are structural, so `FixedClock` in the test folder simply implements `now(): Date`. Stating `implements Clock` documents intent even though duck typing would suffice.

### Dates Stay UTC

`Date` in JavaScript is notoriously timezone-lossy. Tests build fixed dates with `new Date(Date.UTC(2026, 5, 15, 12, 0, 0))`. This keeps timestamp comparisons consistent regardless of the test machine's locale.

### Error Subclasses

`InvalidReadingError` and `NoReadingsError` extend `Error` with a `name` property set explicitly. This is the TypeScript equivalent of C#'s domain-specific exception types — tests can catch by type, and the error name appears in stack traces.

The error messages are **byte-identical across C#, TypeScript, and Python**.

## Scenario Map

Twenty-four scenarios live in `tests/station.test.ts`, one `it()` per scenario. Test names match `SCENARIOS.md` verbatim in sentence form.

## How to Run

```bash
cd weather-station/typescript
npm install
npm test
```
