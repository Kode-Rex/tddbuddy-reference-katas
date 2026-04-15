# Anagram Detector

Three pure functions over strings: decide whether two words are anagrams, filter a candidate list to those matching a subject, and group a list into anagram sets.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. Inputs are `string` and `IEnumerable<string>` (or the equivalent); outputs are `bool` and ordered collections of strings — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the domain is this thin: each test reads as one line from the spec table.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification that all three language implementations satisfy.
