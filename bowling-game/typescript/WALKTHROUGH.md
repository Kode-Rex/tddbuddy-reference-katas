# Bowling Game — TypeScript Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the gear settles at middle once the two-mode index (frame / roll) is in place.

The signature move of Bowling Game is **the class that does not get written**. Two `reflect —` commits mark the moments the author was tempted to extract a `Frame` type and chose not to. The Frame concept is alive in the scoring loop — the roll cursor advances by one for a strike and by two for anything else. That *is* a frame.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`3c61fca`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3c61fca) | scaffold | — | Empty Vitest project. No SUT yet. |
| 2 | [`9fdb0fa`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9fdb0fa) | red | low | `score(new Array(20).fill(0))` → `0`. Compilation fails — `score` doesn't exist. |
| 3 | [`88386f2`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/88386f2) | green (fake-it) | low | `return 0`. Restraint. |
| 4 | [`a3b409c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a3b409c) | red | low | All ones scores twenty. |
| 5 | [`bf33602`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bf33602) | green | low | `rolls.reduce((total, pins) => total + pins, 0)`. Idiomatic TS sum. |
| 6 | [`ce0e772`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ce0e772) | reflect | low | **First Frame-class temptation, declined.** The problem names a Frame. The test list has not asked for one — both scenarios are pure rolls-in, score-out. A class whose only client is its author is noise. |
| 7 | [`43fa999`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/43fa999) | red | low → middle | One spare scores 16. `reduce` no longer suffices — scoring needs lookahead. |
| 8 | [`d9eebe5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d9eebe5) | green | middle | Two-at-a-time walk with spare detection. `rolls[i]! + rolls[i + 1]!` — the non-null assertions are the cost of `noUncheckedIndexedAccess` honesty; the domain guarantees the indices. |
| 9 | [`d58c0cc`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d58c0cc) | refactor | middle | Split `frameIndex` from `rollIndex`. Ten frame iterations; the roll cursor advances by two within each. **The Frame concept now lives in this two-step advance — no class required.** |
| 10 | [`317dcef`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/317dcef) | red | middle | One strike scores 24. |
| 11 | [`8fbc30e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8fbc30e) | green | middle | Strike branch: cursor += 1, lookahead by two rolls. Three branches in the loop. |
| 12 | [`be22d69`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/be22d69) | reflect | middle | **Second Frame-class temptation, declined again.** The 1-or-2 cursor advance *is* the frame. A class would replicate state the list already holds, and the tenth-frame bonus rolls (coming next) are just extra entries in the same list. |
| 13 | [`96ec9c2`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/96ec9c2) | spec (passes on arrival) | middle | Perfect game scores 300. Twelve strikes. **No new code** — the tenth frame's bonus rolls sit at the end of the roll list and the strike branch's lookahead consumes them naturally. |
| 14 | [`820dea5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/820dea5) | spec (passes on arrival) | middle | All spares scores 150. Twenty-one rolls of 5. **No new code** — the bonus rolls are data, not a code path. |
| 15 | [`a6d005a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a6d005a) | reflect | middle | Empty commit. All six scenarios green. The SUT is a function, a `for` loop, and an integer cursor. Two points of temptation; both declined. |

## How to run

```bash
cd bowling-game/typescript
npm install
npx vitest run
```

## The takeaway

Three reflect commits — two on the Frame-class temptation, one on completion. Two spec-on-arrival commits prove the tenth-frame bonus rolls needed no special code path.

**The class that did not get written is the teaching.** The `Frame` noun lives in `rollIndex += 1` vs `rollIndex += 2`. A class would not make that clearer — it would add a layer of indirection away from the one line that encodes the whole domain rule.

TypeScript idioms: `number[]` as the parameter type; non-null assertions (`rolls[i]!`) as the honest cost of `noUncheckedIndexedAccess: true`; no `Array.prototype.reduce` in the final loop — the index-based walk with lookahead reads cleaner than a functional accumulator when the accumulator has to peek ahead in the source array.
