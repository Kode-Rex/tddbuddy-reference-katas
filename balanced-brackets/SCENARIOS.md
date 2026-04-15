# Balanced Brackets — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

A string of `[` and `]` characters is **balanced** when, reading left to right, every
closing bracket has a matching opening bracket that has not already been closed,
and no opening brackets remain unmatched at the end of the string.

The rules, applied in order:

1. **The empty string is balanced** — vacuously; there is nothing to mismatch.
2. **Every `]` must close a previously seen, still-open `[`** — a `]` with no open `[` to its left is an imbalance.
3. **Every `[` must eventually be closed** — unmatched openers at end of string are an imbalance.
4. **Inputs contain only `[` and `]`** — per the kata spec, other characters are out of scope and the implementations treat them as undefined input. No test covers non-bracket characters.

A running depth counter is sufficient: increment on `[`, decrement on `]`; fail fast if the counter ever goes negative; pass if the final counter is zero.

## API

One function forms the kata surface:

- **`isBalanced(input) → bool`** — returns `true` iff the input's brackets are balanced per the rules above.

## Test Scenarios

1. **Empty string is balanced** — `""` → `true`
2. **Single pair is balanced** — `"[]"` → `true`
3. **Two sequential pairs are balanced** — `"[][]"` → `true`
4. **Nested pair is balanced** — `"[[]]"` → `true`
5. **Deeply nested mixed pairs are balanced** — `"[[[][]]]"` → `true`
6. **Closing before opening is not balanced** — `"]["` → `false`
7. **Alternating reversed pairs are not balanced** — `"][]["` → `false`
8. **Trailing imbalance is not balanced** — `"[][]]["` → `false`
9. **A lone opener is not balanced** — `"["` → `false`
10. **A lone closer is not balanced** — `"]"` → `false`
11. **Unmatched opener at end is not balanced** — `"[[]"` → `false`
12. **Unmatched closer at end is not balanced** — `"[]]"` → `false`
