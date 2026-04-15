# Kata Potter

Harry Potter book-basket pricing. Five different titles in a series, each €8. Buying multiple **distinct** titles together unlocks a set discount: 2 distinct → 5%, 3 → 10%, 4 → 20%, 5 → 25%. A basket with duplicates must be split into sets so that the **total** is minimised — which is where the interesting part lives. The famous case is `[2, 2, 2, 1, 1]` (two each of books 1–3, one each of 4–5): the greedy "always make the biggest set first" approach pairs a 5-set with a 3-set for €51.60, but two 4-sets for €51.20 is cheaper. Any correct solver has to find that.

`price(basket)` takes a counted collection of book ids 1–5 and returns the best (lowest) price in Euros.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. One primary entity (`Basket`), one small test-folder builder (`BasketBuilder`, 20–40 lines) whose chained setters read as the basket literal:

```
new BasketBuilder()
    .WithBook(1, 2)
    .WithBook(2, 2)
    .WithBook(3, 2)
    .WithBook(4, 1)
    .WithBook(5, 1)
    .Build()
```

reads as "two each of books 1–3, one each of books 4–5". That is the test literal for the famous scenario, so it deserves a sentence-shaped setup.

## Scope — Pure Domain Only

The reference covers the basket, the per-set discount table, and the grouping-optimisation pass. **No catalogue service, no currency localisation, no persistence, no order/checkout flow, no multi-series generalisation.** The hardcoded five-title / fixed-discount structure is the spec.

### Stretch Goals (Not Implemented Here)

- **Arbitrary series size** — generalise to N distinct titles with a configurable discount table (introduces a `PricingRules` collaborator)
- **Return the grouping breakdown** alongside the price — useful for receipts
- **Performance work** — large baskets (100+ books) where even the greedy-then-adjust pass benefits from memoisation

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference satisfies.
