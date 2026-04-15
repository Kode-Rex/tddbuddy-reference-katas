# Pagination

A `PageRequest` encapsulates the three inputs of a paginated view — `pageNumber` (1-indexed), `pageSize`, and `totalItems` — and derives everything a UI needs to render pagination controls: `totalPages`, `startItem`, `endItem`, `hasPrevious`, `hasNext`, plus a page-window calculation for the `[1 … 5 6 7 … 20]` strip of page links.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**: one primary entity (`PageRequest`), one small test-folder builder (`PageRequestBuilder`, 10–40 lines depending on language), and a handful of derived values exposed as computed properties. No collaborators, no object mothers, no value types beyond primitives. The teaching point is the same as the password kata: without a builder every test opens with a three-argument constructor that buries the variation; with a builder each test opens with one readable line naming the single input it cares about — `new PageRequestBuilder().TotalItems(95).PageSize(10).PageNumber(10).Build()`.

## Scope — PageRequest Only

The astro-site spec describes bonus features that are deliberately out of scope for this reference:

### Stretch Goals (Not Implemented Here)

These are left for a future F3 follow-up that would earn its collaborators:

- **Offset/limit helpers for SQL** — trivial once `startItem` and `pageSize` exist; adds no teaching value here.
- **Cursor-based pagination** — needs a sorted list and a comparator, which turns the SUT into a list-aware query object (F3 territory).
- **`firstPage()` / `lastPage()` navigation helpers** — sugar over `PageNumber(1)` / `PageNumber(totalPages)`; not worth the surface in an F2 reference.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference does satisfy.
