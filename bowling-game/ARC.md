# Bowling Game — Teaching Arc

The intended commit sequence all three language implementations follow. Language idioms diverge at the line level, but the arc — what lands at what gear, where the reflections sit, and the two explicit "I refused to extract `Frame`" moments — is shared.

Legend: **R** red, **G** green, **F** refactor, **Ref** reflect (empty commit), **S** spec-on-arrival (test passes immediately because a prior triangulation generalized).

| # | Phase | Gear | What |
|---|-------|------|------|
| 1 | scaffold | — | Empty solution/project, no tests. |
| 2 | R | low | Gutter game scores zero. |
| 3 | G (fake-it) | low | `return 0`. |
| 4 | R | low | All ones scores twenty. |
| 5 | G | low | Sum the rolls. |
| 6 | Ref | low | Empty commit. Tempted to extract `Frame` — the test list hasn't asked for one. Holding. |
| 7 | R | low → middle | One spare scores 16. Sum no longer cuts it — scoring needs to peek at the next roll. |
| 8 | G | middle | Walk the roll list in frames; on a spare (two rolls summing to 10), add the next roll as bonus and advance by 2. Otherwise sum the two rolls and advance by 2. |
| 9 | F | middle | Introduce `frameIndex` / `rollIndex` split in the scoring loop. Ten iterations of frame; roll cursor moves 1 or 2. |
| 10 | R | middle | One strike scores 24. |
| 11 | G | middle | Detect strike (single roll of 10), add next two rolls as bonus, advance roll cursor by 1. |
| 12 | Ref | middle | Empty commit. Second temptation to extract `Frame` — the scoring loop now has three branches. Refusing: the `Frame` concept lives in the 1-or-2 roll-cursor advance; a class would replicate state the list already holds. |
| 13 | R | middle | Perfect game scores 300. |
| 14 | S | middle | Perfect game passes on arrival — the ten-frame loop, strike branch, and two-roll lookahead already cover the tenth frame's bonus rolls because they sit at the end of the roll list. |
| 15 | R | middle | All spares scores 150. |
| 16 | S | middle | All spares passes on arrival — spare branch plus the single tenth-frame bonus roll is already correct. The 10th frame's bonus rolls are *data*, not a new code path. |
| 17 | Ref | middle | Empty commit. All six scenarios green. No `Frame` class was written. The design is complete. |

Roughly 17 commits per language. Three `reflect —` commits mark the Frame-refusal moments (6, 12) and the done moment (17). Two `spec —` commits (14, 16) prove the tenth-frame bonus rolls needed no special code path.

## Notes per phase

- **Scaffold** (1) — only the project skeleton. No file named after the SUT yet.
- **Red** commits only include the test. Run, confirm red, commit.
- **Green** commits write the minimum code to pass the test that was just committed red.
- **Refactor** commits keep the bar green; nothing changes behavior. The one here (step 9) is the `frameIndex` / `rollIndex` split that makes the strike branch easy to add.
- **Reflect** commits are `git commit --allow-empty` with a message explaining the gear decision or design pressure — here, specifically, the Frame-class temptation and the reason to resist.
- **Spec (passes on arrival)** commits add a test that is immediately green because the loop's two-step advance already models what a tenth frame needs. The commit body says so.
