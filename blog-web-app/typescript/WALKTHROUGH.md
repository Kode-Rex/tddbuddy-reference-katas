# Blog Web App — TypeScript Walkthrough

This kata ships in **middle gear** — see the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale; the decisions transfer directly. This page calls out what's idiomatic to TypeScript.

## TypeScript-Specific Notes

### `Clock` as an Interface

TypeScript interfaces are structural, so `FixedClock` in the test folder simply implements `now(): Date`. Stating `implements Clock` documents intent even though duck typing would suffice.

### `UnauthorizedOperationError` Extends `Error`

C# and Python use domain-specific exception types (`UnauthorizedOperationException`, `UnauthorizedOperationError`). TypeScript extends `Error` and sets `this.name` so `instanceof` checks and message assertions both work in tests.

### `Post` Encapsulates Mutation

`Post` uses private backing fields (`_title`, `_body`) with getter properties and an `edit()` method. This keeps mutation controlled — callers cannot assign `post.title = "hacked"` without going through the API.

### `ReadonlySet` and `ReadonlyMap`

`Blog.users` exposes `ReadonlyMap<string, User>`, `Blog.posts` exposes `ReadonlyMap<number, Post>`, and `Post.tags` exposes `ReadonlySet<string>`. These are compile-time guards — a consumer can read but not mutate the internal collections.

### Dates Stay UTC

All timestamps use `new Date(Date.UTC(...))`. This keeps timestamp comparisons consistent regardless of the test machine's locale.

## Scenario Map

Twenty-five scenarios live in `tests/blog.test.ts`, one `it()` per scenario. Test names match `SCENARIOS.md` verbatim in sentence form.

## How to Run

```bash
cd blog-web-app/typescript
npm install
npm test
```
