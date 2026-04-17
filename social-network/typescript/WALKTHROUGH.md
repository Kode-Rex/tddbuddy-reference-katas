# Social Network — TypeScript Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `Clock` as an Interface

TypeScript interfaces are structural, so `FixedClock` in the test folder simply implements `now(): Date`. Stating `implements Clock` documents intent even though duck typing would suffice.

### Dates Stay UTC

`Date` in JavaScript is notoriously timezone-lossy. The implementation and tests build fixed dates with `new Date(Date.UTC(2026, 0, 15, 9, 0, 0))`. This keeps timestamp comparisons consistent regardless of the test machine's locale.

### `ReadonlySet` and `ReadonlyMap`

`User.following` exposes `ReadonlySet<string>` and `Network.users` exposes `ReadonlyMap<string, User>`. These are compile-time guards — a consumer can read but not mutate the internal collections.

### `Post` as a Class with `readonly` Fields

C# uses a `record` for `Post`. TypeScript has no built-in record type with structural equality, so `Post` is a simple class with `readonly` fields. Tests assert on individual properties rather than structural equality.

### Small Related Types Colocated

Each type lives in its own file (`Clock.ts`, `Post.ts`, `User.ts`, `Network.ts`) — matching the C# layout. For a domain this size, four small files are easier to navigate than one large module.

## Scenario Map

Eighteen scenarios live in `tests/network.test.ts`, one `it()` per scenario. Test names match `SCENARIOS.md` verbatim in sentence form.

## How to Run

```bash
cd social-network/typescript
npm install
npm test
```
