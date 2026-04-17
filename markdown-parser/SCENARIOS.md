# Markdown Parser — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Document** | A complete markdown input string; may contain multiple lines and block elements |
| **Block** | A top-level structural element: paragraph, heading, list, blockquote, or code block |
| **Inline** | Formatting applied within a block: bold, italic, link, inline code |
| **Paragraph** | Plain text wrapped in `<p>...</p>`; separated by blank lines |
| **Heading** | A line starting with 1-6 `#` followed by a space; rendered as `<h1>`-`<h6>` |
| **Bold** | Text wrapped in `**...**`; rendered as `<strong>...</strong>` |
| **Italic** | Text wrapped in `_..._`; rendered as `<em>...</em>` |
| **Link** | `[text](url)` syntax; rendered as `<a href="url">text</a>` |
| **Unordered List** | Consecutive lines starting with `- `; rendered as `<ul><li>...</li></ul>` |
| **Inline Code** | Text wrapped in backticks; rendered as `<code>...</code>` |
| **Code Block** | Lines between triple-backtick fences; rendered as `<pre><code>...</code></pre>` |
| **Blockquote** | Lines starting with `> `; rendered as `<blockquote>...</blockquote>` |
| **Escaped Character** | A backslash before a formatting character suppresses formatting |
| **DocumentBuilder** | Test helper that constructs markdown input fluently for multi-line scenarios |

## Domain Rules

- Empty input produces empty output
- Each line is classified as a block element: heading, list item, blockquote, code-block fence, or paragraph text
- Headings require a space after the `#` characters; `#NoSpace` is a paragraph, not a heading
- Headings do not get wrapped in `<p>` tags
- Consecutive list items are grouped into a single `<ul>` block
- Inline formatting (bold, italic, links, inline code) is applied within paragraphs, headings, list items, and blockquotes
- Bold can nest italic: `**_text_**` produces `<strong><em>text</em></strong>`
- Blank lines separate paragraphs; consecutive non-blank non-special lines form one paragraph
- Code blocks preserve content verbatim — no inline formatting is applied inside them
- Inline code preserves content verbatim — no nested formatting inside backticks
- Escaped characters (`\*`, `\_`, `` \` ``) render as the literal character without formatting
- Blockquote lines starting with `> ` are wrapped in `<blockquote>...</blockquote>`

## Test Scenarios

### Empty and Plain Text

1. **Empty input returns empty output**
2. **Plain text is wrapped in a paragraph**
3. **Multiple words in plain text are preserved**

### Headings

4. **Single hash with space produces h1**
5. **Double hash with space produces h2**
6. **Six hashes with space produces h6**
7. **Hash without space is treated as a paragraph**
8. **Inline formatting inside headings is applied**

### Bold

9. **Double asterisks produce strong**
10. **Bold text surrounded by plain text**

### Italic

11. **Underscores produce emphasis**
12. **Italic text surrounded by plain text**

### Nested Inline Formatting

13. **Bold wrapping italic produces nested tags**
14. **Bold and italic in the same line**

### Links

15. **Link syntax produces anchor tag**
16. **Link surrounded by plain text**

### Unordered Lists

17. **Single list item produces ul with one li**
18. **Multiple consecutive list items grouped into one ul**
19. **Inline formatting inside list items is applied**

### Multiple Paragraphs

20. **Blank line separates two paragraphs**

### Inline Code

21. **Backtick-wrapped text produces code element**
22. **Inline code does not process nested formatting**

### Code Blocks

23. **Fenced code block produces pre and code elements**
24. **Code block preserves content verbatim**

### Blockquotes

25. **Line starting with angle bracket produces blockquote**

### Escaped Characters

26. **Escaped asterisks render as literal asterisks**
27. **Escaped underscores render as literal underscores**
