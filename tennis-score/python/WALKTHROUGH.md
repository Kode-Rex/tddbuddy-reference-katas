# Tennis Score — Python Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the gear settles at middle once the raw point counts lift into a named `Score` `StrEnum`.

The signature move of Tennis Score is **the state machine that extracts itself**. Through Love, 15, 30, 40, Deuce the if/else ladder bends but doesn't break. Advantage is the cliff — conditions start comparing counts against each other. The refactor lifts the count into a seven-member enum (`LOVE | FIFTEEN | THIRTY | FORTY | DEUCE | ADVANTAGE | GAME`) and the formatter collapses to a dispatch. The very next scenario — game win at 4-2 — passes on arrival.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`c6e55b1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c6e55b1) | scaffold | — | Empty pytest project with `src/` layout. No SUT yet. |
| 2 | [`44229ab`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/44229ab) | red | low | `Match().score() == "Love-Love"`. Import fails. |
| 3 | [`eecf796`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/eecf796) | green (fake-it) | low | `return "Love-Love"`. Restraint. |
| 4 | [`a17f692`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a17f692) | red | low | 1-0 reads `"15-Love"`. |
| 5 | [`2c993c1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2c993c1) | green | low | One int, one `if`. |
| 6 | [`15e3ccc`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/15e3ccc) | red | low | 2-2 reads `"30-30"`. |
| 7 | [`116ff33`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/116ff33) | green | low | Two duplicate if ladders. Honest duplication. |
| 8 | [`ae4f2fc`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ae4f2fc) | refactor | low | Extract `_score_word(points)`. f-string formats both players via one call each. |
| 9 | [`419e9eb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/419e9eb) | red | low → middle | 3-3 reads `"Deuce"`. First non-formatted output. |
| 10 | [`609a54e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/609a54e) | green | middle | One extra `if` at the top of `score()`. Still reading raw counts. |
| 11 | [`b8609bb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b8609bb) | reflect | middle | **The if/else is bending.** Advantage is next — comparing counts against each other. Noting the design pressure. |
| 12 | [`b823196`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b823196) | red | middle | 4-3 reads `"Advantage Player 1"`. |
| 13 | [`a95af7b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a95af7b) | green | middle | Two mirrored conditions. Generalized Deuce to "both at 3+ and equal" while here. Formatter reads like arithmetic, not like tennis. |
| 14 | [`aa6dddb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/aa6dddb) | refactor | middle | **Extract `Score` `StrEnum`.** Seven named states; `_score_states()` returns a tuple. `score()` dispatches with `is` identity comparisons. The `_WORDS` dict maps the four display-as-number states to their strings. **The kata's signature move.** |
| 15 | [`ce95ae6`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ce95ae6) | spec (passes on arrival) | middle | 4-2 reads `"Game Player 1"`. **No new code.** The `FORTY + point → GAME` transition is already in `_score_states`. |
| 16 | [`a891009`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a891009) | reflect | middle | Point-level scoring is complete. Sets and match are tallies. |
| 17 | [`0dec310`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0dec310) | red+green | middle | 6-4 in games reads `"Set Player 1"`. Wrapper-level. `_game_just_won_by: int \| None` flag reports the winning moment. |
| 18 | [`b654d84`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b654d84) | red+green | middle | 6-4, 6-3 in sets reads `"Match Player 1"`. Second wrapper layer. Eight scenarios green. |

## How to run

```bash
cd tennis-score/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

## The takeaway

Two `reflect —` commits mark the cliff (step 11) and the design-complete moment (step 16). One `spec —` commit (step 15) proves the state machine absorbed Game with no new code.

**The state machine is the teaching.** Lift the count into a named enum; the formatter becomes a dispatch.

Python idioms: `StrEnum` (Python 3.11+) — members are both enum values and strings, which lets the enum carry its displayed spelling without an extra mapping for the `Love | Deuce | Advantage | Game` cases. A `_WORDS` dict for the four states whose string spelling differs from their name (`Fifteen → '15'`, `Thirty → '30'`, `Forty → '40'`, `Love → 'Love'`). Identity comparison (`is`) for enum members — Python idiomatic and faster than `==`. `int | None` unions (PEP 604) for the "just won" reporting flags. No dataclasses — `Match` has mutable identity by design; it's a stateful accumulator, not a value.
