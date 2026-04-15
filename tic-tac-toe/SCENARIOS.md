# Tic-Tac-Toe — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **the tic-tac-toe board domain only**: placing marks, tracking turns, detecting wins and draws, rejecting invalid moves. UI, CLI, persistence, and AI opponents are **out of scope** — see the top-level [`README.md`](README.md#scope--pure-domain-only) for the full stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Board** | An immutable 3x3 grid of cells. Exposes `place(row, col)` (returns a new board), `outcome()`, `currentTurn()`, and `cellAt(row, col)`. |
| **Cell** | A single grid position: `Empty`, `X`, or `O`. |
| **Mark** | The two non-empty cell values: `X` and `O`. `X` always moves first. |
| **Outcome** | The game state: `InProgress`, `XWins`, `OWins`, or `Draw`. Computed from the board, not tracked separately. |
| **Winning line** | One of the eight three-cell lines (three rows, three columns, two diagonals) that, when filled with the same mark, completes a win. |
| **BoardBuilder** | Test-folder fluent builder that produces a `Board`. Chained `.withXAt(r,c)` and `.withOAt(r,c)` calls place marks at specific coordinates. Reads as a direct literal of the board state under test. |

## Domain Rules

- The board is 3x3 (`BoardSize = 3`). Coordinates are zero-indexed: `(0,0)` is top-left, `(2,2)` is bottom-right.
- `X` moves first; turns alternate strictly (`X, O, X, O, ...`).
- A placement on an occupied cell raises `CellOccupiedException` (C#) / `CellOccupiedError` (TS) / `CellOccupiedError` (Python).
- A placement with a row or column outside `[0, 2]` raises `OutOfBoundsException` / `OutOfBoundsError`.
- A placement on a board whose outcome is anything other than `InProgress` raises `GameOverException` / `GameOverError`.
- A win is any of the eight winning lines filled with three of the same mark: three rows, three columns, the main diagonal, the anti-diagonal.
- A draw is a full board with no winning line. A full board with a winning line is a win, not a draw.
- The message strings for each exception are **identical byte-for-byte** across all three languages; the exception type names differ by language idiom.

### Exception Messages

| Rule | Message |
|------|---------|
| occupied cell | `"cell already occupied"` |
| out-of-bounds coords | `"coordinates out of bounds"` |
| placement after game over | `"game is already over"` |

## Test Scenarios

1. **Empty board reports game in progress** — a fresh `Board` has `outcome() == InProgress` and `currentTurn() == X`.
2. **First placement puts X on the board** — after `place(0,0)` on an empty board, `cellAt(0,0) == X`, `currentTurn() == O`, outcome still `InProgress`.
3. **X wins by completing the top row** — given `X` at `(0,0)` and `(0,1)` and `O` at `(1,0)` and `(1,1)`, placing `X` at `(0,2)` yields `outcome() == XWins`.
4. **X wins by completing the left column** — given `X` at `(0,0)` and `(1,0)` and `O` at `(0,1)` and `(1,1)`, placing `X` at `(2,0)` yields `outcome() == XWins`.
5. **X wins on the main diagonal** — given `X` at `(0,0)` and `(1,1)` and `O` at `(0,1)` and `(0,2)`, placing `X` at `(2,2)` yields `outcome() == XWins`.
6. **O wins on the anti-diagonal** — given `X` at `(0,0)`, `(1,0)`, `(2,1)` and `O` at `(0,2)` and `(1,1)`, placing `O` at `(2,0)` yields `outcome() == OWins`.
7. **Full board with no winning line is a draw** — the board `X O X / X X O / O X O` has `outcome() == Draw`.
8. **Placing on an occupied cell raises cell-occupied** — a board with `X` at `(0,0)` rejects `place(0,0)` with message `"cell already occupied"`.
9. **Placing with a row out of bounds raises out-of-bounds** — `place(3, 0)` on an empty board raises with message `"coordinates out of bounds"`; same for negative coordinates.
10. **Placing with a column out of bounds raises out-of-bounds** — `place(0, 3)` on an empty board raises with message `"coordinates out of bounds"`.
11. **Placing after a win raises game-over** — after `X` has won the top row, any further `place(...)` raises with message `"game is already over"`.
12. **BoardBuilder produces the board the test literal describes** — `BoardBuilder().withXAt(0,0).withOAt(1,1).build()` has `X` at `(0,0)`, `O` at `(1,1)`, every other cell empty, `outcome() == InProgress`. When `X` marks outnumber `O` marks by one, it is `O`'s turn; when equal, it is `X`'s turn.
