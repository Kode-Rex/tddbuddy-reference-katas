# Age Calculator

Given a birthdate and a reference date ("today"), calculate the person's age in whole years.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The inputs are two standard-library date values and the output is an `int`. The date type comes from the standard library, not the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the domain is this thin: each test reads as one sentence about a person whose age the algorithm must compute.

The reference date is **passed in explicitly** rather than read from the clock (no `DateTime.Today`, no `Date.now()`, no `date.today()`). Deterministic tests are the only way to honestly assert "on her seventh birthday she is 7" — the SUT must be a pure function of its two inputs. The teaching carries across all three languages: inject what would otherwise be ambient state.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
