# Tennis Refactoring — Scenarios

Shared characterization specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers the **pure scorer** for a single tennis game — `(p1Score, p2Score, p1Name, p2Name) -> string`. The stateful `TennisGame` class, tiebreak rules, set and match tracking are **out of scope**. See the top-level [`README.md`](README.md#scope--single-game-scoring-only) for the full out-of-scope list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Score** | The non-negative integer count of points a player has won in the current game. `0`, `1`, `2`, `3` correspond to Love, Fifteen, Thirty, Forty. `4` and beyond are valid only in the *endgame* (Advantage / Win). |
| **Point name** | The tennis word for a particular score count when the game is in-play (neither endgame nor Deuce). `Love`, `Fifteen`, `Thirty`, `Forty`. Never `"0"`, `"15"`, `"30"`, `"40"` — the numerals are the implementation detail; the words are the spec. |
| **In-play** | Both players have fewer than `4` points and the scores are unequal — the output is `<p1-name>-<p2-name>` composed from point names (e.g. `"Thirty-Fifteen"`). |
| **All** | The formatter suffix for equal in-play scores: `"Love-All"`, `"Fifteen-All"`, `"Thirty-All"`. At `3-3` (and every `n-n` for `n >= 3`) the suffix is replaced by `"Deuce"`. |
| **Deuce** | The equal-scores state once both players have reached Forty. Any `(n, n)` with `n >= 3` renders `"Deuce"`. |
| **Advantage** | The endgame state when one player leads by exactly one *and* at least one player has `>= 4` points. Output: `"Advantage <winner-name>"`. |
| **Win** | The endgame state when one player leads by two or more *and* at least one player has `>= 4` points. Output: `"Win for <winner-name>"`. |

## Domain Rules

- **Equal scores render as All or Deuce.** `(0, 0)` → `"Love-All"`, `(1, 1)` → `"Fifteen-All"`, `(2, 2)` → `"Thirty-All"`. From `(3, 3)` and every equal pair beyond, output is `"Deuce"` — the legacy collapses `(4, 4)` to Deuce too, and the characterization preserves that.
- **In-play unequal scores render as `Point-Point`.** Both scores below `4` and unequal: output is `<pointName(p1)>-<pointName(p2)>`. `(1, 0)` → `"Fifteen-Love"`, `(3, 1)` → `"Forty-Fifteen"`.
- **Endgame starts when either player has `>= 4` points.** With unequal scores in this range:
  - Difference of exactly `+1` → `"Advantage <p1Name>"`.
  - Difference of exactly `-1` → `"Advantage <p2Name>"`.
  - Difference of `>= 2` → `"Win for <p1Name>"`.
  - Difference of `<= -2` → `"Win for <p2Name>"`.
- **Names are passed through verbatim.** `p1Name` and `p2Name` are interpolated into the Advantage and Win strings without transformation. The reference uses `"Player1"` / `"Player2"` in the scenario table; any other strings the caller passes are legitimate.
- **No validation.** The legacy does not reject negative scores or impossible states (`5, 0` — which could not have occurred in a real game, since the game would have ended at `4, 0`). The characterization is silent on these inputs; the implementation is free to produce whatever the legacy produces.

## Test Scenarios

1. **Love-All at the start of a game** — `getScore(0, 0, "Player1", "Player2")` returns `"Love-All"`.
2. **Fifteen-All when both players have one point** — `getScore(1, 1, ...)` returns `"Fifteen-All"`.
3. **Thirty-All when both players have two points** — `getScore(2, 2, ...)` returns `"Thirty-All"`.
4. **Deuce when both players have three points** — `getScore(3, 3, ...)` returns `"Deuce"`.
5. **Deuce persists past Forty-All** — `getScore(4, 4, ...)` returns `"Deuce"` (the legacy collapses every equal endgame score to Deuce).
6. **Fifteen-Love when player 1 leads by one at the start** — `getScore(1, 0, ...)` returns `"Fifteen-Love"`.
7. **Love-Fifteen when player 2 leads by one at the start** — `getScore(0, 1, ...)` returns `"Love-Fifteen"`.
8. **Thirty-Fifteen when player 1 has two, player 2 has one** — `getScore(2, 1, ...)` returns `"Thirty-Fifteen"`.
9. **Forty-Fifteen when player 1 has three, player 2 has one** — `getScore(3, 1, ...)` returns `"Forty-Fifteen"`.
10. **Advantage to player 1 when they lead by one in the endgame** — `getScore(4, 3, ...)` returns `"Advantage Player1"`.
11. **Advantage to player 2 when they lead by one in the endgame** — `getScore(3, 4, ...)` returns `"Advantage Player2"`.
12. **Advantage persists at higher equal-gap scores** — `getScore(5, 4, ...)` returns `"Advantage Player1"` (gap of one past the Deuce threshold).
13. **Win for player 1 when they lead by two in the endgame** — `getScore(5, 3, ...)` returns `"Win for Player1"`.
14. **Win for player 2 when they lead by two in the endgame** — `getScore(3, 5, ...)` returns `"Win for Player2"`.
15. **Player names are passed through verbatim into Advantage** — `getScore(4, 3, "Serena", "Venus")` returns `"Advantage Serena"`.
16. **Player names are passed through verbatim into Win** — `getScore(5, 3, "Serena", "Venus")` returns `"Win for Serena"`.
