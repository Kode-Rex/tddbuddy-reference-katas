# Change Maker — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- `makeChange(amount, denominations)` returns the list of coins that totals `amount`, picked **greedily** from largest to smallest denomination.
- `denominations` is assumed to be sorted in **descending** order (largest first). Callers pass their currency's coin set directly.
- `amount` is a non-negative integer expressed in the currency's smallest unit (US cents, British pence, Norwegian øre).
- An `amount` of `0` returns an empty list.
- The returned list preserves descending order by denomination (25, 25, 10, 5, 1 for 66 cents under US coins).
- Greedy is correct for the three canonical currencies below; arbitrary denomination sets are out of scope (see the top-level [`README.md`](README.md)).

## Canonical Denominations

- **US Dollar (cents):** 25, 10, 5, 1
- **British Pound (pence):** 50, 20, 10, 5, 2, 1
- **Norwegian Krone (øre):** 20, 10, 5, 1

## Test Scenarios

1. **Zero amount returns no coins** — any denomination set, amount `0` → `[]`
2. **US: 1 cent is a single penny** — `[1]`
3. **US: 5 cents is a single nickel** — `[5]`
4. **US: 25 cents is a single quarter** — `[25]`
5. **US: 30 cents is a quarter and a nickel** — `[25, 5]`
6. **US: 41 cents is a quarter, a dime, a nickel, and a penny** — `[25, 10, 5, 1]`
7. **US: 66 cents is two quarters, a dime, a nickel, and a penny** — `[25, 25, 10, 5, 1]`
8. **UK: 43 pence is 20, 20, 2, 1** — exercises the British 20p denomination
9. **UK: 88 pence is 50, 20, 10, 5, 2, 1** — one of each British coin
10. **Norway: 37 øre is 20, 10, 5, 1, 1** — exercises the Norwegian coin set
11. **Norway: 40 øre is two 20-øre coins** — two of the highest denomination
