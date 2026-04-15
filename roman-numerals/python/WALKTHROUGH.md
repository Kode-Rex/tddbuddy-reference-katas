# Roman Numerals — Python Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom: red → green for the opening triangulation, a reflect where the first design idea runs out of road, one big `green —` that promotes the table, and a long chain of `spec —` commits as the remaining scenarios pass on arrival.

The signature move of Roman Numerals is **the table that beats special cases**. Through `1 → "I"`, `2 → "II"`, `3 → "III"` a simple `dict[int, str]` keyed by the input looks fine. `4 → "IV"` ruins it — adding a fourth key explains nothing about `IX`, `XL`, or `MCMLXXXIV`. The refactor promotes the lookup from `{input: output}` to a `list[tuple[int, str]]` of `(value, symbol)` pairs with subtractives baked in as first-class entries, and a greedy subtract loop does the rest. Five `spec —` commits at the end prove the abstraction was right.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`97fbdfd`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/97fbdfd) | scaffold | — | Empty pytest project. No SUT yet — the first failing test will name it. |
| 2 | [`584a7fc`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/584a7fc) | red | low | `assert to_roman(1) == "I"`. Import fails — `roman_numerals.roman` does not exist. |
| 3 | [`34eb90a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/34eb90a) | green (fake-it) | low | `return "I"`. Restraint. |
| 4 | [`0bbe061`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0bbe061) | red | low | `2 → "II"`. |
| 5 | [`fd5770b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/fd5770b) | green | low | `if n == 2: return "II"`. One hardcoded branch. |
| 6 | [`ffe5df2`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ffe5df2) | red | low | `3 → "III"`. |
| 7 | [`587590a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/587590a) | green | low | Extend into a `dict[int, str]` keyed by input: `{1:"I", 2:"II", 3:"III"}`. Tidier than a pile of ifs — but the key *is* the input. |
| 8 | [`31bbca8`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/31bbca8) | reflect | low | **The lookup-by-input doesn't generalize.** Three entries, three tests. Every new input would need its own key; `4` is next, and concatenating `"I"` `n` times is the tempting wrong turn. Noting the design pressure before the gear shift. |
| 9 | [`9cabce4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9cabce4) | red | low | `5 → "V"`. |
| 10 | [`8ebf224`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8ebf224) | green | low | Add `5: "V"` to the dict. Same input-keyed shape — the design hasn't changed yet. |
| 11 | [`53fa0be`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/53fa0be) | red | low → middle | `4 → "IV"`. Here's the cliff. |
| 12 | [`a30d7e5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a30d7e5) | green | middle | **Promote the table.** Replace the dict with a `list[tuple[int, str]]` — `[(5, "V"), (4, "IV"), (1, "I")]` — and a greedy subtract loop: `while n >= value: parts.append(symbol); n -= value`. Subtractives are entries in the table, not special cases. **This is the kata's signature move.** The `2 → "II"` and `3 → "III"` scenarios, which previously had explicit entries, still pass — the loop emits them from `(1, "I")` alone. |
| 13 | [`30f7107`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/30f7107) | red | middle | `10 → "X"`. |
| 14 | [`94c4b31`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/94c4b31) | green | middle | Extend the table with `(10, "X")` and `(9, "IX")`. The greedy loop needs no changes. |
| 15 | [`0b46c76`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0b46c76) | spec (passes on arrival) | middle | `9 → "IX"`. **No code change.** First spec-on-arrival. |
| 16 | [`bd68665`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bd68665) | red | middle | `40 → "XL"`. |
| 17 | [`ba1e7f6`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ba1e7f6) | green | middle | Extend the table with `(90, "XC")`, `(50, "L")`, `(40, "XL")`. Three new entries. |
| 18 | [`73efb6f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/73efb6f) | spec | middle | `90 → "XC"`. No code change. Second spec-on-arrival. |
| 19 | [`3178370`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3178370) | red | middle | `400 → "CD"`. |
| 20 | [`c603ced`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c603ced) | green | middle | Extend the table with `(900, "CM")`, `(500, "D")`, `(400, "CD")`, `(100, "C")`. The pattern repeats. |
| 21 | [`ba1d870`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ba1d870) | spec | middle | `900 → "CM"`. No code change. Third spec-on-arrival. |
| 22 | [`d854414`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d854414) | red | middle | `1000 → "M"`. |
| 23 | [`4f65705`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/4f65705) | green | middle | Extend the table with `(1000, "M")`. Full thirteen-entry mapping now present. |
| 24 | [`e8e0b0c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e8e0b0c) | reflect | middle | **Table is complete.** Remaining scenarios should be pure spec. |
| 25 | [`ac56235`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ac56235) | spec | middle → high | `1984 → "MCMLXXXIV"`. Fourth spec-on-arrival. |
| 26 | [`ff1ef8e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ff1ef8e) | spec | high | `3999 → "MMMCMXCIX"`. Fifth and final. Five `spec —` commits in a row. |

## How to run

```bash
cd roman-numerals/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

## The takeaway

Two `reflect —` commits (steps 8 and 24) and **five `spec —` commits** (steps 15, 18, 21, 25, 26) are the signature of this kata. The table abstraction is data-driven: the greedy loop knows nothing about Roman numerals — it only knows about a descending list of `(value, symbol)` pairs. Subtractives are not exceptions to the rule; they are entries in the table.

Python idioms used: module-level `list[tuple[int, str]]` for the mapping (PEP 585 generic types, no need for `typing.List`); a `list` accumulator with `"".join(...)` at the end (idiomatic over `+=` in Python — strings are immutable, so repeated concatenation in a hot loop would be O(n²)); plain `def` function, no class (pure function); underscore-prefixed `_MAPPING` to signal module-private.
