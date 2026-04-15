# Tennis Score

**Teaching mode:** [Pedagogy](../README.md#1-pedagogy--learn-the-tdd-rhythm) — the fourth Pedagogy kata in the repo.

Score a tennis match. A `Match` records points for either player with `pointWonBy(player)` and renders the current status with `score()`. The teaching moment is the **state-machine extraction** — the first few scenarios tempt the solver into a sprawling if/else over raw point counts, and the `Deuce` and `Advantage` cases make that cliff obvious. The refactor lifts the point score into a `Score` state (`Love | Fifteen | Thirty | Forty | Deuce | Advantage | Game`) and the formatter collapses to a clean dispatch.

## What this kata teaches

The **moment a conditional pile becomes a state machine.** Scoring 0-0, 1-0, 2-2 with nested ifs on `(p1Points, p2Points)` feels fine through the first few triangulations. Deuce arrives and the branches need to start asking "are both players at 3 or more?". Advantage lands and suddenly the conditions are comparing differences between counts instead of the counts themselves. That's the cliff. The refactor lifts the point count into a **named state** — one per tennis term — and the formatter becomes a dispatch on `(p1State, p2State)`.

Walk the commit log and you'll see the if/else chain grow through Love, 15, 30, 40, Deuce. At the Advantage commit the walkthrough flags the design pressure explicitly. The refactor step — introducing the `Score` enum/union — turns the formatter into one line per case.

## Gear arc

- **Low gear** for the opening triangulations — 0-0, 1-0, 2-2. Hardcode, then build a lookup table.
- **Middle gear** at Deuce. One special case still fits; the if/else is bending but not broken.
- **Gear shift into refactor** at Advantage — the conditions are comparing counts against each other now, not against fixed values. Extract the `Score` state.
- **Middle gear** through Game, Set, Match — the state machine is in place; each new scenario is a pure addition to the formatter or a wrapper-level concern (games within a set, sets within a match).
- **No high gear needed.** Eight scenarios; the signature refactor lands at scenario five.

## Why a Score state (and where it stops)

The `Score` union covers **point scoring within a game** — Love through Advantage and Game. That's where the state machine earns its keep: the valid transitions (`Forty + point → Game` or `Forty + point → Advantage` depending on opponent) are exactly what the enum encodes.

**Games within a set** and **sets within a match** are not a second state machine. They're tallies with a win condition (first to six games by two; first to two sets). A `Set` or `Match` class tracks counts; the `Score` union does not extend into them. The walkthrough flags this divergence explicitly — it's the "simpler wrapper-level addition" gear after the signature refactor.

If a future requirement asked "score a tiebreak" or "handle no-advantage scoring," the wrappers would stay integer counts and the point-level state would extend. The cleanness of that extension is the payoff for extracting the enum in the first place.

## Files

- [`SCENARIOS.md`](./SCENARIOS.md) — the shared spec (8 numbered scenarios).
- [`ARC.md`](./ARC.md) — the intended commit arc all three languages follow.
- `csharp/`, `typescript/`, `python/` — three idiomatic implementations with per-language `WALKTHROUGH.md`.

## How to read

1. Skim `SCENARIOS.md`.
2. Pick a language; open its `WALKTHROUGH.md`.
3. Walk the commit table. Stop at the `refactor —` row that introduces `Score` — that's the "if/else just became a state machine" moment.
4. Read the final SUT. The formatter should be a single dispatch; the branches that used to compare `p1 >= 4 && p1 - p2 == 1` should all be gone.
