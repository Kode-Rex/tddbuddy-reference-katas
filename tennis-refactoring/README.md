# Tennis Refactoring

A legacy tennis scoring function with the correct output but the wrong shape: one function, nested conditionals, magic score numbers, and a formatter that computes `p1Score - p2Score` to decide whether it is looking at *Advantage* or *Win*. The work is two moves, in order: **capture the legacy behavior with characterization tests, then refactor the implementation into code you would be happy to ship** — all without breaking a single test.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. The F2 tier's classification is explicit: *characterization test set only* — no test-folder builder ships here. A scorer takes two integers and a pair of names and returns a string; setting up "a tennis game" is already one line. The F2 discipline that earns its keep is the **shared characterization contract**: every language commits to the same output bytes for the same inputs.

## The Refactoring Move

A refactoring kata is not *"write tests for what exists and stop"* — it is *"pin the behavior of the legacy, then rewrite the implementation until those tests still pass against code you would be happy to hand to a teammate."* In this reference:

- [`SCENARIOS.md`](SCENARIOS.md) is the characterization set. Every entry captures a concrete `(p1Score, p2Score)` pair the legacy code handles and commits all three language ports to the legacy's exact output string — byte-for-byte.
- Each language ships a **clean** scorer that satisfies the same contract. The refactor lifts the point count into a named `Score` state (`Love | Fifteen | Thirty | Forty`) and splits the formatter into three named branches — equal scores, endgame (advantage/win), and normal play. The legacy's `diff == 1` / `diff >= 2` arithmetic is gone; the `scores = ["Love", "Fifteen", "Thirty", "Forty"]` magic array is gone; the implicit "4 or higher means endgame" threshold is named.
- Tests are **truly characterization** — they pass against both the legacy function (paste the original into the source file and the suite is still green) and the refactored implementation. That is the contract the F2 refactoring tier codifies.

This is the pattern every refactoring kata in the set follows. See also [`calc-refactor/`](../calc-refactor/) — the other F2 refactoring kata, which carries a `CalculatorBuilder` because its entity accepts a key sequence. Tennis scoring does not; hence no builder here.

## Scope — Single-Game Scoring Only

The original brief describes a game of tennis — Love through Deuce through Advantage through Win. Sets, matches, and tiebreaks are mentioned as *bonus* work. **Only single-game scoring is in scope for this reference.** The characterization set covers every output the legacy function produces from integer point counts; anything beyond that (a stateful `TennisGame` class, tiebreak rules, set/match tracking) is a deliberate stretch.

### What Is In Scope

- Equal scores at every point count: `0-0` through `Deuce` (any `>= 3` both sides).
- Unequal in-play scores: every valid `(p1, p2)` with both `< 4`, producing `X-Y` strings.
- Endgame: Advantage for the leader when the gap is exactly one and at least one player has `>= 4`; Win when the gap is two or more.
- The player-name parameters are passed through verbatim — the legacy uses `"Player1"` / `"Player2"` as the brief's convention, and the scenarios commit to those exact strings.

### What Is Not Modeled

- No stateful `TennisGame` class, no `wonPoint(name)` method. The scorer is a pure function of `(p1Score, p2Score, p1Name, p2Name)` — the legacy's shape, deliberately preserved.
- No tiebreak scoring. No set or match tracking.
- No validation of nonsensical inputs (negative scores, `5-0` that should have ended at `4-0`). The legacy does not validate; the characterization does not either.

See [`SCENARIOS.md`](SCENARIOS.md) for the complete characterization set.

## Relationship to `tennis-score/`

[`tennis-score/`](../tennis-score/) is the **Pedagogy mode** tennis kata — it *shows how a clean tennis scorer emerges from TDD*, low-gear triangulation through the if/else cliff at Advantage, then the state-machine refactor that collapses the formatter. Its eight scenarios are the curriculum.

`tennis-refactoring/` is the mirror image — the legacy is already written, *and you have to get from that code to a clean scorer without breaking its behavior*. Same domain, different pedagogical question. The scenario sets overlap deliberately: the equal-scores, in-play, and advantage/win outputs are byte-identical across the two katas, because tennis is tennis.

**Pick one based on what you are practicing:**

- Learning TDD rhythm and when to extract a state machine → [`tennis-score/`](../tennis-score/).
- Practicing characterization-test-then-refactor on legacy code → `tennis-refactoring/` (this kata).

## Why This Is F2 — Characterization Test Set Only

- **One primary function** — `getScore(p1Score, p2Score, p1Name, p2Name)`. No collaborators, no injected clocks, no persistence.
- **No builder.** Unlike `calc-refactor/` (where a key sequence of 4–10 presses per scenario earns a `CalculatorBuilder`), tennis scoring already takes four arguments. A builder would cost more lines than the scenario bodies. The F2 discipline this kata carries is the *characterization contract* itself, not a builder.
- **Rich return type is a string.** `"Love-All"`, `"Advantage Player1"`, `"Win for Player2"` — the scorer's output string *is* the spec. Tests assert on the exact string.

## How to Read This Kata

1. Read this README for scope and the refactoring-move framing.
2. Read [`SCENARIOS.md`](SCENARIOS.md) for the characterization set — the contract between legacy intent and clean implementation.
3. Pick a language and read its `WALKTHROUGH.md` for design rationale.
4. Run the tests.

Reference walkthroughs: [`csharp/`](csharp/WALKTHROUGH.md), [`typescript/`](typescript/WALKTHROUGH.md), [`python/`](python/WALKTHROUGH.md).
