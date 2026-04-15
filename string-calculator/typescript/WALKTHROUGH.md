# String Calculator — TypeScript Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching — read it top to bottom to feel the rhythm. Gears shift from **low** (fake-it, triangulate one data point at a time) to **middle** (one scenario per cycle) to **high** (obvious implementation; later scenarios pass on arrival).

The TypeScript arc matches the C# arc scenario-for-scenario; only the idioms differ. Where C# uses LINQ `Sum`, TypeScript uses `Array.reduce`; where C# uses `Regex.Matches`, TypeScript uses `String.matchAll`; otherwise the moments are identical.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`1821f0b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/1821f0b7ad0c72a3ff50997addd497636d2f28cc) | scaffold | — | Vitest 1.6 on Node 20, ESM, strict TS. No SUT yet. |
| 2 | [`9666016`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9666016d67475b536e1c8862c64cb57b9a41b2c3) | red | low | First test imports `add` from a module that doesn't exist. |
| 3 | [`408953a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/408953a39f7e179afc7556bc0738d752a6098baf) | green (fake-it) | low | `return 0`. The `_numbers` prefix signals deliberately unused. |
| 4 | [`8098de8`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8098de89c5be933b67889dffaef40fd7a10d2645) | red | low | Single number. Second data point. |
| 5 | [`680524d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/680524dda74228b9f9dd8af538f676d582a9e771) | green | low | `parseInt` with empty guard. |
| 6 | [`8a582df`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8a582df6ba3bfa5f771a27932971d451119da2a2) | red | low | Two numbers. |
| 7 | [`7d21032`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7d21032ae7cca2f6be897871a0b71ed862d0ec90) | green | low | `split(',').reduce(...)` — already generalizes. |
| 8 | [`bf68f40`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bf68f40d0fb63ecccdb2fa9df6d9bc449fd17832) | reflect | low → middle | Empty commit. Triangulation complete — shift up. |
| 9 | [`d9bb023`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d9bb0232c9c965d41e760faf014cf7147f993519) | spec (passes on arrival) | middle | Many numbers — honest pass. |
| 10 | [`34cb0a5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/34cb0a5c2aede12d1d64353ed59f72a4677c5243) | red | middle | Newline delimiter. |
| 11 | [`80f6378`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/80f6378560a05b86d57d207baae745ba6c7506c3) | green | middle | Split on regex `/[,\n]/`. |
| 12 | [`e6038ca`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e6038ca23a267b4721fe8d56f50bd2f9fccc9e2b) | reflect | middle | Empty commit. Shape is set; one commit per scenario from here. |
| 13 | [`8d01f73`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8d01f73eb22f2d64194eea6ea34cc0b08befc416) | red+green | middle | Custom delimiter; inline header parse. |
| 14 | [`a200ec4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a200ec48a4f88a0c3f5c37b5aacb979463f859aa) | refactor | middle | Extract `delimiterParser`. `add()` reads as *parse, split, sum*. |
| 15 | [`62f8a59`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/62f8a593b9bb384772107b43c9ff21dcfc0ce653) | red+green | middle | Negative rejected; `.join(', ')` ready for multi. |
| 16 | [`e53490c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e53490c911c17a72d81e3a357ec008a6045c6146) | spec (passes on arrival) | middle | Multi-negative — honest pass. |
| 17 | [`7aa6547`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7aa6547ef05c1b5fad450fdcee340b0de576db72) | red+green | middle | Numbers > 1000 ignored. `.filter(n => n <= 1000)` before reduce. |
| 18 | [`f13b016`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f13b01639e2d0fda4c99ce8073e9475480a8f6fa) | red+green | high | Any-length bracketed. `matchAll` + `escapeForRegex` + alternation. |
| 19 | [`c5a068b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c5a068bca321dffae9d41613261a491d7a3394a5) | spec (pass on arrival) | high | Multiple and multi-char delimiters — both pass immediately. |

## How to run

```bash
cd string-calculator/typescript
npm install
npx vitest run
```

## The takeaway

The TypeScript idioms that matter here: **`matchAll` returns an iterator**, so `Array.from(..., m => m[1]!)` captures the groups; **RegExp string construction needs escaping** (`escapeForRegex`), because user-supplied delimiter literals cannot be trusted as regex patterns. Everything else is the same teaching arc as C#.
