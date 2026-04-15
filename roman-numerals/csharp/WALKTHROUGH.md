# Roman Numerals — C# Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green for the opening triangulation, a reflect where the first design idea runs out of road, one big `green —` that promotes the table, and then a long chain of `spec —` commits as the remaining scenarios pass on arrival.

The signature move of Roman Numerals is **the table that beats special cases**. Through `1 → I`, `2 → II`, `3 → III` a simple input-keyed dictionary looks fine. `4 → IV` ruins it — adding a fourth key explains nothing about `IX`, `XL`, or `MCMLXXXIV`. The refactor promotes the lookup from `{input → output}` to an ordered list of `(value, symbol)` pairs with subtractives baked in as first-class entries, and a greedy subtract loop does the rest. Five `spec —` commits at the end prove the abstraction was right.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`e8cbdb1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e8cbdb1) | scaffold | — | Empty xUnit solution. No SUT yet — the first failing test will name it. |
| 2 | [`9d1e038`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9d1e038) | red | low | `Roman.ToRoman(1).Should().Be("I")`. Compilation fails — no class yet. |
| 3 | [`8fc46df`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8fc46df) | green (fake-it) | low | `return "I"`. Restraint. |
| 4 | [`834bf8c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/834bf8c) | red | low | `2 → "II"`. The fake-it has to bend. |
| 5 | [`d53baf3`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d53baf3) | green | low | `if (n == 2) return "II";`. One hardcoded branch. No design yet. |
| 6 | [`a6df4a1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a6df4a1) | red | low | `3 → "III"`. |
| 7 | [`f3d800c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f3d800c) | green | low | Extend into a `Dictionary<int, string>` keyed by input: `{1:"I", 2:"II", 3:"III"}`. Tidier than a pile of ifs — but the key *is* the input. |
| 8 | [`b03b2d4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b03b2d4) | reflect | low | **The lookup-by-input doesn't generalize.** Three entries, three tests. Every new input would need its own key; `4` is next, and concatenating `I` `n` times is the tempting wrong turn. Noting the design pressure before the gear shift. |
| 9 | [`583bdad`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/583bdad) | red | low | `5 → "V"`. |
| 10 | [`160d1cb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/160d1cb) | green | low | Add `{ 5, "V" }` to the dictionary. Same input-keyed shape — the design hasn't changed yet. |
| 11 | [`dc77d2f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/dc77d2f) | red | low → middle | `4 → "IV"`. Here's the cliff. An input key of `4` with value `"IV"` works for this one test but explains nothing about `IX`, `XL`, `CD`, `CM`, or `MCMLXXXIV`. |
| 12 | [`cea9fb8`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/cea9fb8) | green | middle | **Promote the table.** Replace the input-keyed dictionary with an ordered `(int Value, string Symbol)[]` — `[(5, "V"), (4, "IV"), (1, "I")]` — and a greedy subtract loop: `while (n >= value) { result.Append(symbol); n -= value; }`. Subtractives are entries in the table, not special cases. **This is the kata's signature move.** The `2 → "II"` and `3 → "III"` scenarios, which previously had explicit entries, still pass — the loop emits them from `(1, "I")` alone. |
| 13 | [`72f1dfd`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/72f1dfd) | red | middle | `10 → "X"`. |
| 14 | [`8c55026`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8c55026) | green | middle | Extend the table with `(10, "X")` and `(9, "IX")`. The greedy loop needs no changes. Adding `IX` while here is honest — the table grows at one step and the spec-on-arrival lands at the next. |
| 15 | [`1ac8424`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/1ac8424) | spec (passes on arrival) | middle | `9 → "IX"`. **No code change.** The `(9, "IX")` entry added in the previous green makes the greedy loop emit `IX` directly. First spec-on-arrival. |
| 16 | [`525b528`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/525b528) | red | middle | `40 → "XL"`. |
| 17 | [`09cc2f1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/09cc2f1) | green | middle | Extend the table with `(90, "XC")`, `(50, "L")`, `(40, "XL")`. Three new entries in descending order. |
| 18 | [`7cce43c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7cce43c) | spec | middle | `90 → "XC"`. No code change. Second spec-on-arrival. |
| 19 | [`f5ae050`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f5ae050) | red | middle | `400 → "CD"`. |
| 20 | [`b139d49`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b139d49) | green | middle | Extend the table with `(900, "CM")`, `(500, "D")`, `(400, "CD")`, `(100, "C")`. The pattern repeats at every order of magnitude. |
| 21 | [`a83a2f2`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a83a2f2) | spec | middle | `900 → "CM"`. No code change. Third spec-on-arrival. |
| 22 | [`5b08e71`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/5b08e71) | red | middle | `1000 → "M"`. |
| 23 | [`1a3615a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/1a3615a) | green | middle | Extend the table with `(1000, "M")`. Full thirteen-entry mapping now present: `M, CM, D, CD, C, XC, L, XL, X, IX, V, IV, I`. |
| 24 | [`43834c9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/43834c9) | reflect | middle | **Table is complete.** Every order of magnitude from `I` to `M` with all six subtractives. No further entries required. Remaining scenarios should be pure spec. |
| 25 | [`c6937a8`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c6937a8) | spec | middle → high | `1984 → "MCMLXXXIV"`. **No code change.** Four orders of magnitude composing cleanly — `M`, `CM`, `L+XXX`, `IV`. Fourth spec-on-arrival. |
| 26 | [`362bf4c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/362bf4c) | spec | high | `3999 → "MMMCMXCIX"`. **No code change.** Maximum in-range input. Fifth spec-on-arrival. Five `spec —` commits in a row — proof the table abstraction was right from step 12 onward. |

## How to run

```bash
cd roman-numerals/csharp
dotnet test
```

## The takeaway

One `reflect —` commit marks the cliff (step 8) and a second marks the design-complete moment (step 24). **Five `spec —` commits** — steps 15, 18, 21, 25, 26 — are the signature of this kata. They are the honest moments where the algorithm outran the test list.

**The table is the teaching.** Three triangulations, one reflect, one gear-shifting green at step 12, and then every remaining scenario either extends the table by one to three entries or passes on arrival. The subtractives are not exceptions to the rule; they are entries in the table.

**The greedy loop is boring on purpose.** It does not know about Roman numerals. It knows about a list of `(value, symbol)` pairs in descending order. Give it a different table and it will do different things. That's the sign of a good abstraction — the algorithm is data-driven.

C# idioms used: `static class` for the pure-function SUT (`Roman.ToRoman`); `(int Value, string Symbol)[]` for the mapping (named tuples are the light-weight pair in C# since 7.0); `StringBuilder` for the accumulator; tuple deconstruction inside the `foreach`.
