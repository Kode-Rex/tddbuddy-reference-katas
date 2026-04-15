# Word Wrap

A pure function `wrap(text, width)` that breaks a string of words into lines of a target column width, joining the lines with `\n`. Words are placed whole when they fit; oversize words that exceed the column width are split hard at the width boundary. Runs of whitespace collapse into single word boundaries.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic string-transform kata with **no builders**. The input is a string plus a target width and the output is a single `\n`-joined string — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The classic Kent Beck shape returns a string rather than a list; scenario-as-test naming still carries when the kata handles word-boundary breaks, oversize-word splitting, and whitespace collapsing.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
