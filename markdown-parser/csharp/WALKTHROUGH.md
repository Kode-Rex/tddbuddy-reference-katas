# Markdown Parser — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice.

Rather than stepping through twenty-seven red/green cycles, this walkthrough explains **why the design came out the shape it did**.

## The Design at a Glance

```
MarkdownParser.Parse(markdown) → html
  ├── Block-level scan: headings, lists, blockquotes, code blocks, paragraphs
  └── Inline formatting: bold, italic, links, inline code, escapes
```

Two layers. Block scanning decides which structural element each line belongs to. Inline formatting runs inside each block's content (except code blocks, which are verbatim).

## Why a Static Method, Not an Instance

The parser has no state. It takes a string, returns a string. There is no accumulator, no configuration, no collaborator. Making it a static `Parse` method communicates that — callers cannot accidentally share state between calls.

If the spec evolved to include configurable extensions (e.g. "enable footnotes"), injecting configuration via a constructor would earn its keep. For the current spec, statefulness would be ceremony.

## Why Direct-to-HTML, Not AST

An AST (Abstract Syntax Tree) earns its keep when multiple consumers traverse the same parse result — rendering to HTML, rendering to plain text, extracting links, etc. The spec has one consumer: HTML output. Building an AST would double the code and the test surface for no teaching benefit.

The decision is reversible. If a future requirement added "extract all links" or "render to plain text," introducing an AST node hierarchy at that point would be a clean refactor — the block/inline split already mirrors where nodes would live.

## Why Block-Then-Inline

Markdown's semantics require two-pass processing. A heading is identified by its leading `#` characters — that's a block decision. Bold formatting inside the heading is inline. Conflating the two layers produces a single-pass parser that is harder to extend and harder to read.

The block scan runs first, classifying each line. Then `ApplyInlineFormatting` runs on each block's content. Code blocks skip inline formatting entirely — that's a block-level rule, not an inline-level one.

## Why `DocumentBuilder`

Multi-line markdown tests are hard to read as raw strings. A builder like:

```csharp
new DocumentBuilder()
    .WithListItem("item one")
    .WithListItem("item two")
    .Build();
```

reads as "a document with two list items." The raw equivalent `"- item one\n- item two"` buries the structure in escape characters and delimiter noise. For single-line inputs, raw strings are fine — the builder earns its keep only when multi-line structure matters.

## Why Escape Handling Uses Placeholders

Escaped characters (`\*`, `\_`, `` \` ``) must survive inline formatting without being matched by the bold/italic/code regexes. The cleanest approach is placeholder substitution: replace escapes with sentinel strings before formatting, then restore them after. This avoids fragile negative-lookbehind patterns and keeps each regex focused on its single job.

## Scenario Map

The twenty-seven scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/MarkdownParser.Tests/MarkdownParserTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim.

## How to Run

```bash
cd markdown-parser/csharp
dotnet test
```
