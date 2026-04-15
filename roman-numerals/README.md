# Roman Numerals

**Teaching mode:** [Pedagogy](../README.md#1-pedagogy--learn-the-tdd-rhythm) — the fifth and final Pedagogy kata in the repo.

Convert a positive integer `1..3999` to its Roman numeral string. The teaching moment is the **mapping table that beats special cases** — the triangulation through `1 → I`, `2 → II`, `3 → III` tempts the solver into "repeat `I` `n` times", and `4 → IV` breaks it flat. The right refactor is not a special case for subtractives; it's promoting the lookup from "digits" to an **ordered mapping of descending values** with subtractive entries (`CM`, `CD`, `XC`, `XL`, `IX`, `IV`) baked in as first-class entries. Once the table is complete, the remaining scenarios — 9, 40, 90, 400, 900, 1984, 3999 — pass on arrival.

## Scope

**Arabic → Roman only.** That is the canonical TDD teaching direction for this kata. Roman → Arabic and input validation are stretch goals, not part of the nine scenarios.

## What this kata teaches

The **algorithm-from-a-table** move. In Prime Factors the algorithm emerges from a loop; here it emerges from a mapping. The pedagogical cliff is small but sharp — the first three scenarios look like "multiply a single symbol", the fourth ruins that idea, and the solver has to choose between:

- **Special-casing 4, 9, 40, 90, 400, 900** — a ladder of if-statements that grows with every subtractive.
- **Promoting the table** to pairs of `(value, symbol)` in descending order, including the subtractives as their own pairs, and running a greedy "subtract the largest fitting value, append its symbol, repeat" loop.

The second choice collapses the spec. The commit log makes that payoff visible — five `spec —` commits in a row at the end prove the table generalized.

## Gear arc

- **Low gear** for the opening triangulations: `1 → I`, `2 → II`, `3 → III`. Fake-it, then a tiny lookup keyed by the input integer.
- **Gear shift** at `4 → IV`. The lookup-by-input approach won't scale — four would need a fourth key, five a fifth, and the structure doesn't extend to 1984. Introduce the `(value, symbol)` pair list with `(5, "V"), (4, "IV"), (1, "I")` and a greedy subtract loop.
- **Middle gear** for `10 → X`, `40 → XL`, `400 → CD`, `1000 → M`. Each is one table entry. The algorithm doesn't change.
- **Spec-on-arrival** for `9 → IX`, `90 → XC`, `900 → CM`, `1984 → MCMLXXXIV`, `3999 → MMMCMXCIX`. Once the table has the pairs and the loop is greedy, these pass without touching code.

## Why subtractives belong in the table

The alternative — treating subtractives as a post-pass or a special case — looks reasonable for `4` alone, but it generalises poorly. By `900 → CM` the solver is maintaining two lookup structures and a set of rules about when to consult each. Promoting `CM` to "a thing that is worth 900" is the same move as recognising `X` is "a thing that is worth 10". The table beats the ladder because it **removes the special case by renaming it**.

This is the Roman Numerals pedagogy in one sentence: *the subtractives are not exceptions to the rule; they are entries in the table.*

## Files

- [`SCENARIOS.md`](./SCENARIOS.md) — the shared spec (9 numbered scenarios plus intermediate triangulation).
- [`ARC.md`](./ARC.md) — the intended commit arc all three languages follow.
- `csharp/`, `typescript/`, `python/` — three idiomatic implementations with per-language `WALKTHROUGH.md`.

## How to read

1. Skim `SCENARIOS.md`.
2. Pick a language; open its `WALKTHROUGH.md`.
3. Walk the commit table. Stop at the `green —` row that introduces the `(value, symbol)` pair list — that's the "lookup just became an algorithm" moment.
4. Read the chain of `spec —` commits at the end. That's proof the table generalized.
