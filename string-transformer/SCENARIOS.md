# String Transformer — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers the **eight required transformations** chained through a pipeline. The bonus transformations (`cipher`, `slug`, `mask`, immutable branching) are **out of scope** — see the top-level [`README.md`](README.md#stretch-goals-not-implemented-here).

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Transformation** | A single named string operation implementing `apply(string) -> string`. The eight transformations each know one rule and nothing else. Examples: `Capitalise`, `Reverse`, `Truncate(n)`, `Replace(target, replacement)`. |
| **Pipeline** | An ordered, immutable sequence of transformations. `Pipeline.run(input)` applies every transformation in order, feeding each one the output of its predecessor, and returns the final string. A pipeline with no transformations returns its input unchanged. |
| **PipelineBuilder** | Test-folder fluent builder that accumulates transformations and produces a `Pipeline` via `build()`. Each chainable method (`capitalise()`, `reverse()`, `truncate(n)`, …) appends the corresponding transformation. The builder is how scenarios read as one line per variation. |
| **Truncation marker** | The literal three-character string `"..."` appended when `truncate(n)` actually shortens the input. Byte-identical across all three languages — the marker *is* part of the spec. |

## Domain Rules

- **Order matters.** Transformations apply in insertion order; `capitalise().reverse()` is not the same as `reverse().capitalise()`.
- **Capitalise** uppercases every letter that begins a word. A letter begins a word if it is the first character of the input or it immediately follows a non-letter character (whitespace, underscore, hyphen, digit, punctuation — anything that is not an ASCII letter). Letters that are not word-initial are left unchanged. `"hello world"` becomes `"Hello World"`; `"hello_world"` becomes `"Hello_World"`. Empty input stays empty.
- **Reverse** reverses the code-unit sequence of the string. Whitespace and punctuation flip with everything else.
- **RemoveWhitespace** deletes every whitespace character (space, tab, newline). Non-whitespace characters keep their original order.
- **SnakeCase** lowercases the input, then collapses every maximal run of whitespace or hyphens into a single underscore. Leading and trailing separator runs are trimmed away.
- **CamelCase** tokenises on whitespace and hyphens, lowercases the first token, and title-cases every subsequent token, concatenating with no separators. `"Hello World"` and `"HELLO WORLD"` both become `"helloWorld"`.
- **Truncate(n)** returns the input unchanged when `input.length <= n`; otherwise returns the first `n` characters followed by the three-character truncation marker `"..."`.
- **Repeat(n)** returns the input repeated `n` times, joined by a single space. `repeat(1)` returns the original input. `repeat(0)` returns the empty string.
- **Replace(target, replacement)** replaces every non-overlapping occurrence of the literal `target` substring with `replacement`. Target matching is case-sensitive.

## Test Scenarios

1. **Empty pipeline returns the input unchanged** — building a pipeline with no transformations and running `"hello world"` through it returns `"hello world"`.
2. **Capitalise capitalises each word** — `"hello world"` through `capitalise()` returns `"Hello World"`.
3. **Reverse reverses the whole string** — `"hello world"` through `reverse()` returns `"dlrow olleh"`.
4. **RemoveWhitespace drops every space** — `"hello world"` through `removeWhitespace()` returns `"helloworld"`.
5. **SnakeCase lowercases and joins with underscores** — `"hello world"` through `snakeCase()` returns `"hello_world"`.
6. **SnakeCase collapses hyphens and whitespace into single underscores** — `"hello-world test"` through `snakeCase()` returns `"hello_world_test"`.
7. **CamelCase lowercases the first word and title-cases the rest** — `"Hello World"` through `camelCase()` returns `"helloWorld"`.
8. **CamelCase normalises all-uppercase input** — `"HELLO WORLD"` through `camelCase()` returns `"helloWorld"`.
9. **Truncate shortens long input and appends the marker** — `"hello world"` through `truncate(5)` returns `"hello..."`.
10. **Truncate leaves short input untouched** — `"hello world"` through `truncate(50)` returns `"hello world"`.
11. **Repeat produces n space-joined copies** — `"ha"` through `repeat(3)` returns `"ha ha ha"`.
12. **Replace swaps every occurrence of the target** — `"hello world"` through `replace("world", "there")` returns `"hello there"`.
13. **Chaining applies transformations in order** — `"hello world"` through `capitalise().reverse()` returns `"dlroW olleH"`.
14. **Chaining snakeCase then capitalise uppercases letters after underscores** — `"hello world"` through `snakeCase().capitalise()` returns `"Hello_World"` (capitalise treats underscore as a non-letter, so the `w` following it counts as a word start).
15. **Empty input survives capitalise** — `""` through `capitalise()` returns `""`.
