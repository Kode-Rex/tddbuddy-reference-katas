# Greeting

A pure function `greet(name)` that personalizes a salutation. The kata starts trivially — `greet("Bob") → "Hello, Bob."` — and grows through six progressive requirements: handle nulls with a stand-in, handle shouts (all-caps names), handle two names, handle many names with an Oxford comma, and finally handle mixed shouts-and-normals by splitting into two greetings.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic string-formatting kata with **no builders**. The input is a name (string, null, or an array of strings) and the output is a string — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the scenario list grows; each of the six scenarios maps to one test.

The TDD Buddy prompt includes two bonus tasks (splitting entries that contain commas, and escaping commas with quoted entries). **Those bonuses are out of scope for this reference implementation** — the six core scenarios are sufficient to demonstrate the F1 shape. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
