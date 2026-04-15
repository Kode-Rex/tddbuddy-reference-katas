# Tennis Refactoring — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the characterization set was understood. This walkthrough explains **why the refactored design came out the shape it did**, not how the commits unfolded.

This is an **F2 refactoring kata** classified *characterization test set only* — no test-folder builder ships (contrast with [`../../calc-refactor/csharp/`](../../calc-refactor/csharp/WALKTHROUGH.md), where a key-sequence builder does earn its keep). The F2 discipline here is the **shared characterization contract** in [`../SCENARIOS.md`](../SCENARIOS.md): sixteen `(p1Score, p2Score, p1Name, p2Name) -> string` pairs that pin the legacy's exact output byte-for-byte, across C#, TypeScript, and Python.

## The Refactoring Move

The kata brief hands you a ten-line legacy function with the correct output and the wrong shape: three nested branches over `(p1Score == p2Score)`, `(p1Score >= 4 || p2Score >= 4)`, and the else, plus an inline `["Love","Fifteen","Thirty","Forty"]` array and a formatter that asks `if (diff == 1) "Advantage " + p1Name` — mixing *which branch the game is in* with *who is winning*. The refactor is not "unit-test the legacy" — it is *"decide what the legacy promises, write that down as characterization tests, then rewrite the implementation whose tests pin the contract."*

In this reference the legacy is gone. The clean scorer has three named branches that match the three conceptual states of a tennis game, and the `Score` type lifts the point-name lookup out of the formatter:

```csharp
TennisScorer.GetScore(0, 0, "Player1", "Player2").Should().Be("Love-All");
TennisScorer.GetScore(4, 3, "Player1", "Player2").Should().Be("Advantage Player1");
TennisScorer.GetScore(5, 3, "Player1", "Player2").Should().Be("Win for Player1");
```

## Relationship to `tennis-score/` (Pedagogy)

[`../../tennis-score/`](../../tennis-score/) is the **Pedagogy** tennis kata — the *curriculum* that walks a solver, commit by commit, from Love-Love through Deuce through the state-machine refactor. `tennis-refactoring/` is the mirror image — legacy code in hand, characterize it, rewrite it. Same domain, different pedagogical question. The scenario strings overlap deliberately; tennis is tennis.

## Scope — Single-Game Scoring Only

No `TennisGame` class, no `wonPoint(name)`, no tiebreak, no set or match tracking. The scorer is a pure function of four arguments — the legacy's shape, deliberately preserved. See [`../README.md`](../README.md#scope--single-game-scoring-only) for the full out-of-scope list.

## The Design at a Glance

```
TennisScorer.GetScore(p1Score, p2Score, p1Name, p2Name) -> string
  EqualScore      (when scores match)        → "Love-All" / "Fifteen-All" / "Thirty-All" / "Deuce"
  EndgameScore    (when either is >= 4)      → "Advantage <name>" / "Win for <name>"
  InPlayScore     (otherwise)                → "<Point>-<Point>"

Score.Name(0..3) → "Love" | "Fifteen" | "Thirty" | "Forty"

EndgameThreshold = 4       DeuceThreshold = 3
```

Two files under `src/TennisRefactoring/` — one static scorer, one internal name-lookup. No builder under `tests/` by design.

## Why Three Named Branches

The legacy has three implicit states mashed into one function. Naming them — `EqualScore`, `EndgameScore`, `InPlayScore` — is the refactor. Every test in the characterization set lands in exactly one of those three; every reader of the code can find which branch a scenario hits by name rather than by reading the condition stack. This is the single biggest readability win the refactor earns and it is deliberately small — three methods, seven to nine lines each.

## Why `Score.Name` Is a Static Lookup, Not an Enum

One plausible refactor lifts the point names into an enum (`Love | Fifteen | Thirty | Forty`). For the Pedagogy kata [`../../tennis-score/`](../../tennis-score/) this is exactly the move — there the state machine is the teaching point, and the enum extends to include `Deuce | Advantage | Game` because the *transitions* are what the kata showcases.

This refactoring kata is a different animal. The legacy function is an **integer-count scorer** — it never models the state between points, only the output at a given `(p1, p2)` pair. Lifting the point names into an enum is fine; lifting Deuce/Advantage/Game into the same enum is **adding behavior the legacy does not have**. The characterization contract would not pin that decision, and the refactor would be doing two jobs at once. A flat `Score.Name(int) -> string` is the minimum move that removes the magic array without changing the shape the legacy promises.

If you want the state-machine refactor, read [`../../tennis-score/csharp/WALKTHROUGH.md`](../../tennis-score/csharp/WALKTHROUGH.md). They are different katas for a reason.

## Why Named Thresholds, Not Inline `4` and `3`

F2 is Full-Bake mode. Named constants beat inline magic numbers when the name adds meaning — and `EndgameThreshold = 4` *does* add meaning the legacy's `>= 4` hides. The reader sees `if either score >= endgame threshold` instead of `if either score >= 4` and knows immediately that `4` is the domain concept "endgame starts here." Same for `DeuceThreshold = 3`.

Contrast with the F1 / Pedagogy style convention (`tennis-score/`), where `4` stays inline *because the walkthrough names it by value* and pulling it into a constant would hide the teaching moment. Different mode, opposite rule.

## Why No Builder

Calc Refactor ships `aCalculator().pressKeys("1+2=")` because every scenario is a five-to-twelve-key sequence. Tennis scoring is `getScore(3, 4, "Player1", "Player2")` — four arguments, all of which are the scenario. A `MatchBuilder().WithP1Score(3).WithP2Score(4).WithNames("Player1", "Player2")` would be strictly *longer* than the call site it replaces.

This is the F2 tier's *characterization test set only* classification — a category explicitly marked in [`docs/plans/2026-04-14-remaining-katas.md`](../../docs/plans/2026-04-14-remaining-katas.md). F2 does not require a builder; it requires a characterization contract. This kata carries the latter.

## Scenario Map

The sixteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/TennisRefactoring.Tests/TennisScorerTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd tennis-refactoring/csharp
dotnet test
```

Expected: **16 passed, 0 failed.**
