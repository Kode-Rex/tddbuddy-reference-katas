# Age Calculator — Python Walkthrough

This is an **algorithmic kata**: inputs are two `datetime.date` values (`birthdate` and `today`) and the output is an `int` — so there are no domain builders, no value types, and no collaborators to introduce. The reference lands as a single commit: `src/age_calculator/age_calculator.py` defines `calculate(birthdate: date, today: date) -> int` which subtracts years and then knocks one off if `today`'s `(month, day)` has not yet reached `birthdate`'s `(month, day)` in the reference year. The package `__init__.py` re-exports `calculate` so tests import it as `from age_calculator import calculate`. `tests/test_age_calculator.py` has one function per scenario in [`../SCENARIOS.md`](../SCENARIOS.md); each test name reads as a sentence from that spec.

**Tuple comparison.** Python's native tuple comparison — `(today.month, today.day) < (birthdate.month, birthdate.day)` — is the cleanest rendering of the rule across the three languages. C# can't compose `<` across `ValueTuple<int, int>` without custom overloads; TypeScript has no tuple comparison at all; Python's lexicographic tuple ordering is exactly what the domain calls for. Worth noting as a language-idiom divergence.

**Reference date passed in, not read from the clock — deliberate.** The function takes `today` as a parameter rather than calling `date.today()`. Ambient-clock access would make every birthday-boundary test a flake: "on her seventh birthday she is 7" can only be asserted when the reference date is an explicit input.

**Inline literals — deliberate.** The `1` in `years -= 1` and the tuple comparison stay inline rather than hiding behind a `birthday_has_passed(...)` helper. In F1 katas the function body fits on one screen and the literals *are* the rule. F3 and larger modes prefer named constants; F1 deliberately doesn't.

**Future-birthdate guard.** `birthdate > today` raises `ValueError("birthdate is after today")` — byte-identical message with the C# `ArgumentException` and TS `Error`. The exception type is language-idiomatic; the message string is the shared contract.
