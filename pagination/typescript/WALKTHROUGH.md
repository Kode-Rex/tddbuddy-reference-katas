# Pagination — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — PageRequest Only

SQL offset/limit helpers, cursor-based pagination, and first/last-page navigation sugar are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The TypeScript Shape

- **`PageRequest` is an interface plus a `createPageRequest(spec)` factory**, not a class. Classes earn nothing here — no inheritance, no `this`-sensitive methods beyond `pageWindow`, no `instanceof` checks anywhere. The interface names the shape; the factory computes the derived values up-front and closes over them in a closure-style `pageWindow` method. `readonly` on every field gives the same immutability guarantee C# gets from `record`.
- **Derived fields are computed at construction**, not as getters. TypeScript interfaces cannot carry getter semantics cheaply, and eagerly computing `totalPages`, `startItem`, `endItem`, `hasPrevious`, and `hasNext` is idiomatic — they are cheap scalars, and the returned object is immutable. `pageWindow` stays a method because it takes a parameter.
- **Defaults are module-level constants** (`DEFAULT_PAGE_NUMBER`, `DEFAULT_PAGE_SIZE`, `DEFAULT_TOTAL_ITEMS`) rather than a single namespace object. Tree-shaking friendly and idiomatic in TS; C# groups them under `PageDefaults` because a namespace with five one-liners needs a home.
- **Errors are plain `Error`** with the byte-identical messages the spec requires (`"totalItems must be >= 0"`, `"pageSize must be >= 1"`).

## Why `PageRequestBuilder` Lives in `tests/`

Same F2 rationale as C#: fourteen scenarios, each caring about one or two inputs, each writing `new PageRequestBuilder().totalItems(95).pageSize(10).pageNumber(10).build()` with the single variation named. The builder is ~20 lines — inside the F2 budget.

## Scenario Map

The fourteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/pageRequest.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd pagination/typescript
npm install
npx vitest run
```
