# End of Line Trim

A pure function `trim(input)` that removes trailing whitespace (spaces and tabs) from the end of every line in a string, while preserving line endings (`\r\n` or `\n`) and leading whitespace. Editors routinely leave drifted trailing spaces and tabs in source files; this kata strips them without mangling line structure.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic string-transform kata with **no builders**. The input is a string and the output is a string — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the kata handles mixed line endings; each scenario maps to one test.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
