# Text Justification — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- `justify(text, width)` accepts a string and a positive integer `width` and returns a list of justified lines.
- **Tokenization.** The input is split on runs of any ASCII whitespace; empty tokens are discarded. Multiple consecutive spaces or tabs collapse into a single word boundary.
- **Line packing is greedy.** Starting from the first word, pack as many words as will fit on the current line such that `sum(wordLens) + (numberOfWords - 1)` — i.e. the words with a single space between each — is less than or equal to `width`. When the next word would push the line past `width`, close the line and start a new one.
- **Full justification** on every line **except the last**:
  - If the line has more than one word, distribute the extra padding spaces evenly across the gaps between words. If the extras do not divide evenly, the left-hand gaps each receive one additional space until the extras are exhausted.
  - If the line has a single word whose length is less than `width`, pad the line on the right with spaces to reach `width`.
  - If the line has a single word whose length is greater than or equal to `width`, the word stands alone unmodified (it may exceed `width`).
- **Last line is left-aligned.** Words are joined by a single space; the line is **not** right-padded to `width`. This matches the typeset convention that the closing line of a paragraph sits flush-left without a trailing ragged pad.
- **Empty input returns an empty list.** A string containing only whitespace is treated as empty input.

## Test Scenarios

1. **Empty string returns empty list** — `justify("", 10)` returns `[]`.
2. **Whitespace-only input returns empty list** — `justify("   \t  ", 10)` returns `[]`.
3. **Single word shorter than width — only line is the last line** — `justify("Word", 10)` returns `["Word"]`.
4. **Words that fit on one line are returned unjustified** — `justify("Hi there", 20)` returns `["Hi there"]`.
5. **Two-line justification with uneven space distribution** — `justify("This is a test", 12)` returns `["This   is  a", "test"]`. Line 1 has content 7 across 2 gaps with 5 padding spaces distributed 3-then-2; line 2 is the left-aligned last line.
6. **Three-line justification** — `justify("This is a very long word", 10)` returns `["This  is a", "very  long", "word"]`. Each non-last line is padded to exactly 10 characters; the last line is left-aligned.
7. **Multiple consecutive whitespace characters collapse** — `justify("This   is   a   test", 12)` returns `["This   is  a", "test"]`, identical to scenario 5.
8. **Single-word non-last line is right-padded to width** — `justify("longword ab", 9)` returns `["longword ", "ab"]`. `"longword"` alone on a line (length 8 < width 9) is padded with one trailing space; `"ab"` is the left-aligned last line.
9. **Word longer than width stands alone and may exceed width** — `justify("verylongword hi", 5)` returns `["verylongword", "hi"]`. The oversize word is placed on its own line unmodified; `"hi"` is the left-aligned last line.
10. **Even space distribution across equal gaps** — `justify("alpha beta gamma delta epsilon", 25)` returns `["alpha  beta  gamma  delta", "epsilon"]`. Line 1 has content 19 across 3 gaps with 6 padding spaces distributed 2-2-2; line 2 is the left-aligned last line.
