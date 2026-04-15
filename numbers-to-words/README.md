# Numbers to Words

Convert a non-negative integer (0–9999) into its English-words spelling — the form you'd use when a number starts a sentence and a style guide forbids leaving it as digits. `21` becomes `"twenty-one"`, `303` becomes `"three hundred three"`, `3466` becomes `"three thousand four hundred sixty-six"`.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The input is an integer, the output is a string — the inputs and outputs *are* the domain. The teaching point is that even a pure string-transform reads as a spec when the tests are named for the grammar rule they exercise: "compound numbers from twenty-one through ninety-nine are hyphenated," "hundreds in a four-digit number name the thousand first." See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.

**Scope.** The TDD Buddy prompt restricts input to non-negative integers up to four digits (0–9999) and follows two grammar rules: spell out numbers that start a sentence, and hyphenate compound numbers from twenty-one through ninety-nine. The reference omits the bonus "fifty-three hundred" four-digit shorthand in favour of the canonical "five thousand three hundred" form; the scenario list calls this out explicitly.
