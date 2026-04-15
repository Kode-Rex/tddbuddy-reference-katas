# Bingo — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **the bingo card domain only**: the 5x5 grid, the FREE space, marking called numbers, and detecting winning lines (rows, columns, diagonals). Random card generation, the caller/draw service, the game loop, and any UI/persistence concerns are **out of scope** — see the top-level [`README.md`](README.md#scope--pure-domain-only) for the full stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Card** | A 5x5 grid of cells; each cell holds a number (or the free marker at the centre) plus a marked/unmarked flag. Exposes `mark(number)`, `isMarked(row, col)`, `numberAt(row, col)`, `winningPattern()`, `hasWon()`. |
| **Cell** | One grid position. Holds a number in the range for its column (or the free marker at `(2, 2)`) and a marked flag. |
| **Free Space** | The centre cell `(row=2, col=2)`. Holds no number; starts marked and stays marked. |
| **Column Range** | Each column draws numbers from a fixed inclusive range: B `1–15`, I `16–30`, N `31–45`, G `46–60`, O `61–75`. |
| **Winning Pattern** | The completed line: one of `Row(index)`, `Column(index)`, `DiagonalMain`, `DiagonalAnti`, or `None` when no line is complete. |
| **CardBuilder** | Test-folder fluent builder that produces a `Card`. Chained `.withNumberAt(row, col, number)` and `.withFreeAt(row, col)` calls place values at specific coordinates. Reads as a direct literal of the card state under test. The builder does not enforce column-range rules — it is a test-side synthesiser. |

## Domain Rules

- The card is 5x5 (`CardSize = 5`). Rows and columns are zero-indexed; `(0, 0)` is the top-left `B` cell, `(4, 4)` is the bottom-right `O` cell.
- Column ranges are fixed: column `0 (B) → [1, 15]`, `1 (I) → [16, 30]`, `2 (N) → [31, 45]`, `3 (G) → [46, 60]`, `4 (O) → [61, 75]`.
- The free space at `(2, 2)` holds no number; it is marked from construction and cannot be un-marked.
- `Card.mark(number)` accepts any integer in `[1, 75]`. A number outside that range raises `NumberOutOfRangeException` (C#) / `NumberOutOfRangeError` (TS / Python).
- Marking a number **not present on the card** is a silent no-op. The caller announces numbers at large; not every call lands on every card.
- Marking a number **that is present** flips every cell holding that number (numbers are unique per card, so in practice at most one cell).
- A win is the first completed line of five marked cells: any of five rows, five columns, the main diagonal `(0,0)→(4,4)`, or the anti-diagonal `(0,4)→(4,0)`. The free space counts as marked for the two diagonals and for row 2 / column 2.
- `Card.winningPattern()` returns the first match in the scan order: rows top-to-bottom, then columns left-to-right, then main diagonal, then anti-diagonal. When nothing is complete it returns the `None` variant (`WinPattern.None` / `null` / `None`).
- `Card.hasWon()` is `winningPattern() != None`.

### Exception Messages

The exception message strings are **identical byte-for-byte** across all three languages; the exception type names differ by language idiom.

| Rule | Message |
|------|---------|
| called number outside 1–75 | `"called number must be between 1 and 75"` |

## Test Scenarios

1. **Blank card reports no win and no marks** — a fresh `Card` built with numbers only has `hasWon() == false`, `winningPattern() == None`, and `isMarked(r, c) == false` for every cell except the free space.
2. **Free space starts marked** — on a fresh `Card`, `isMarked(2, 2) == true`.
3. **Marking a number that is on the card marks the matching cell** — given a card with `7` at `(0, 0)`, `mark(7)` leaves `isMarked(0, 0) == true` and every other non-free cell still unmarked.
4. **Marking a number not on the card is a silent no-op** — given a card without the number `42`, `mark(42)` leaves every non-free cell unmarked and no exception is raised.
5. **Marking a number outside 1–75 raises** — `mark(0)` and `mark(76)` both raise with message `"called number must be between 1 and 75"`.
6. **Completing row 0 wins on that row** — marking every number in row 0 yields `winningPattern() == Row(0)` and `hasWon() == true`.
7. **Completing column 4 wins on that column** — marking every number in column 4 yields `winningPattern() == Column(4)`.
8. **Completing the main diagonal wins on `DiagonalMain`** — marking `(0,0)`, `(1,1)`, `(3,3)`, `(4,4)` (the free space at `(2,2)` already counts) yields `winningPattern() == DiagonalMain`.
9. **Completing the anti-diagonal wins on `DiagonalAnti`** — marking `(0,4)`, `(1,3)`, `(3,1)`, `(4,0)` yields `winningPattern() == DiagonalAnti`.
10. **Four marks in a row is not a win** — marking four of the five numbers in row 0 leaves `hasWon() == false` and `winningPattern() == None`.
11. **Winning-pattern scan order is rows, then columns, then diagonals** — a card that simultaneously completes row 0 *and* column 0 reports `Row(0)` first (rows are scanned before columns).
12. **CardBuilder produces the card the test literal describes** — `CardBuilder().withNumberAt(0, 0, 3).build()` has `numberAt(0, 0) == 3`, the free space at `(2, 2)` marked, and no other marks. Marking `3` then marks `(0, 0)`.
