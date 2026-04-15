# Tennis Score — Teaching Arc

The intended commit sequence all three language implementations follow. Language idioms diverge at the line level, but the arc — what lands at what gear, where the reflection sits, and the signature state-machine refactor — is shared.

Legend: **R** red, **G** green, **F** refactor, **Ref** reflect (empty commit), **S** spec-on-arrival (test passes immediately because a prior triangulation generalized).

| # | Phase | Gear | What |
|---|-------|------|------|
| 1 | scaffold | — | Empty solution/project, no tests. |
| 2 | R | low | Start of match reads `"Love-Love"`. |
| 3 | G (fake-it) | low | `return "Love-Love"`. |
| 4 | R | low | `1-0` reads `"15-Love"`. |
| 5 | G | low | Minimal `if` branch for P1's point count. |
| 6 | R | low | `2-2` reads `"30-30"`. |
| 7 | G | low | Extend: lookup table `[Love, 15, 30, 40]` per player; format `"P1-P2"`. |
| 8 | F | low | Collapse two lookups into a single `scoreWord(points)` helper. |
| 9 | R | low → middle | `3-3` reads `"Deuce"`. |
| 10 | G | middle | Special-case both-at-three in the formatter. |
| 11 | Ref | middle | Empty commit. The formatter is sprawling — every new condition branches on raw point counts. Next scenario (Advantage) will compare counts against each other, not against fixed values. That's the cliff. |
| 12 | R | middle | `4-3` reads `"Advantage Player 1"`. |
| 13 | G | middle | Special-case Advantage (one player at 4+, lead by exactly 1). |
| 14 | F | middle | **Extract `Score` state** — `Love / Fifteen / Thirty / Forty / Deuce / Advantage / Game`. Formatter becomes a clean dispatch on the state pair. |
| 15 | R | middle | `4-2` reads `"Game Player 1"`. |
| 16 | S | middle | Game-win passes on arrival — the state machine's `Forty + point → Game` transition already covers it. |
| 17 | Ref | middle | Empty commit. State machine is in place. Point-level scoring is complete. Games and sets are not a second state machine — they're tallies. |
| 18 | R+G | middle | Set win: `6-4` in games reads `"Set Player 1"`. Wrapper-level addition — track games-within-set. |
| 19 | R+G | middle | Match win: `6-4, 6-3` in sets reads `"Match Player 1"`. Second wrapper layer — track sets-within-match. |

Roughly 19 commits per language. Two `reflect —` commits mark the pre-refactor cliff (11) and the design-complete moment (17). One `spec —` commit (16) proves the game-win scenario needed no new code once the state machine existed.

## Notes per phase

- **Scaffold** (1) — only the project skeleton. No file named after the SUT yet.
- **Red** commits only include the test. Run, confirm red, commit.
- **Green** commits write the minimum code to pass the test that was just committed red.
- **Refactor** commits keep the bar green; nothing changes behavior. The one at step 8 (extract `scoreWord`) is a tidy. The one at step 14 (extract `Score` state) is the signature move.
- **Reflect** commits are `git commit --allow-empty` with a message explaining the gear decision or design pressure.
- **Spec (passes on arrival)** commits add a test that is immediately green because the state machine already models the transition.
- **Set and match wrappers** (18, 19) are combined red+green commits — middle gear. Each is a count and a win check; no new state machinery is introduced.

## Per-language divergences acknowledged

- **C#** — `Score` is a `public enum` (a discriminated union would be overkill for seven cases with no payload). The formatter is a `switch` expression.
- **TypeScript** — `Score` is a string-literal union (`'Love' | 'Fifteen' | ... | 'Game'`). The formatter is a switch over the pair.
- **Python** — `Score` is a `StrEnum` (Python 3.11+). The formatter is an `if/elif` ladder keyed by the enum pair.

Each is idiomatic. All three share the arc: the refactor at step 14 lifts raw `int` point counts into a named state, and the formatter that follows is a dispatch, not a comparison.
