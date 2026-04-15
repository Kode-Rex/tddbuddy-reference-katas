# 100 Doors — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- Input: `numDoors` — a non-negative integer count of doors, all initially closed.
- Procedure: for each `pass` from `1` through `numDoors`, toggle every `pass`-th door (doors are 1-indexed: `pass`, `2 * pass`, `3 * pass`, ...).
- Output: the 1-indexed door numbers that are **open** at the end, in ascending order.

**Teaching note — perfect-square insight.** A door `i` is toggled once for each of its divisors, and only perfect squares have an odd number of divisors (divisors pair up except for `sqrt(i)`, which pairs with itself). So the open doors are exactly the perfect squares `1, 4, 9, 16, ...` up to `numDoors`. The reference implementation uses simulation — matching the problem's narrative — but the closed form is `floor(sqrt(i))` open doors.

## Test Scenarios

1. **Zero doors** — empty list (no doors to toggle).
2. **One door** — `[1]` (pass 1 opens the single door; there is no pass 2+).
3. **Ten doors** — `[1, 4, 9]` (the perfect squares ≤ 10).
4. **One hundred doors** — `[1, 4, 9, 16, 25, 36, 49, 64, 81, 100]` (the perfect squares ≤ 100).
