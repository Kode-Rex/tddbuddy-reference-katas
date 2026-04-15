# Prime Factors — TypeScript Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching — read it top to bottom to feel the rhythm. Gears shift from **low** (fake-it, triangulate one data point at a time) to **middle** (obvious implementation) to **high** (later scenarios pass on arrival).

The TypeScript arc matches the C# arc commit-for-commit. The signature move is the **run of spec-on-arrival commits** after the algorithm clicks at step 13 — each one saying explicitly *no new code was needed*.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`265e2b1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/265e2b1) | scaffold | — | Vitest 1.6 on Node 20, ESM, strict TS. No SUT yet. |
| 2 | [`66a444d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/66a444d) | red | low | First test imports `generate` from a module that doesn't exist. |
| 3 | [`937fb1a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/937fb1a) | green (fake-it) | low | `return []`. The `_n` prefix signals deliberately unused. |
| 4 | [`b7b9823`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b7b9823) | red | low | `generate(2) === [2]`. |
| 5 | [`57d4d2e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/57d4d2e) | green | low | `if (n > 1) return [n];` — still mostly fake. |
| 6 | [`86f90d7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/86f90d7) | spec (passes on arrival) | low | `generate(3) === [3]` — the `> 1` guard covers it. |
| 7 | [`077704e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/077704e) | red | low | `generate(4) === [2, 2]` — first composite. |
| 8 | [`e632e74`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e632e74) | green | low | Divide-out-2 loop: push 2 while even, then append remaining `n` if > 1. |
| 9 | [`11f5fe7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/11f5fe7) | reflect | low | Empty commit. Pattern is forming. |
| 10 | [`c4cfa9e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c4cfa9e) | spec (passes on arrival) | low | `generate(6) === [2, 3]` — honest pass. |
| 11 | [`dc2a21c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/dc2a21c) | spec (passes on arrival) | low | `generate(8) === [2, 2, 2]` — powers of two handled by the while. |
| 12 | [`690236c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/690236c) | red | low → middle | `generate(9) === [3, 3]` — **the hinge.** First odd composite. |
| 13 | [`be3860a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/be3860a) | green | middle | `for (let divisor = 2; n > 1; divisor++)` wrapping the while. **Algorithm complete.** |
| 14 | [`fe18c99`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/fe18c99) | reflect | middle | Empty commit. End of algorithmic pressure; rest is proof-by-example. |
| 15 | [`689f480`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/689f480) | spec (passes on arrival) | middle | `generate(12) === [2, 2, 3]`. No new code. |
| 16 | [`e2165bb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e2165bb) | spec (passes on arrival) | middle | `generate(15) === [3, 5]`. No new code. |
| 17 | [`c28c1a9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c28c1a9) | spec (passes on arrival) | high | `generate(100) === [2, 2, 5, 5]`. No new code. |
| 18 | [`8203e6b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8203e6b) | spec (passes on arrival) | high | `generate(30030) === [2, 3, 5, 7, 11, 13]`. Six distinct primes, no new code. |

## How to run

```bash
cd prime-factors/typescript
npm install
npx vitest run
```

## The takeaway

Seven spec-on-arrival commits. One real algorithmic breakthrough at step 13. The TypeScript idioms are minimal here: `number[]` as the return type, `factors.push(...)` as the append, plain `for` / `while` loops. No `Array.from`, no functional folds — the imperative shape *is* the algorithm Uncle Bob teaches, and reading it out loud is the point.

The idiomatic divergence from C# is the type: `number[]` in TS versus `IReadOnlyList<int>` in C#. Both are correct for the languages. Neither is worth a refactor commit to rename — the test list expresses the contract more loudly than the signature does.
