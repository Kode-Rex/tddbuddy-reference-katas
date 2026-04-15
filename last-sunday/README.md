# Last Sunday

Given a month and year, return the date of the last Sunday of that month.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The inputs are two `int`s (year and month, with month 1-indexed per human convention) and the output is a standard-library date value — `DateOnly` in C#, a UTC `Date` in TypeScript, and `datetime.date` in Python. The date type comes from the standard library, not the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the domain is this thin: each test reads as one sentence naming a month whose last Sunday the algorithm must find.

The original TDD Buddy prompt asks for the last Sunday of *each* month in a given year. This reference implementation exposes the single-month primitive — `Find(year, month)` — because the per-year list is just a `map` over `1..12` and the teaching weight lives in the month-level math (how to walk back from the last day of the month to the nearest Sunday). See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
