# Roman Numerals — TypeScript Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom: red → green for the opening triangulation, a reflect where the first design idea runs out of road, one big `green —` that promotes the table, and a long chain of `spec —` commits as the remaining scenarios pass on arrival.

The signature move of Roman Numerals is **the table that beats special cases**. Through `1 → 'I'`, `2 → 'II'`, `3 → 'III'` a simple `Record<number, string>` lookup keyed by the input looks fine. `4 → 'IV'` ruins it — adding a fourth key explains nothing about `IX`, `XL`, or `MCMLXXXIV`. The refactor promotes the lookup from `{input → output}` to a `ReadonlyArray<readonly [number, string]>` of `(value, symbol)` pairs with subtractives baked in as first-class entries, and a greedy subtract loop does the rest. Five `spec —` commits at the end prove the abstraction was right.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`087d7a5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/087d7a5) | scaffold | — | Empty Vitest project. No SUT yet — the first failing test will name it. |
| 2 | [`af7d4b0`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/af7d4b0) | red | low | `expect(toRoman(1)).toBe('I')`. Module resolution fails — `src/romanNumerals.ts` does not exist. |
| 3 | [`a21fba5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a21fba5) | green (fake-it) | low | `return 'I'`. Restraint. |
| 4 | [`f258903`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f258903) | red | low | `2 → 'II'`. |
| 5 | [`8bf5f7e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8bf5f7e) | green | low | `if (n === 2) return 'II';`. One hardcoded branch. |
| 6 | [`3cd99c5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3cd99c5) | red | low | `3 → 'III'`. |
| 7 | [`83801e5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/83801e5) | green | low | Extend into a `Record<number, string>` keyed by input: `{ 1:'I', 2:'II', 3:'III' }`. Tidier than a pile of ifs — but the key *is* the input. The `!` non-null assertion is the cost of `noUncheckedIndexedAccess` on an indexed record. |
| 8 | [`9a946d6`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9a946d6) | reflect | low | **The lookup-by-input doesn't generalize.** Three entries, three tests. Every new input would need its own key; `4` is next, and concatenating `'I'` `n` times is the tempting wrong turn. Noting the design pressure before the gear shift. |
| 9 | [`27b3934`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/27b3934) | red | low | `5 → 'V'`. |
| 10 | [`65bffa7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/65bffa7) | green | low | Add `5: 'V'` to the record. Same input-keyed shape — the design hasn't changed yet. |
| 11 | [`6dcc19a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6dcc19a) | red | low → middle | `4 → 'IV'`. Here's the cliff. |
| 12 | [`2fdb72c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2fdb72c) | green | middle | **Promote the table.** Replace the `Record` with a `ReadonlyArray<readonly [number, string]>` — `[[5, 'V'], [4, 'IV'], [1, 'I']]` — and a greedy subtract loop: `while (n >= value) { result += symbol; n -= value; }`. Subtractives are entries in the table, not special cases. **This is the kata's signature move.** The `2 → 'II'` and `3 → 'III'` scenarios, which previously had explicit entries, still pass — the loop emits them from `[1, 'I']` alone. |
| 13 | [`422db53`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/422db53) | red | middle | `10 → 'X'`. |
| 14 | [`1628cbe`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/1628cbe) | green | middle | Extend the table with `[10, 'X']` and `[9, 'IX']`. The greedy loop needs no changes. |
| 15 | [`1d55011`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/1d55011) | spec (passes on arrival) | middle | `9 → 'IX'`. **No code change.** First spec-on-arrival. |
| 16 | [`a8be136`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a8be136) | red | middle | `40 → 'XL'`. |
| 17 | [`bf60b78`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bf60b78) | green | middle | Extend the table with `[90, 'XC']`, `[50, 'L']`, `[40, 'XL']`. Three new entries. |
| 18 | [`9057df5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9057df5) | spec | middle | `90 → 'XC'`. No code change. Second spec-on-arrival. |
| 19 | [`d6bf2a5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d6bf2a5) | red | middle | `400 → 'CD'`. |
| 20 | [`720faec`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/720faec) | green | middle | Extend the table with `[900, 'CM']`, `[500, 'D']`, `[400, 'CD']`, `[100, 'C']`. The pattern repeats. |
| 21 | [`3a4c6fb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3a4c6fb) | spec | middle | `900 → 'CM'`. No code change. Third spec-on-arrival. |
| 22 | [`9fbe2aa`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9fbe2aa) | red | middle | `1000 → 'M'`. |
| 23 | [`dac19cc`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/dac19cc) | green | middle | Extend the table with `[1000, 'M']`. Full thirteen-entry mapping now present. |
| 24 | [`49b515f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/49b515f) | reflect | middle | **Table is complete.** Remaining scenarios should be pure spec. |
| 25 | [`dcfc62c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/dcfc62c) | spec | middle → high | `1984 → 'MCMLXXXIV'`. Fourth spec-on-arrival. |
| 26 | [`c53ca21`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c53ca21) | spec | high | `3999 → 'MMMCMXCIX'`. Fifth and final. Five `spec —` commits in a row. |

## How to run

```bash
cd roman-numerals/typescript
npm install
npx vitest run
```

## The takeaway

Two `reflect —` commits (steps 8 and 24) and **five `spec —` commits** (steps 15, 18, 21, 25, 26) are the signature of this kata. The table abstraction is data-driven: the greedy loop knows nothing about Roman numerals — it only knows about a descending list of `(value, symbol)` pairs. Subtractives are not exceptions to the rule; they are entries in the table.

TypeScript idioms used: `ReadonlyArray<readonly [number, string]>` for the mapping (strict typing communicates "this is a constant, indexed by order, never mutated"); `function` export for the SUT (no class needed — it's a pure function); tuple destructuring in the `for...of` loop; plain `+=` string concatenation (V8 handles small concatenations efficiently; a `join` is overkill for ≤ 16 symbols).
