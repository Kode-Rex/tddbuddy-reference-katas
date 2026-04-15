# Tennis Score — C# Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the gear settles at middle once the raw point counts lift into a named `ScoreState`.

The signature move of Tennis Score is **the state machine that extracts itself**. Through Love, 15, 30, 40, Deuce the if/else ladder bends but doesn't break. Advantage is the cliff — conditions start comparing counts against each other, not against fixed values. The refactor lifts the count into a seven-state enum (`Love | Fifteen | Thirty | Forty | Deuce | Advantage | Game`) and the formatter collapses to a dispatch. The very next scenario — game win at 4-2 — passes on arrival.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`8aa8053`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8aa8053) | scaffold | — | Empty xUnit solution. No SUT yet — the first failing test will name it. |
| 2 | [`11afd9b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/11afd9b) | red | low | `new Match().Score().Should().Be("Love-Love")`. Compilation fails. |
| 3 | [`2595660`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2595660) | green (fake-it) | low | `return "Love-Love"`. Restraint. |
| 4 | [`a1d8bde`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a1d8bde) | red | low | 1-0 reads `"15-Love"`. Match needs `PointWonBy`. |
| 5 | [`9519d9b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9519d9b) | green | low | One integer, one `if`. Triangulating — no design yet. |
| 6 | [`21df558`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/21df558) | red | low | 2-2 reads `"30-30"`. Symmetry — one integer won't cut it. |
| 7 | [`ac1c9e7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ac1c9e7) | green | low | Two integers, two duplicate `if` ladders for per-player word. Honest duplication. |
| 8 | [`e69d834`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e69d834) | refactor | low | Extract `ScoreWord` — a `switch` expression over the int. The two ladders collapse into one call per player. A tidy, not yet a state machine. |
| 9 | [`167e18d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/167e18d) | red | low → middle | 3-3 reads `"Deuce"`. First non-`{word}-{word}` output. |
| 10 | [`74a7f97`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/74a7f97) | green | middle | One extra `if` at the top of `Score()`. Still reading raw point counts. |
| 11 | [`df61927`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/df61927) | reflect | middle | **The if/else is bending.** Deuce fit with one more branch on raw counts. Advantage is next — a player at 4+ leading by exactly 1 — that's comparing counts against each other, not against fixed values. Noting the design pressure; staying in the current shape one more scenario to make the refactor unavoidable. |
| 12 | [`fcdb504`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/fcdb504) | red | middle | 4-3 reads `"Advantage Player 1"`. |
| 13 | [`02e0767`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/02e0767) | green | middle | Two new conditions at the top of `Score()` — `p1 >= 4 && p1 - p2 == 1` and its mirror. Generalized Deuce to "both at 3+ and equal" while here. The formatter now reads like arithmetic, not like tennis. |
| 14 | [`8a546df`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8a546df) | refactor | middle | **Extract `ScoreState` enum.** Lift raw point counts into a named state: `Love / Fifteen / Thirty / Forty / Deuce / Advantage / Game`. `Score()` becomes a dispatch on the state pair returned by `ScoreStates()`. The conditions that used to compare integers against each other now compare enum values against named constants — the formatter reads like tennis again. **This is the kata's signature move.** |
| 15 | [`4f6c6c1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/4f6c6c1) | spec (passes on arrival) | middle | 4-2 reads `"Game Player 1"`. **No new code.** The `Forty + point → Game` transition is already encoded in `ScoreStates`: p1 at 4+ with p2 below 3 lifts to `Game`. The scenario that would have added another integer comparison in the old shape needs nothing here. |
| 16 | [`6507592`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6507592) | reflect | middle | Point-level scoring is complete. Six scenarios through Game covered by one dispatch on a seven-state enum. Sets and match are next — they are not a second state machine, they're tallies with a win condition. No new enum coming. |
| 17 | [`f09dc71`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f09dc71) | red+green | middle | 6-4 in games reads `"Set Player 1"`. Wrapper-level. Game-ending points advance a game counter; six games with a two-game lead wins the set. The `ScoreState` enum stays at point-level — games and sets are integer tallies with a win check. A `_gameJustWonBy` flag reports the winning moment before the next point rolls the counter forward. |
| 18 | [`2c70794`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2c70794) | red+green | middle | 6-4, 6-3 in sets reads `"Match Player 1"`. Second wrapper layer. Same pattern as sets-over-games. Eight scenarios green. The state machine still only knows points. |

## How to run

```bash
cd tennis-score/csharp
dotnet test
```

## The takeaway

Two `reflect —` commits mark the cliff (step 11) and the design-complete moment (step 16). One `spec —` commit (step 15) proves the state machine absorbed Game with no new code.

**The state machine is the teaching.** Through the first six scenarios the if/else chain looks reasonable — a few special cases on raw counts. The Advantage scenario makes the cliff obvious: conditions stop comparing counts against fixed values and start comparing counts against each other. Lift the count into a named state and the formatter becomes a dispatch. The Game scenario that follows needs no code — the `Forty + point → Game` transition is already there. That's the payoff.

**Sets and match are deliberately not part of the state machine.** They're integer tallies with win conditions. The walkthrough flags this explicitly at step 16 — the temptation would be to extend `ScoreState` into `SetState` and `MatchState` enums. Resist. A game is seven named states with transitions; a set is "first to six by two"; a match is "first to two sets". Different domains, different shapes.

C# idioms used: `public enum` for the seven states (a discriminated union would be overkill for seven cases with no payload); C# 8 `switch` expressions for `PointsToScore` and `Word`; tuple return on `ScoreStates()` consumed by deconstruction; `int? _gameJustWonBy` to mark the reporting moment without polluting the state machine.
