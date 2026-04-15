# String Calculator — Python Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Read it end to end to feel the rhythm — red → green, red → green, refactor, reflect — and the gear shifts from **low** (fake-it, one data point at a time) to **middle** (scenario per cycle) to **high** (obvious implementation; later scenarios pass on arrival).

Python idioms replace LINQ/reduce: `sum()` with a generator expression, `re.split` for regex tokenizing, `re.escape` for user-supplied literals, and `ValueError` as the idiomatic carrier for a domain validation failure.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`54048a8`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/54048a8b1c35ae3949d99b638fcc4ad8bdf2e591) | scaffold | — | pytest + `src/` layout, pyproject. No SUT yet. |
| 2 | [`eb7486d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/eb7486dc579bb4f87957a1f0b66ee3d8cce61a5e) | red | low | Import of `calculator` fails at collection — that's the red. |
| 3 | [`e5b3c6e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e5b3c6eae23da9b245277f236f8c1e2d6b2b3f4c) | green (fake-it) | low | `return 0`. |
| 4 | [`588c7bb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/588c7bb131ff9099f33a2338eed80042862e480f) | red | low | Single number. |
| 5 | [`912aa45`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/912aa4528de8de75397bcdff7409ac0e5dc7d823) | green | low | `int(numbers)` with empty guard. |
| 6 | [`2efcd27`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2efcd27695ffb4820640f066e84f18628f153eae) | red | low | Two numbers. |
| 7 | [`4ebc6cb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/4ebc6cb7ba5f32941f71ae595643a0abcf56aab1) | green | low | `sum(int(t) for t in s.split(","))` — already general. |
| 8 | [`73318b0`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/73318b0b1711d8c52651350fd1c09e5994c138d5) | reflect | low → middle | Empty commit. Shift up. |
| 9 | [`f03a276`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f03a27636601f1981798358cd4ca8aa16a8c8696) | spec (passes on arrival) | middle | Many numbers — honest pass. |
| 10 | [`d5dd393`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d5dd39357e7e80a90b9be3df41eb808c09612481) | red | middle | Newline delimiter. |
| 11 | [`85ef1ea`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/85ef1eaf23f7ae74511055f497ce9d303ea37167) | green | middle | `re.split(r"[,\n]", s)`. |
| 12 | [`5e61a18`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/5e61a18a1545886ebf25cb798521e862001ee50a) | reflect | middle | Empty commit. Shape set. |
| 13 | [`e92cadf`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e92cadf394e9d91ef5d34d8842f57372062494e9) | red+green | middle | Custom delimiter; inline header parse with `re.escape`. |
| 14 | [`89ae48b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/89ae48b77e3476fb66d199f5317dc8c1a0cdc376) | refactor | middle | Extract `delimiter_parser`; `NamedTuple` keeps the return self-documenting. |
| 15 | [`5ba482d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/5ba482d9c8d5dad20069dec8f430f75fd9b5a2b6) | red+green | middle | Negative raises `ValueError`; `", ".join(...)` ready for multi. |
| 16 | [`0224071`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/02240717d2cdefc0f6b3da9bcc38e5fd051ebaad) | spec (passes on arrival) | middle | Multi-negative — honest pass. |
| 17 | [`852299f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/852299fccb0ded6009a31027c431be89c80b1cf2) | red+green | middle | Numbers > 1000 ignored; filter in the final generator. |
| 18 | [`029eeeb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/029eeeb727e37746e86f9d65daee5184cea72afe) | red+green | high | Any-length bracketed. `re.findall` + `re.escape` + alternation. |
| 19 | [`f93b3a3`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f93b3a3ddce0c3e0615e440d5ed3cf3ddf92c9f9) | spec (pass on arrival) | high | Multiple and multi-char delimiters — both pass immediately. |

## How to run

```bash
cd string-calculator/python
python3 -m venv .venv
.venv/bin/pip install pytest
.venv/bin/pytest
```

## The takeaway

Python's `re.escape` + `"|".join(...)` makes the delimiter-alternation pattern about three lines shorter than the C# equivalent. The idiom also names itself: "escape each literal, then alternate." That's domain-adjacent wording; the tests read like the spec reads.
