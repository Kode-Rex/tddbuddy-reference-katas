# Numbers to Words — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- `toWords(n)` returns the English-words spelling of a non-negative integer `n` in the range `0..9999`.
- Compound numbers from **twenty-one through ninety-nine** are **hyphenated**: `21 → "twenty-one"`, `99 → "ninety-nine"`.
- Multiples of ten from twenty to ninety are a single word (no hyphen): `20 → "twenty"`, `90 → "ninety"`.
- Hundreds and thousands are separated by a single space; no "and" is inserted: `303 → "three hundred three"` (not `"three hundred and three"`), `3466 → "three thousand four hundred sixty-six"`.
- A leading "one" is written for hundreds and thousands: `100 → "one hundred"`, `1000 → "one thousand"`.
- Trailing zeros are omitted: `2000 → "two thousand"` (not `"two thousand zero hundred"`), `2400 → "two thousand four hundred"`.
- Inputs outside the range `0..9999` are out of scope; behaviour is undefined. The kata explicitly restricts to four-digit non-negative integers.
- The "fifty-three hundred" shorthand for four-digit numbers mentioned in the kata's bonus is **not** implemented; the reference uses the canonical `"five thousand three hundred"` form.

## Test Scenarios

### 1 digit (0–9)

1. **Zero is spelled out** — `0 → "zero"`
2. **Five is spelled out** — `5 → "five"`
3. **Eight is spelled out** — `8 → "eight"`

### 2 digits (10–99)

4. **Ten is spelled out** — `10 → "ten"`
5. **Nineteen is spelled out** — `19 → "nineteen"` (teens are single words)
6. **Twenty is spelled out** — `20 → "twenty"`
7. **Twenty-one is hyphenated** — `21 → "twenty-one"`
8. **Seventy-seven is hyphenated** — `77 → "seventy-seven"`
9. **Ninety-nine is hyphenated** — `99 → "ninety-nine"`

### 3 digits (100–999)

10. **One hundred names the leading one** — `100 → "one hundred"`
11. **Three hundred three has no "and"** — `303 → "three hundred three"`
12. **Five hundred fifty-five keeps the hyphen in the tens** — `555 → "five hundred fifty-five"`

### 4 digits (1000–9999)

13. **Two thousand omits trailing zeros** — `2000 → "two thousand"`
14. **Two thousand four hundred skips the tens and ones** — `2400 → "two thousand four hundred"`
15. **Three thousand four hundred sixty-six is fully spelled out** — `3466 → "three thousand four hundred sixty-six"`
