# Markdown Parser

Build a parser that converts a subset of Markdown to HTML, one feature at a time. Excellent for practicing **test data builders** over a document-processing pipeline with nested inline formatting and block-level element transitions.

## What this kata teaches

- **Test Data Builders** — `DocumentBuilder` constructs multi-line markdown input fluently; tests read as document specifications, not raw string concatenation.
- **Block vs Inline Parsing** — the parser distinguishes block-level elements (headings, paragraphs, lists, blockquotes, code blocks) from inline formatting (bold, italic, links, inline code), mirroring real-world parser architecture.
- **Nested Formatting** — bold wrapping italic, links inside paragraphs, inline code inside bold — exercising composable parsing rules.
- **Edge Case Discipline** — escaped characters, hash-without-space, empty input, and blank-line paragraph separation each have a scenario that names the boundary.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
