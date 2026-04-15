# Bowling Game — Python Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the gear settles at middle once the two-mode index (frame / roll) is in place.

The signature move of Bowling Game is **the class that does not get written**. Two `reflect —` commits mark the moments the author was tempted to extract a `Frame` type and chose not to. The Frame concept is alive in the scoring loop — the roll cursor advances by one for a strike and by two for anything else. That *is* a frame.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`391c87f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/391c87f) | scaffold | — | Empty pytest project. No SUT yet. |
| 2 | [`6a7d481`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6a7d481) | red | low | `score([0] * 20) == 0`. Import fails — `score` doesn't exist. |
| 3 | [`bfd55ae`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bfd55ae) | green (fake-it) | low | `return 0`. Restraint. |
| 4 | [`aa6fa57`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/aa6fa57) | red | low | All ones scores twenty. |
| 5 | [`d207bac`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d207bac) | green | low | `return sum(rolls)`. Python's built-in does the job. |
| 6 | [`3b9a996`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3b9a996) | reflect | low | **First Frame-class temptation, declined.** The problem names a Frame. The test list has not asked for one — both scenarios are pure rolls-in, score-out. A class whose only client is its author is noise. |
| 7 | [`e75a18a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e75a18a) | red | low → middle | One spare scores 16. `sum` no longer cuts it — scoring needs lookahead. |
| 8 | [`a9c8633`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a9c8633) | green | middle | Two-at-a-time walk with spare detection. First lookahead. |
| 9 | [`0cc76eb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0cc76eb) | refactor | middle | Replace the `while` with `for _frame_index in range(10)`; split `frame_index` from `roll_index`. **The Frame concept now lives in this two-step advance — no class required.** |
| 10 | [`06abd97`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/06abd97) | red | middle | One strike scores 24. |
| 11 | [`0630950`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0630950) | green | middle | Strike branch: `roll_index += 1`, lookahead by two rolls. Three branches in the loop. |
| 12 | [`0d6c9cd`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0d6c9cd) | reflect | middle | **Second Frame-class temptation, declined again.** The 1-or-2 cursor advance *is* the frame. A `Frame` dataclass would replicate state the list already holds, and the tenth-frame bonus rolls (coming next) are just extra entries in the same list. |
| 13 | [`ca094d9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ca094d9) | spec (passes on arrival) | middle | Perfect game scores 300. Twelve strikes. **No new code** — the tenth frame's bonus rolls sit at the end of the roll list. |
| 14 | [`8b1243b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8b1243b) | spec (passes on arrival) | middle | All spares scores 150. **No new code** — the bonus rolls are data, not a code path. |
| 15 | [`0829679`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0829679) | reflect | middle | Empty commit. All six scenarios green. The SUT is a function, a `for` loop, and an integer cursor. Two points of temptation; both declined. |

## How to run

```bash
cd bowling-game/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

## The takeaway

Three reflect commits — two on the Frame-class temptation, one on completion. Two spec-on-arrival commits prove the tenth-frame bonus rolls needed no special code path.

**The class that did not get written is the teaching.** The `Frame` noun lives in `roll_index += 1` vs `roll_index += 2`. A `@dataclass` would not make that clearer — it would add indirection away from the single line that encodes the domain rule.

Python idioms: `list[int]` annotation (PEP 585); `range(10)` with `_frame_index` to advertise the counter is unused in the body; no list comprehension for scoring — the branching accumulator reads cleaner as a loop than as a reduce.
