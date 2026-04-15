# Conway's Sequence

Generate terms of the look-and-say sequence (also known as Conway's sequence). Each term is produced by describing the digit runs of the previous term: `"1"` → one 1 → `"11"` → two 1s → `"21"` → one 2, one 1 → `"1211"` → `"111221"` → `"312211"` → ...

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The input is a digit string and the output is a digit string — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. Each implementation exposes two functions: `next_term(term)` for a single step and `look_and_say(seed, n)` for `n` applications (with `n = 0` returning the seed unchanged). See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
