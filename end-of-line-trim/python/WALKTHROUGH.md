# End of Line Trim — Python Walkthrough

This is an **algorithmic string-transform kata**: the input is a string, the output is a string, and there are no aggregates, value types, or collaborators — the inputs and outputs *are* the domain. The reference lands as a single commit: `src/end_of_line_trim/end_of_line_trim.py` defines `trim(input: str) -> str`. A single left-to-right scan walks the input, emits each line with its trailing space/tab run dropped, and preserves the original terminator (`\r\n` or `\n`) verbatim. The package `__init__.py` re-exports `trim` so tests import it as `from end_of_line_trim import trim`. `tests/test_end_of_line_trim.py` has one function per scenario in [`../SCENARIOS.md`](../SCENARIOS.md); each test name reads as a sentence from that spec.

**Line-ending policy — CRLF and LF, not lone CR.** The spec calls out Windows `\r\n` and Unix `\n`. A lone `\r` is left as content, not treated as a terminator. Python's `str.splitlines()` would silently swallow a bare `\r` as a terminator — which is why the reference hand-rolls the scan rather than reaching for `splitlines()` / `rstrip()` per line.

**One pass, accumulate into a list, join once.** Appending to a list and `"".join(parts)` at the end is the idiomatic Python replacement for `StringBuilder`. A regex like `re.sub(r'[ \t]+(?=\r?\n|\Z)', '', input)` would also work, but the hand-rolled scan makes the CRLF-vs-LF branch explicit where the policy lives.

**Inline literals — deliberate.** The whitespace set is `" "` and `"\t"`, named inline in `_right_trim`. Extracting a `_is_trailing_whitespace` predicate would name a two-element set without adding meaning. F1 katas treat the literal rules as the rule itself.
