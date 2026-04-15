# Diamond — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- **Target letter** — an uppercase letter `A`–`Z`. Let `n = target - 'A'` (so `A → 0`, `C → 2`, `E → 4`).
- **Height and width** — both equal `2n + 1`. The diamond is square.
- **Rows** — index `r` runs `0 .. 2n`. The row's letter is `'A' + min(r, 2n - r)`; the top half (`r ≤ n`) walks `A..target`, the bottom half mirrors it.
- **Leading spaces** — each row is left-padded with `n - (rowLetter - 'A')` spaces so all letters line up on the diamond's diagonals.
- **Inner spaces** — the `A` row has a single letter. Every other row has two identical letters separated by `2 * (rowLetter - 'A') - 1` spaces.
- **No trailing spaces** — rows end immediately after the final letter; lines are joined with a single `\n`; there is no trailing newline after the last row.
- **Lowercase input** — accepted and normalized to uppercase before rendering.
- **Invalid input** — anything other than a single `A`–`Z` / `a`–`z` letter raises the language-idiomatic argument exception with the message `letter must be a single A-Z character`.

## Test Scenarios

1. **`A` is a single-letter diamond** — `Print('A')` → `"A"`.
2. **`B` renders three rows with a single inner space** — `Print('B')` → `" A\nB B\n A"`.
3. **`C` renders five rows and three inner spaces on the widest row** — `Print('C')` → `"  A\n B B\nC   C\n B B\n  A"`.
4. **`D` renders seven rows** — widest row is `D     D` (five inner spaces).
5. **`E` renders nine rows** — widest row is `E       E` (seven inner spaces).
6. **`Z` renders a full 51-row diamond** — first row has 25 leading spaces then `A`; widest row has `Z` then 49 spaces then `Z`; last row mirrors the first.
7. **Lowercase input is normalized** — `Print('c')` produces the same output as `Print('C')`.
8. **Top half mirrors bottom half** — for any valid target, row `r` equals row `2n - r`.
9. **No trailing whitespace on any row** — every line ends immediately after its final letter.
10. **Non-letter input is rejected** — `'1'`, `' '`, `'!'` raise the language-idiomatic argument exception with message `letter must be a single A-Z character`.
