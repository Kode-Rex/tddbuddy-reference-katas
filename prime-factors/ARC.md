# Prime Factors — Teaching Arc

The intended commit sequence all three language implementations follow. Language idioms diverge at the line level, but the arc — what lands at what gear, where the reflections sit, and the unmistakable chain of `spec —` commits — is shared.

Legend: **R** red, **G** green, **F** refactor, **Ref** reflect (empty commit), **S** spec-on-arrival (test passes immediately because a prior triangulation generalized).

| # | Phase | Gear | What |
|---|-------|------|------|
| 1 | scaffold | — | Empty solution/project, no tests. |
| 2 | R | low | `1` returns empty list. |
| 3 | G (fake-it) | low | `return []`. |
| 4 | R | low | `2` returns `[2]`. |
| 5 | G | low | `if n > 1: return [n]` — still mostly fake. |
| 6 | S | low | `3` returns `[3]` — passes on arrival; the `> 1` guard generalized. |
| 7 | R | low | `4` returns `[2, 2]`. |
| 8 | G | low | Divide-out-2 loop: pull 2s into a list while `n % 2 == 0`, then append remaining `n` if `> 1`. |
| 9 | Ref | low | Empty commit. Pattern is forming — a loop is peeking through. |
| 10 | R | low → middle | `6` returns `[2, 3]` — the key triangulation. Forces a divisor other than 2. |
| 11 | G | middle | Outer loop over `divisor`, starting at 2: while `n % divisor == 0` pull the factor, then `divisor += 1`. The algorithm is now complete. |
| 12 | Ref | middle | Empty commit. The hinge — the algorithm has generalized. Remaining scenarios should pass on arrival. |
| 13 | S | middle | `8` returns `[2, 2, 2]` — honest pass. |
| 14 | S | middle | `9` returns `[3, 3]` — honest pass. |
| 15 | S | middle | `12` returns `[2, 2, 3]` — honest pass. |
| 16 | S | middle | `15` returns `[3, 5]` — honest pass. |
| 17 | S | high | `100` returns `[2, 2, 5, 5]` — honest pass. |
| 18 | S | high | `30030` returns `[2, 3, 5, 7, 11, 13]` — honest pass; proof the algorithm handles the first six primes without further code. |

Roughly 18 commits per language. The gear column is the teaching point — but this kata's real teaching point is the **run of six `spec —` commits at the end**. That sequence is what distinguishes Prime Factors from every other triangulation kata: *the algorithm outran the test list*, and the commit log says so explicitly.

## Notes per phase

- **Scaffold** (1) — only the project skeleton. No file named after the SUT yet.
- **Red** commits only include the test. Run the test, confirm red, commit.
- **Green** commits write the minimum code to pass the test that was just committed red.
- **Refactor** commits keep the bar green; nothing changes behavior.
- **Reflect** commits are `git commit --allow-empty` with a message explaining the gear decision or the algorithmic insight. They are the written equivalent of a pause at the top of the stairs.
- **Spec (passes on arrival)** commits add a test that is immediately green because the previous triangulation generalized correctly. The commit body says so. This is honest TDD — and in Prime Factors, this is where the kata *lives*.
