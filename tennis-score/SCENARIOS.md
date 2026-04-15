# Tennis Score — Scenarios

The shared specification all three language implementations satisfy. Eight scenarios, ordered by the gear they land at. Scenarios 1–6 drive the point-level state machine within a game; scenarios 7–8 lift to set and match at wrapper level.

## Domain Rules

- A **match** is played between Player 1 and Player 2. Points are recorded one at a time with `pointWonBy(player)`.
- **Point scoring within a game:** `0 → Love`, `1 → 15`, `2 → 30`, `3 → 40`. A point from `Forty` wins the game — **unless the opponent is also at `Forty`**, in which case the score is `Deuce`. From `Deuce`, one point gives the leader `Advantage`. From `Advantage`, the leader wins the game on the next point; the trailer equalising returns to `Deuce`.
- **Games within a set:** first player to six games wins the set, provided they lead by two. (The spec's scenario 7 lands at 6-4, so no deuce-in-games / tiebreak is in scope.)
- **Sets within a match:** best of three. First player to win two sets wins the match.
- The SUT is stateful: `Match` accumulates points via `pointWonBy(player)` and reports `score()`. No validation beyond what the eight scenarios specify (which is none).

## Scenarios

1. **Start of match reads "Love-Love".** No points played → `"Love-Love"`.
2. **One point to Player 1 reads "15-Love".** `pointWonBy(1)` once → `"15-Love"`.
3. **Two points each reads "30-30".** `pointWonBy(1)`, `pointWonBy(2)`, `pointWonBy(1)`, `pointWonBy(2)` → `"30-30"`.
4. **Three points each reads "Deuce".** Six points, three each → `"Deuce"`.
5. **Advantage after Deuce.** From Deuce, `pointWonBy(1)` → `"Advantage Player 1"`.
6. **Game win from Forty.** Player 1 at 4 points, Player 2 at 2 → `"Game Player 1"`.
7. **Set win.** Player 1 wins six games to Player 2's four, no set previously won → `"Set Player 1"`.
8. **Match win.** Player 1 wins one set 6-4 and a second set 6-3 → `"Match Player 1"`.

## Out of Scope

Behavior not listed above is undefined by this spec. No tiebreak. No no-advantage scoring. No invalid-input handling. No Player 2 symmetry tests beyond what scenarios 3 and 4 cover implicitly. If a future requirement names any of those, a scenario is added here first.
