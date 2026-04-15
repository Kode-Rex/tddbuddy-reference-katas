# Code Breaker — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **feedback scoring only** — secret vs. guess, returning exact and color-only match counts plus a canonical feedback string. Random secret generation and the full game loop are **out of scope** — see the top-level [`README.md`](README.md#scope--feedback-engine-only).

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Peg** | A single playable value drawn from the digits 1–6. Modeled as a typed enum (`Peg.One` … `Peg.Six`) so invalid values cannot be constructed. |
| **Secret** | The 4-peg code the player is trying to break. Immutable; positions are 1-indexed when discussed in prose, 0-indexed in code. |
| **Guess** | A 4-peg code submitted by the player against the secret. Same shape as `Secret`; the distinct type documents *role* at the call site. |
| **Exact match** | A position where the guess peg equals the secret peg. Rendered as `+` in the canonical feedback string. |
| **Color match** | A peg value that appears in both the secret and the guess but at different positions, after exact matches are removed. Rendered as `-`. Each occurrence is consumed once — duplicates are **not** double-counted. |
| **Feedback** | The result of scoring a guess against a secret. Exposes `exactMatches` (int), `colorMatches` (int), `isWon` (true when `exactMatches == 4`), and `render()`/`ToString()` returning the canonical string (all `+` before all `-`, e.g. `"++--"`, `"+"`, `""`). |
| **SecretBuilder** / **GuessBuilder** | Test-folder fluent builders taking four pegs. They exist so test setup reads as a single line naming the code — `new SecretBuilder().With(One, Two, Three, Four).Build()` — instead of a raw array literal. |

## Domain Rules

- **Code length is fixed at 4** for this reference.
- **Peg range is 1–6** — six enum values, no others.
- **Exact matches are counted first**, in a separate pass. Positions consumed by an exact match are *not* eligible for a color-only match on either side.
- **Color matches respect multiplicity**. If the secret has one `One` peg at position 2 and the guess has `One` pegs at positions 1 and 4, the first unmatched `One` in the guess pairs with the (unmatched) `One` in the secret once; the second `One` in the guess finds no partner and contributes zero.
- **The canonical feedback string lists all `+` symbols before all `-` symbols.** `+-` never appears in the other order; `-+` is not a valid rendering.
- **An all-`+` feedback is a win.** `isWon` is derived: `exactMatches == 4`.

## Test Scenarios

1. **Secret 1234 vs guess 5678 has no matches** — feedback renders as `""`; `exactMatches = 0`, `colorMatches = 0`.
2. **Secret 1234 vs guess 1578 has one exact match** — feedback renders as `"+"`; `exactMatches = 1`, `colorMatches = 0`.
3. **Secret 1234 vs guess 1234 is a win** — feedback renders as `"++++"`; `exactMatches = 4`, `isWon` is true.
4. **Secret 1234 vs guess 4321 has four color matches** — feedback renders as `"----"`; `exactMatches = 0`, `colorMatches = 4`.
5. **Secret 1234 vs guess 1243 has two exact and two color matches** — feedback renders as `"++--"`; `exactMatches = 2`, `colorMatches = 2`.
6. **Secret 1234 vs guess 2135 has one exact and two color matches** — feedback renders as `"+--"`; `exactMatches = 1`, `colorMatches = 2`.
7. **Secret 1124 vs guess 5167 counts the duplicate peg only once** — the single `One` in the guess pairs with the exact match at position 2; the secret's other `One` is not available as a color match. Feedback renders as `"+"`; `exactMatches = 1`, `colorMatches = 0`.
8. **Secret 1122 vs guess 2211 has four color matches, no exact** — every peg appears in both codes but at the wrong position. Feedback renders as `"----"`; `exactMatches = 0`, `colorMatches = 4`.
9. **Secret 1111 vs guess 1112 counts three exacts and ignores the non-matching peg** — feedback renders as `"+++"`; `exactMatches = 3`, `colorMatches = 0`.
10. **Secret 1111 vs guess 2111 counts three exacts at positions 2–4** — the `Two` at position 1 does not appear in the secret; feedback renders as `"+++"`; `exactMatches = 3`, `colorMatches = 0`.
11. **Secret 1234 vs guess 1111 counts one exact and no color matches** — the secret has only one `One`; the guess's other three `One` pegs find no partners. Feedback renders as `"+"`; `exactMatches = 1`, `colorMatches = 0`.
12. **A Feedback with four exact matches reports `isWon` true; any other Feedback reports `isWon` false** — including feedback with four color-only matches.
