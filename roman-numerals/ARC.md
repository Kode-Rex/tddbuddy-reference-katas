# Roman Numerals — Teaching Arc

The intended commit sequence all three language implementations follow. Language idioms diverge at the line level, but the arc — where the lookup-by-input approach breaks, where the `(value, symbol)` mapping lands, and the long chain of `spec —` commits at the end — is shared.

Legend: **R** red, **G** green, **F** refactor, **Ref** reflect (empty commit), **S** spec-on-arrival (test passes immediately because a prior triangulation generalized).

| # | Phase | Gear | What |
|---|-------|------|------|
| 1 | scaffold | — | Empty solution/project, no tests. |
| 2 | R | low | `1 → "I"`. |
| 3 | G (fake-it) | low | `return "I"`. |
| 4 | R | low | `2 → "II"`. |
| 5 | G | low | Hardcode a branch for `2`. |
| 6 | R | low | `3 → "III"`. |
| 7 | G | low | Extend into a dictionary keyed by the input integer: `{1:"I", 2:"II", 3:"III"}`. |
| 8 | Ref | low | Empty commit. The lookup-by-input doesn't generalize — every input needs an entry. `4` is next; concatenation-of-`I` is a tempting dead end. Noting the pressure before the gear shift. |
| 9 | R | low | `5 → "V"`. |
| 10 | G | low | Add `5:"V"` to the dictionary. Still input-keyed. |
| 11 | R | low → middle | `4 → "IV"`. Input-keyed would demand a fourth entry with no structure; the table has to be rethought. |
| 12 | G | middle | **Promote the table.** Replace the input-keyed dictionary with an ordered list of `(value, symbol)` pairs — `[(5,"V"), (4,"IV"), (1,"I")]` — and implement a greedy subtract loop: while `n >= value`, append `symbol` and subtract. Subtractives are entries in the table, not special cases. |
| 13 | R | middle | `10 → "X"`. |
| 14 | G | middle | Extend the table with `(10,"X")`. |
| 15 | S | middle | `9 → "IX"` — passes on arrival. Add `(9,"IX")` to the table and the greedy loop handles it. (The entry was added while writing the green for step 14 — honest: the table grows at one step and a spec lands at the next.) |
| 16 | R | middle | `40 → "XL"`. |
| 17 | G | middle | Extend the table with `(50,"L"), (40,"XL")`. |
| 18 | S | middle | `90 → "XC"` — passes on arrival once `(90,"XC")` is in the table. |
| 19 | R | middle | `400 → "CD"`. |
| 20 | G | middle | Extend the table with `(100,"C"), (90,"XC"), (500,"D"), (400,"CD")`. |
| 21 | S | middle | `900 → "CM"` — passes on arrival. |
| 22 | R | middle | `1000 → "M"`. |
| 23 | G | middle | Extend the table with `(1000,"M")`. Full mapping now present. |
| 24 | Ref | middle | Empty commit. The table has every needed entry; remaining tests should be `spec —`. |
| 25 | S | middle → high | `1984 → "MCMLXXXIV"` — composite across four orders of magnitude, passes on arrival. |
| 26 | S | high | `3999 → "MMMCMXCIX"` — maximum in-range, passes on arrival. Five `spec —` commits in a row prove the table abstraction was right. |

Roughly 26 commits per language. Two `reflect —` commits mark the "lookup-by-input doesn't generalize" moment (step 8) and the "table is complete" moment (step 24). **Five `spec —` commits** — at 15, 18, 21, 25, 26 — are the signature move. Subtractives are not exceptions to the rule; they are entries in the table.

## Notes per phase

- **Scaffold** (1) — only the project skeleton. No file named after the SUT yet.
- **Red** commits only include the test. Run, confirm red, commit.
- **Green** commits write the minimum code to pass the test that was just committed red.
- **Refactor** commits keep the bar green; nothing changes behavior. This arc has no standalone `refactor —` commit — the signature design move (the table promotion at step 12) is a `green —` that also *is* the refactor. The walkthrough calls it out explicitly.
- **Reflect** commits are `git commit --allow-empty` with a message explaining the gear decision or design pressure.
- **Spec (passes on arrival)** commits add a test that is immediately green because the table and greedy loop already model the transition. The commit body says so.

## Per-language divergences acknowledged

- **C#** — the mapping is a `static readonly` array of `(int Value, string Symbol)` tuples. The greedy loop is a `while` over a `StringBuilder`. `ToRoman` is a `static` method on a `static class`.
- **TypeScript** — the mapping is a `const` array of `readonly [number, string]` tuples typed as `ReadonlyArray<readonly [number, string]>`. String concatenation uses `+=`. `toRoman` is an exported `function`.
- **Python** — the mapping is a module-level list of `tuple[int, str]`. Accumulation uses a list and `"".join(...)`. `to_roman` is a module-level function.

Each is idiomatic. All three share the arc: a lookup-by-input approach that breaks at `4`, a `(value, symbol)` ordered table that absorbs every remaining scenario, and a chain of `spec —` commits that prove the abstraction paid off.
