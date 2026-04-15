# End of Line Trim — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- `trim(input)` accepts a string and returns a string.
- **Trailing whitespace** is any run of ASCII space (`" "`) or horizontal tab (`"\t"`) characters immediately preceding a line terminator or the end of the string.
- **Line terminators** are preserved verbatim: a `\r\n` pair stays `\r\n`, a bare `\n` stays `\n`. Mixed endings within the same input are handled correctly.
- **Leading whitespace is not touched.** `"  abc"` returns `"  abc"`.
- A line that is only whitespace collapses to empty, but its terminator is preserved — `"  \n"` returns `"\n"`.
- A final line without a terminator is still right-trimmed — `"abc  "` returns `"abc"`.
- An empty string returns an empty string.
- A string that is only a line terminator returns that line terminator unchanged — `"\r\n"` returns `"\r\n"`.

## Line-ending Policy

The implementation recognizes `\r\n` and `\n` as line terminators and preserves whichever form each line used. A lone `\r` (old Mac OS line ending) is treated as ordinary content — it is not a terminator in this kata and is not stripped, because the TDD Buddy spec names only Windows and Unix line endings.

## Test Scenarios

1. **No trailing whitespace** — `"abc"` returns `"abc"`.
2. **Trailing space** — `"abc "` returns `"abc"`.
3. **Trailing tab** — `"abc\t"` returns `"abc"`.
4. **Leading whitespace is preserved** — `" abc"` returns `" abc"`.
5. **Trailing whitespace on multiple lines (CRLF)** — `"ab\r\n cd \r\n"` returns `"ab\r\n cd\r\n"`.
6. **Line terminator only (CRLF)** — `"\r\n"` returns `"\r\n"`.
7. **Trailing whitespace on multiple lines (LF)** — `"ab\n cd \n"` returns `"ab\n cd\n"`.
8. **Whitespace-only line collapses but keeps its terminator** — `"  \n"` returns `"\n"`.
9. **Empty string** — `""` returns `""`.
10. **Mixed line endings are preserved per line** — `"ab \r\ncd \nef "` returns `"ab\r\ncd\nef"`.
