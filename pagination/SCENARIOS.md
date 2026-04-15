# Pagination — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **`PageRequest` with derived metadata and a page-window calculation**. SQL offset/limit helpers, cursor-based pagination, and first/last-page navigation sugar are **out of scope** — see the top-level [`README.md`](README.md#scope--pagerequest-only) for the full stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **PageRequest** | An immutable value combining `pageNumber` (1-indexed), `pageSize`, and `totalItems`. Derives `totalPages`, `startItem`, `endItem`, `hasPrevious`, `hasNext`, and `pageWindow(size)`. Clamps an out-of-range `pageNumber` at construction so downstream code never has to think about it. |
| **PageRequestBuilder** | Test-folder fluent builder that produces a `PageRequest`. Defaults: `pageNumber=1`, `pageSize=10`, `totalItems=0`. Chained methods opt into the single variation each test cares about. |
| **totalPages** | `ceil(totalItems / pageSize)`, or `0` when `totalItems` is zero. |
| **startItem** | The 1-indexed index of the first item on the current page. `0` when there are no items. |
| **endItem** | The 1-indexed index of the last item on the current page. On the final page with a partial window, less than `startItem + pageSize - 1`. `0` when there are no items. |
| **hasPrevious** | `true` iff `pageNumber > 1`. |
| **hasNext** | `true` iff `pageNumber < totalPages`. |
| **pageWindow(size)** | An ordered list of page numbers to show in the UI, centered on the current page where possible and clipped at the first/last page. Never longer than `size` and never longer than `totalPages`. |

## Domain Rules

- `totalItems` must be `>= 0`. `pageSize` must be `>= 1`. Violating either raises a domain error at construction (`ArgumentException` / `Error` / `ValueError`).
- `pageNumber` is **clamped** at construction: values `< 1` become `1`; values `> totalPages` become `totalPages` (or `1` when `totalPages == 0`, so that an empty dataset still has a valid "page 1" representation with `startItem = endItem = 0`).
- Defaults (from `PageRequestBuilder` with no chained calls): `pageNumber=1`, `pageSize=10`, `totalItems=0`.
- `pageWindow(size)` with `size <= 0` returns an empty list.

## Test Scenarios

1. **First page of 100 items with page size 10 shows items 1 through 10** — `startItem=1`, `endItem=10`, `hasPrevious=false`, `hasNext=true`, `totalPages=10`.
2. **Middle page of 100 items with page size 10 shows items 41 through 50** — page 5: `startItem=41`, `endItem=50`, `hasPrevious=true`, `hasNext=true`.
3. **Last page of 100 items with page size 10 shows items 91 through 100** — page 10: `startItem=91`, `endItem=100`, `hasPrevious=true`, `hasNext=false`.
4. **Last page with a partial window shows only the remaining items** — 95 items, page size 10, page 10: `startItem=91`, `endItem=95`, `totalPages=10`.
5. **Single item on one page reports itself as start and end** — 1 item, page size 10, page 1: `startItem=1`, `endItem=1`, `hasPrevious=false`, `hasNext=false`, `totalPages=1`.
6. **Zero items reports no pages and no items** — 0 items, page size 10, page 1: `totalPages=0`, `startItem=0`, `endItem=0`, `hasPrevious=false`, `hasNext=false`.
7. **Page number below 1 is clamped to 1** — 100 items, page size 10, requested page 0: resolves to page 1 with `startItem=1`, `endItem=10`.
8. **Page number above totalPages is clamped to the last page** — 100 items, page size 10, requested page 99: resolves to page 10 with `startItem=91`, `endItem=100`.
9. **Page size of 1 gives every item its own page** — 5 items, page size 1, page 3: `startItem=3`, `endItem=3`, `totalPages=5`, `hasPrevious=true`, `hasNext=true`.
10. **Negative totalItems is rejected at construction** — raises a domain error with message `"totalItems must be >= 0"`.
11. **Page size below 1 is rejected at construction** — raises a domain error with message `"pageSize must be >= 1"`.
12. **pageWindow centers on the current page when there is room** — 10 total pages, window size 5: page 1 returns `[1,2,3,4,5]`; page 5 returns `[3,4,5,6,7]`; page 10 returns `[6,7,8,9,10]`.
13. **pageWindow is clipped when totalPages is smaller than the window** — 4 total pages, window size 5, page 3: returns `[1,2,3,4]`.
14. **pageWindow on an empty dataset is empty** — 0 items, window size 5: returns `[]`.
