# Leap Year

Determine whether a given Gregorian year is a leap year using the standard cascade of rules: a year is leap if divisible by `4`, **except** centuries that are not divisible by `400`.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The input is an `int` and the output is a `bool` — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the domain is this thin: each test reads as one line from the spec table.

The astro-site kata prompt mentions input-validation concerns (zero, negative years, non-integer inputs). **Those are out of scope for this reference implementation** — the four-line Gregorian rule cascade is sufficient to demonstrate the F1 shape. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
