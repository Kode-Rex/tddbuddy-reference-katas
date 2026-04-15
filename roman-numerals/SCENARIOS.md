# Roman Numerals — Scenarios

The shared specification all three language implementations satisfy. Nine named scenarios plus a small set of intermediate triangulation cases the solver writes to drive the design. Order matters — the list is the curriculum.

## Domain Rules

- A pure function converts a positive integer to a Roman numeral string.
  - C#: `static class RomanNumerals { static string ToRoman(int n) }`
  - TypeScript: `function toRoman(n: number): string`
  - Python: `def to_roman(n: int) -> str`
- The domain of `n` is `1..3999` inclusive. Behaviour outside that range is undefined by this spec.
- Standard subtractive notation is used: `IV = 4`, `IX = 9`, `XL = 40`, `XC = 90`, `CD = 400`, `CM = 900`. No other subtractive combinations (`IL`, `IC`, `IM`, etc.) appear in the output.
- No validation. The spec does not cover inputs outside `1..3999`, non-integers, or malformed Roman input.

## Scenarios

1. **1 → "I".** The smallest case. Drives the first red-green.
2. **4 → "IV".** The first subtractive; the gear-shift moment. A straight lookup-by-input breaks here.
3. **9 → "IX".** Subtractive at the ones. Passes on arrival once the `(value, symbol)` table contains `IX`.
4. **40 → "XL".** Subtractive at the tens. One new table entry.
5. **90 → "XC".** Subtractive at the tens. Passes on arrival.
6. **400 → "CD".** Subtractive at the hundreds. One new table entry.
7. **900 → "CM".** Subtractive at the hundreds. Passes on arrival.
8. **1984 → "MCMLXXXIV".** Composite across four orders of magnitude; proves the greedy loop composes subtractives with additives.
9. **3999 → "MMMCMXCIX".** Maximum in-range value; three thousands, then subtractives at each lower order.

## Intermediate Triangulation

Tests the solver writes during the arc to pin down behaviour between the named scenarios. These land as `red —`/`green —` or `spec —` commits in the commit log but are not part of the canonical list above — they are the triangulations that make the algorithm emerge.

- **2 → "II".** Triangulates away from "fake-it with a literal".
- **3 → "III".** Suggests "concatenate `I` `n` times" — a tempting wrong turn.
- **5 → "V".** Forces the lookup table to grow past `I`.
- **10 → "X".** Extends the table to the tens before the subtractives at tens are tested.
- **1000 → "M".** One table entry short of the full mapping; lets the composite scenarios pass on arrival.

## Out of Scope

Behaviour not listed above is undefined by this spec. No Roman → Arabic conversion. No input validation (range, type, or format). No zero, no negatives, no values above 3999. If a future requirement names any of those, a scenario is added here first.
