# Recipe Calculator — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- A **recipe** is a mapping from ingredient name to a non-negative quantity.
- **Scale** takes a recipe and a non-negative `factor` and returns a new recipe where every quantity is multiplied by `factor`. Ingredient names are preserved.
- An **empty recipe** scales to an empty recipe for any factor.
- A **factor of `1`** returns the same quantities (identity).
- A **factor of `0`** returns zero for every ingredient.
- A negative `factor` is invalid and raises a domain-appropriate error.

The kata's bonus tracks (unit conversion, dietary restrictions, substitutions, shopping lists, nutrition) are **not** implemented here — see the top-level [`README.md`](README.md).

## Test Scenarios

1. **Empty recipe scales to empty recipe** — with any factor (e.g. `2`), the result is an empty map
2. **Single ingredient doubles when factor is 2** — `{ flour: 100 }` at factor `2` becomes `{ flour: 200 }`
3. **Single ingredient halves when factor is 0.5** — `{ flour: 100 }` at factor `0.5` becomes `{ flour: 50 }`
4. **Multiple ingredients all scale by the same factor** — `{ flour: 200, sugar: 100, butter: 50 }` at factor `3` becomes `{ flour: 600, sugar: 300, butter: 150 }`
5. **Factor of 1 returns identical quantities** — `{ flour: 100, sugar: 50 }` at factor `1` is unchanged
6. **Factor of 0 zeroes every quantity** — `{ flour: 100, sugar: 50 }` at factor `0` becomes `{ flour: 0, sugar: 0 }`
7. **Fractional quantities scale without being rounded** — `{ salt: 1.5 }` at factor `3` becomes `{ salt: 4.5 }`
8. **Negative factor is rejected** — scaling any recipe by `-1` raises a domain-appropriate error
