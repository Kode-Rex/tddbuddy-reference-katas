# Pagination — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It follows the [`password/`](../../password/) reference as the second **F2** kata: one primary entity (`PageRequest`), one small test-folder builder (`PageRequestBuilder`), and nothing else. Read the repo [Gears section](../../README.md#gears--bridging-tdd-and-bdd) for why middle gear is the deliberate choice, not a corner cut.

## Scope — PageRequest Only

The original TDD Buddy Pagination prompt describes bonus features — SQL offset/limit helpers, cursor-based pagination, and `firstPage`/`lastPage` navigation sugar. **All of that is deliberately out of scope here.** Each bonus tips the kata either into trivia (sugar over existing fields) or into F3 territory (cursor-based pagination needs the sorted source list as a collaborator). See the kata [`README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Design at a Glance

```
PageRequest (record)
  PageNumber    (clamped at construction)
  PageSize
  TotalItems
  ─ derived ─
  TotalPages
  StartItem
  EndItem
  HasPrevious
  HasNext
  PageWindow(size) ──> IReadOnlyList<int>

PageRequestBuilder (tests/) ──Build──> PageRequest
```

Two files under `src/Pagination/` (the entity plus the defaults constants) and one builder under `tests/Pagination.Tests/`. That is the whole F2 surface.

## Why `PageRequest` Clamps at Construction

The spec says "if `currentPage < 1`, clamp to 1; if `currentPage > totalPages`, clamp to `totalPages`." The easy thing is to accept any `pageNumber` and clamp in each derived getter — but then every derived property has to remember the rule, and any future derivation is one missed clamp away from an off-by-one bug.

Clamping at construction means `PageNumber` on the record is **always** a valid page. `StartItem`, `EndItem`, `HasPrevious`, and `HasNext` can all be expressed in plain arithmetic without rechecking bounds. Tests that pass `PageNumber(0)` or `PageNumber(99)` assert on the clamped value — the clamp *is* the behavior, not a defensive afterthought.

The one subtlety: when `totalItems = 0`, `totalPages = 0`, and the spec's table shows page 1 is still the "valid" answer with `startItem = endItem = 0`. So clamping resolves to `1` in the empty case, and `StartItem`/`EndItem` guard specifically on `TotalItems == 0` to return zero instead of `(1-1)*10+1 = 1`.

## Why Invalid Inputs Throw (But Out-of-Range Page Numbers Don't)

`totalItems < 0` and `pageSize < 1` are programmer errors — there is no sensible default for "negative total items." They throw `ArgumentException` with a message that names the failing parameter and the required invariant.

Out-of-range `pageNumber` is not a programmer error — it is the UI asking for a page that does not exist, which is exactly what clamping is for. A user bookmarks page 99 of a list that has shrunk to 10 pages; the right behavior is to show page 10, not throw. The asymmetry is deliberate.

## Why `PageWindow` Lives on `PageRequest`

`PageWindow(windowSize)` is a **method**, not a precomputed property. Two reasons:

1. The window size is a UI concern — a desktop view might show 7 pages, a mobile view 3. `PageRequest` does not know which, so the caller supplies it.
2. Making it a method instead of `Window5`, `Window7`, `Window10` avoids multiplying derived properties for every imaginable window size.

The algorithm is small: half the window goes left of the current page, half goes right, then clip to `[1..totalPages]` shifting the other edge to keep the window width constant where possible. When `totalPages < windowSize`, the window shrinks to `totalPages`.

## Why `PageRequestBuilder` Exists — The F2 Signature Pattern

Fourteen scenarios need fourteen slightly different requests. Without a builder, each test opens with:

```csharp
var request = new PageRequest(pageNumber: 10, pageSize: 10, totalItems: 95);
```

Readable, but every test has to specify three numbers and figure out which one is the variation. With the builder:

```csharp
var request = new PageRequestBuilder().TotalItems(95).PageSize(10).PageNumber(10).Build();
```

One line per field, each field named, and defaults cover the two that do not matter. The builder is fourteen lines (including braces) — well inside the F2 budget — and never has to grow.

Named constants for the defaults live in `PageDefaults` because this is F2 (Full-Bake mode). `DefaultPageSize = 10` is a business number, and naming it in one place beats scattering `10` across three languages' builders.

## What Is Deliberately Not Modeled

- **No `Page<T>` slice helper** — the spec stops at calculating the window, not applying it to a list. Adding slicing would mean a second type, a second scenario set, and a generic parameter that adds surface without adding teaching value.
- **No offset/limit getters** — sugar over `(PageNumber - 1) * PageSize` and `PageSize`. Bonus territory.
- **No cursor-based pagination** — needs the sorted source list; F3.
- **No `FirstPage()` / `LastPage()` helpers** — `PageNumber(1)` and `PageNumber(TotalPages)` are one line each.

Every omission above points at an F3 extension or a trivial one-liner. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## Scenario Map

The fourteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/Pagination.Tests/PageRequestTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd pagination/csharp
dotnet test
```
