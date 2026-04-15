# Text Justification

A pure function `justify(text, width)` that breaks a string of words into lines of a target column width and fully justifies each line — padding the gaps between words with extra spaces so the line reaches the target width. The last line is left-aligned with single spaces between words and is **not** right-padded to the target width, matching how most typeset paragraphs end.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic string-transform kata with **no builders**. The input is a string plus a target width and the output is a list of strings — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the kata handles spacing distribution, single-word lines, over-long words, and an unpadded last line.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
