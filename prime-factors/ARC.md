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
| 10 | S | low | `6` returns `[2, 3]` — passes on arrival; divide-out-2 leaves `n=3`, which the trailing `> 1` append handles. |
| 11 | S | low | `8` returns `[2, 2, 2]` — passes on arrival; the loop keeps pulling 2s. |
| 12 | R | low → middle | `9` returns `[3, 3]` — the real triangulation hinge. Divide-out-2 leaves `9`, which gets appended whole — wrong. |
| 13 | G | middle | Outer loop over `divisor`, starting at 2: while `n % divisor == 0` pull the factor, then `divisor += 1`. The algorithm is now complete. |
| 14 | Ref | middle | Empty commit. The hinge — the algorithm has generalized. Remaining scenarios should pass on arrival. |
| 15 | S | middle | `12` returns `[2, 2, 3]` — honest pass. |
| 16 | S | middle | `15` returns `[3, 5]` — honest pass. |
| 17 | S | high | `100` returns `[2, 2, 5, 5]` — honest pass. |
| 18 | S | high | `30030` returns `[2, 3, 5, 7, 11, 13]` — honest pass; proof the algorithm handles the first six primes without further code. |

Roughly 18 commits per language. The gear column is the teaching point — but this kata's real teaching point is the **chain of seven `spec —` commits** (3, 6, 8 in the early run; 12, 15, 100, 30030 after the hinge). That distribution is what distinguishes Prime Factors from every other triangulation kata: *the algorithm outran the test list twice*, with one genuine triangulation (`9`) splitting the chain into a "before the loop" and "after the loop" half. The commit log says so explicitly.

## Notes per phase

- **Scaffold** (1) — only the project skeleton. No file named after the SUT yet.
- **Red** commits only include the test. Run the test, confirm red, commit.
- **Green** commits write the minimum code to pass the test that was just committed red.
- **Refactor** commits keep the bar green; nothing changes behavior.
- **Reflect** commits are `git commit --allow-empty` with a message explaining the gear decision or the algorithmic insight. They are the written equivalent of a pause at the top of the stairs.
- **Spec (passes on arrival)** commits add a test that is immediately green because the previous triangulation generalized correctly. The commit body says so. This is honest TDD — and in Prime Factors, this is where the kata *lives*.
