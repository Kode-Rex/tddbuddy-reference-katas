# Bowling Game — C# Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the gear settles at middle once the two-mode index (frame / roll) is in place.

The signature move of Bowling Game is **the class that does not get written**. Two `reflect —` commits mark the moments the author was tempted to extract a `Frame` type and chose not to. The Frame concept is alive in the scoring loop — the roll cursor advances by one for a strike and by two for anything else. That *is* a frame. Naming it with a class would replicate state the roll list already holds.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`6a6424d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6a6424d) | scaffold | — | Empty xUnit solution. No SUT yet — the first failing test will name it. |
| 2 | [`093bac6`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/093bac6) | red | low | `Game.Score(new int[20]).Should().Be(0)`. Compilation fails — `Game` doesn't exist. |
| 3 | [`712fc69`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/712fc69) | green (fake-it) | low | `return 0`. Restraint is the lesson. |
| 4 | [`78ce782`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/78ce782) | red | low | All ones scores twenty. Second data point. |
| 5 | [`9a5d7fe`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9a5d7fe) | green | low | Sum the rolls. Still linear — no bonus logic yet. |
| 6 | [`79eb5f1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/79eb5f1) | reflect | low | **First Frame-class temptation, declined.** The problem names a Frame. The test list has not asked for one — both scenarios are pure rolls-in, score-out. A class whose only client is its author is noise. Holding. |
| 7 | [`64b6dbf`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/64b6dbf) | red | low → middle | One spare scores 16. Sum alone won't cut it — scoring needs to peek at the next roll. |
| 8 | [`d0f0592`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d0f0592) | green | middle | Walk two rolls at a time; on a spare, score `10 + rolls[i+2]`. First lookahead. |
| 9 | [`e22cce9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e22cce9) | refactor | middle | Split `frameIndex` from `rollIndex`. Ten frame iterations; the roll cursor advances by two within each. **The Frame concept now lives in this two-step advance — no class required.** Setting up the strike branch. |
| 10 | [`09d7601`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/09d7601) | red | middle | One strike scores 24. |
| 11 | [`c663218`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c663218) | green | middle | Strike branch: `total += 10 + rolls[i+1] + rolls[i+2]`, advance roll cursor by 1. Three branches in the loop: strike (cursor += 1), spare (cursor += 2), open (cursor += 2). |
| 12 | [`d52938f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d52938f) | reflect | middle | **Second Frame-class temptation, declined again.** The loop has three branches; a reader unfamiliar with bowling might want to pull them into a `Frame` class. But the 1-or-2 cursor advance *is* the frame. A class would replicate state the list already holds, and the tenth-frame bonus rolls (coming next) are just extra entries in the same list. |
| 13 | [`3af3119`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3af3119) | spec (passes on arrival) | middle | Perfect game scores 300. Twelve strikes. The ten-frame loop + strike branch + two-roll lookahead already covered the tenth frame's bonus rolls because they sit at the end of the roll list. **No new code.** This is the payoff for refusing the Frame class — the tenth frame is not a special case. |
| 14 | [`4fca520`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/4fca520) | spec (passes on arrival) | middle | All spares scores 150. Twenty-one rolls of 5. The spare branch's lookahead consumes the 21st entry naturally. **No new code** — the bonus rolls are data, not a code path. |
| 15 | [`003c4c3`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/003c4c3) | reflect | middle | Empty commit. All six scenarios green. The SUT is a static method, a `for` loop, and an integer cursor. Two points of temptation; both declined. |

## How to run

```bash
cd bowling-game/csharp
dotnet test
```

## The takeaway

Three reflect commits — two marking the Frame-class temptation and one marking the design-complete moment. Two spec-on-arrival commits (perfect game, all spares) prove the tenth-frame bonus rolls needed no special code path.

**The class that did not get written is the teaching.** The `Frame` noun lives in the 1-or-2 cursor advance. A reader of the final SUT can point at line `rollIndex += 1;` and say "that's the strike frame boundary" and at `rollIndex += 2;` and say "that's the open/spare frame boundary". The frame concept is clearer as an integer increment than it would be as a class.

C# idioms used: `IReadOnlyList<int>` as the public parameter type, `new int[20]` / `Enumerable.Repeat` for scenario construction, no LINQ in the hot loop (imperative reads cleaner than `.Aggregate` for a branching accumulator).
