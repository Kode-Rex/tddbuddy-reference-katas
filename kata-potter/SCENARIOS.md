# Kata Potter — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **basket pricing only**: the five-title series, the fixed discount table, and the grouping-optimisation pass. Configurable series size, persistence, receipts with breakdowns, and checkout flow are **out of scope** — see [`README.md`](README.md#scope--pure-domain-only) for the stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Book** | One of five distinct titles in the series, identified by id 1–5. |
| **Basket** | A counted collection of books; the customer's shopping cart. Exposes `price()` in Euros. |
| **Base Price** | Price of one book with no discount: **€8.00**. |
| **Set** | A grouping of up to five *distinct* books. A set of `k` books costs `k * BasePrice * (1 - DiscountFor(k))`. |
| **Discount Table** | Fixed: 1 book → 0%, 2 distinct → 5%, 3 → 10%, 4 → 20%, 5 → 25%. |
| **Grouping** | A partition of the basket into sets. The price of a grouping is the sum of its set prices. |
| **Optimal Grouping** | The grouping with the minimum total price. `price(basket)` returns the optimal grouping's price. |
| **BasketBuilder** | Test-folder fluent builder. Chained `.withBook(series, count)` calls place books into the basket; reads as a direct literal of the basket under test. |

## Domain Rules

- Book ids are integers `1..5`. The basket does not accept ids outside that range — `price` is undefined for malformed input; the builder raises on out-of-range ids (`BookOutOfRangeException` / `BookOutOfRangeError`). Counts must be non-negative; a zero count is equivalent to not placing the book.
- Each book costs **€8.00** before discount.
- The per-set discount table is **fixed**: 1 → 0%, 2 → 5%, 3 → 10%, 4 → 20%, 5 → 25%. No other set sizes exist; the basket only ever contains 1–5 distinct titles per set because there are only five titles.
- `price(basket)` returns the price of the **optimal grouping** (lowest total).
- The pricing result is a decimal Euro value. Tests compare with a tolerance of `0.001` (or exact equality where values are terminating decimals — the discount table produces terminating decimals at two places).
- The **greedy** heuristic (always make the biggest set first) is *wrong* in the general case. Pairing a 5-set with a 3-set always costs more than two 4-sets (`5 * 8 * 0.75 + 3 * 8 * 0.90 = 51.60` vs. `2 * 4 * 8 * 0.80 = 51.20`). A correct solver adjusts for this.

### Exception Messages

The exception message string is **identical byte-for-byte** across all three languages; the exception type names differ by language idiom.

| Rule | Message |
|------|---------|
| builder given book id outside 1–5 | `"book id must be between 1 and 5"` |

## Test Scenarios

1. **Empty basket costs zero** — `price(empty) == 0.00`.
2. **One book costs the base price** — one copy of book 1 → `€8.00`.
3. **Two copies of the same book get no discount** — two of book 1 → `€16.00` (two sets of one, no duplicate counts as "distinct").
4. **Two distinct books get the 5% discount** — one of book 1 and one of book 2 → `€15.20` (`2 * 8 * 0.95`).
5. **Three distinct books get the 10% discount** — one each of books 1, 2, 3 → `€21.60` (`3 * 8 * 0.90`).
6. **Four distinct books get the 20% discount** — one each of books 1–4 → `€25.60` (`4 * 8 * 0.80`).
7. **Five distinct books get the 25% discount** — one each of books 1–5 → `€30.00` (`5 * 8 * 0.75`).
8. **Duplicates are priced separately from the discounted set** — two of book 1 and one of book 2 → `€23.20` (one 2-set at €15.20 plus one single at €8.00).
9. **Two copies of every book makes two 5-sets** — two each of books 1–5 → `€60.00` (`2 * 5 * 8 * 0.75`).
10. **Greedy-fails basket prefers two 4-sets over a 5+3 pair** — two each of books 1, 2, 3 plus one each of books 4, 5 → `€51.20` (two 4-sets at €25.60 each), **not** `€51.60` (5-set €30.00 + 3-set €21.60).
11. **Bigger greedy-fails basket** — three each of books 1, 2, 3 and two each of books 4, 5 → `€81.20` (one 5-set at €30.00 plus two 4-sets at €25.60 each), **not** the `€81.60` greedy result (two 5-sets at €30.00 plus one 3-set at €21.60).
12. **BasketBuilder rejects book ids outside 1..5** — `withBook(0, 1)` and `withBook(6, 1)` both raise with message `"book id must be between 1 and 5"`.
