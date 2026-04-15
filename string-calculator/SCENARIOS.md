# String Calculator — Scenarios

The shared specification all three language implementations satisfy. The order **is** the curriculum — each scenario is where it is because the previous scenarios made it reachable.

## Domain Rules

- `add(numbers: string) -> int` takes a string and returns the sum of the numbers it contains.
- Default delimiters are comma and newline.
- A custom-delimiter header `//[delimiter]\n` (bracket-wrapped) precedes the numbers.
- Negative numbers are rejected by raising/throwing with message `"negatives not allowed: <n>"`, listing every negative encountered.
- Numbers greater than 1000 are ignored (treated as zero for the sum).
- Delimiters may be any length, and a header may declare multiple delimiters: `//[d1][d2]\n`.

## Scenarios

1. **Empty string returns zero.** `add("") == 0`.
2. **A single number returns itself.** `add("1") == 1`.
3. **Two numbers return their sum.** `add("1,2") == 3`.
4. **An arbitrary count of numbers sums them all.** `add("1,2,3,4") == 10`.
5. **A newline is also a delimiter.** `add("1\n2,3") == 6`.
6. **A custom single-character delimiter can be declared.** `add("//;\n1;2") == 3`.
7. **A negative number is rejected with a listing message.** `add("-1,2")` raises `"negatives not allowed: -1"`. When multiple negatives are present, all are listed: `add("-1,-2,3")` raises `"negatives not allowed: -1, -2"`.
8. **Numbers greater than 1000 are ignored.** `add("2,1001") == 2`.
9. **Delimiters may be any length and/or there may be several.**
   - Any length: `add("//[***]\n1***2***3") == 6`.
   - Multiple single-char: `add("//[*][%]\n1*2%3") == 6`.
   - Multiple multi-char: `add("//[**][%%]\n1**2%%3") == 6`.

## Out of Scope

Behavior not listed above is undefined by this spec — malformed headers, empty tokens between delimiters, numbers with embedded whitespace. No tests are written for those. If a future requirement names them, a scenario is added here first.
