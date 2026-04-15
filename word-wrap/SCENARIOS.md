# Word Wrap — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- `wrap(text, width)` accepts a string and a positive integer `width` and returns a single string whose lines are joined with `\n`.
- **Tokenization.** The input is split on runs of any ASCII whitespace; empty tokens are discarded. Multiple consecutive spaces or tabs collapse into a single word boundary.
- **Line packing is greedy.** Starting from the first word, pack as many words as will fit on the current line such that `sum(wordLens) + (numberOfWords - 1)` — i.e. the words with a single space between each — is less than or equal to `width`. When the next word would push the line past `width`, close the line and start a new one.
- **Oversize words split hard.** If a single word's length is greater than `width`, the word is broken at the `width`-th character across as many lines as needed. Any remainder of an oversize word that is shorter than `width` begins the next line and may be joined by following words that still fit.
- **Empty input returns the empty string.** A string containing only whitespace is treated as empty input.
- **Lines are joined by `\n`.** The return is a single string, not a list — matching the classic Kent Beck Word Wrap kata shape.

## Test Scenarios

1. **Empty string returns empty string** — `wrap("", 10)` returns `""`.
2. **Whitespace-only input returns empty string** — `wrap("   \t  ", 10)` returns `""`.
3. **Single word shorter than width returned unchanged** — `wrap("Hello", 10)` returns `"Hello"`.
4. **Single word equal to width returned unchanged** — `wrap("Hello", 5)` returns `"Hello"`.
5. **Two words that fit on one line returned unchanged** — `wrap("Hello World", 20)` returns `"Hello World"`.
6. **Two words break at the word boundary when they do not fit** — `wrap("Hello World", 5)` returns `"Hello\nWorld"`.
7. **Two words break at the word boundary when the gap pushes past width** — `wrap("Hello World", 7)` returns `"Hello\nWorld"`. `"Hello World"` is length 11 > 7, so the line closes after `"Hello"`.
8. **Three words across three lines when each line fits one word** — `wrap("Hello wonderful World", 9)` returns `"Hello\nwonderful\nWorld"`.
9. **Oversize single word splits hard at width** — `wrap("Supercalifragilisticexpialidocious", 10)` returns `"Supercalif\nragilistic\nexpialidoc\nious"`. The 34-character word splits across three full-width lines with a 4-character remainder.
10. **Oversize word remainder joins the next word when it fits** — `wrap("abcdefghij kl", 5)` returns `"abcde\nfghij\nkl"`. `"abcdefghij"` splits into `"abcde"` + `"fghij"`; `"kl"` begins a new line because `"fghij kl"` is length 8 > 5.
11. **Multiple consecutive whitespace characters collapse** — `wrap("Hello   World", 5)` returns `"Hello\nWorld"`, identical to scenario 6.
12. **Multi-word multi-line greedy packing** — `wrap("The quick brown fox jumps over the lazy dog", 10)` returns `"The quick\nbrown fox\njumps over\nthe lazy\ndog"`.
