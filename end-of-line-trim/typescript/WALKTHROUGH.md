# End of Line Trim — TypeScript Walkthrough

This is an **algorithmic string-transform kata**: the input is a string, the output is a string, and there are no aggregates, value types, or collaborators — the inputs and outputs *are* the domain. The reference lands as a single commit: `src/endOfLineTrim.ts` exports `trim(input: string): string`. A single left-to-right scan walks the input, emits each line with its trailing space/tab run dropped, and preserves the original terminator (`\r\n` or `\n`) verbatim. `tests/endOfLineTrim.test.ts` has one `it()` per scenario in [`../SCENARIOS.md`](../SCENARIOS.md); each test name reads as a sentence from that spec.

**Line-ending policy — CRLF and LF, not lone CR.** The spec calls out Windows `\r\n` and Unix `\n`. A lone `\r` is left as content, not treated as a terminator. With `noUncheckedIndexedAccess` on, `input[i + 1]` is typed `string | undefined`, and the strict equality `=== '\n'` safely short-circuits past the end of the string without an explicit bounds check.

**One pass, no regex.** A regex like `/[ \t]+(?=\r?\n|$)/gm` expresses the rule concisely but hides the terminator-preservation detail behind engine flags. The hand-rolled scan makes the line-ending policy visible where it matters — in the branch that picks `\r\n` versus `\n`.

**Inline literals — deliberate.** The whitespace set is `' '` and `'\t'`, named inline in the `rightTrim` helper. Extracting a `isTrailingWhitespace` predicate would name a two-element set without adding meaning. F1 katas treat the literal rules as the rule itself.
