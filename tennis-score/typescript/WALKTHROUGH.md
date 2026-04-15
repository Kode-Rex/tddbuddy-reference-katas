# Tennis Score — TypeScript Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the gear settles at middle once the raw point counts lift into a named `Score` string-literal union.

The signature move of Tennis Score is **the state machine that extracts itself**. Through Love, 15, 30, 40, Deuce the if/else ladder bends but doesn't break. Advantage is the cliff — conditions start comparing counts against each other. The refactor lifts the count into a seven-member union (`'Love' | 'Fifteen' | 'Thirty' | 'Forty' | 'Deuce' | 'Advantage' | 'Game'`) and the formatter collapses to a dispatch. The very next scenario — game win at 4-2 — passes on arrival.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`0ec2da8`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0ec2da8) | scaffold | — | Empty Vitest project. No SUT yet. |
| 2 | [`7813028`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7813028) | red | low | `new Match().score()` → `'Love-Love'`. Compilation fails. |
| 3 | [`03c3f6a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/03c3f6a) | green (fake-it) | low | `return 'Love-Love'`. Restraint. |
| 4 | [`379a32e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/379a32e) | red | low | 1-0 reads `'15-Love'`. |
| 5 | [`6bbde12`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6bbde12) | green | low | `pointWonBy(player: 1 \| 2)` typed with a literal-union parameter — no nullable-number gymnastics needed. One `if`. |
| 6 | [`8da5498`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8da5498) | red | low | 2-2 reads `'30-30'`. |
| 7 | [`1455d36`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/1455d36) | green | low | Two integers, two duplicate if ladders. Honest duplication. |
| 8 | [`0b3f9a2`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0b3f9a2) | refactor | low | Extract `scoreWord(points: number): string`. Template literal formats both players via one call each. |
| 9 | [`b63cec5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b63cec5) | red | low → middle | 3-3 reads `'Deuce'`. First non-formatted output. |
| 10 | [`237f11e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/237f11e) | green | middle | One extra `if` at the top of `score()`. Still reading raw counts. |
| 11 | [`d9639a9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d9639a9) | reflect | middle | **The if/else is bending.** Advantage is next — a player at 4+ leading by exactly 1. That's comparing counts against each other, not against fixed values. Noting the design pressure; one more scenario and the refactor is unavoidable. |
| 12 | [`ea325ac`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ea325ac) | red | middle | 4-3 reads `'Advantage Player 1'`. |
| 13 | [`4664664`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/4664664) | green | middle | Two mirrored `if`s on `p1 - p2 === 1` / `p2 - p1 === 1`. Generalized Deuce to "both at 3+ and equal" while here. The formatter reads like arithmetic, not like tennis. |
| 14 | [`8435b90`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8435b90) | refactor | middle | **Extract `Score` string-literal union.** `'Love' \| 'Fifteen' \| 'Thirty' \| 'Forty' \| 'Deuce' \| 'Advantage' \| 'Game'`. `scoreStates()` returns a `[Score, Score]` tuple; `score()` dispatches on it. The conditions that used to compare integers against each other now compare union members against named values — the formatter reads like tennis again. **This is the kata's signature move.** |
| 15 | [`6b8292d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6b8292d) | spec (passes on arrival) | middle | 4-2 reads `'Game Player 1'`. **No new code.** The `Forty + point → Game` transition is already encoded: p1 at 4+ with p2 below 3 returns `'Game'`. |
| 16 | [`36f3744`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/36f3744) | reflect | middle | Point-level scoring is complete. Sets and match are tallies, not a second state machine. |
| 17 | [`ed92fb0`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ed92fb0) | red+green | middle | 6-4 in games reads `'Set Player 1'`. Wrapper-level addition. A `gameJustWonBy: 1 \| 2 \| null` flag reports the winning moment before the next point rolls the counter forward. |
| 18 | [`b24d764`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b24d764) | red+green | middle | 6-4, 6-3 in sets reads `'Match Player 1'`. Second wrapper layer. Eight scenarios green. |

## How to run

```bash
cd tennis-score/typescript
npm install
npx vitest run
```

## The takeaway

Two `reflect —` commits mark the cliff (step 11) and the design-complete moment (step 16). One `spec —` commit (step 15) proves the state machine absorbed Game with no new code.

**The state machine is the teaching.** Conditions that compare counts against each other — not against fixed values — are the cliff. Lift the count into a named union; the formatter becomes a dispatch.

TypeScript idioms: a **string-literal union** (not an enum, not a discriminated union with payloads) — seven plain string members. `as const` isn't needed because the union members are the values themselves. `[Score, Score]` tuple return on `scoreStates()`. The `pointWonBy(player: 1 | 2)` parameter uses a numeric literal union so the method never has to validate — the type system does. `noUncheckedIndexedAccess: true` doesn't bite because the state machine dispatches on known union members, not indexed lookups.
