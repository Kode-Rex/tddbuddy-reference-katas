# Markdown Parser — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — direct-to-HTML, block-then-inline two-pass architecture. This document covers only what diverges in TypeScript idiom.

## Exported Surface

TypeScript exports a single `parse` function rather than a static class method. This is idiomatic — a module-level function communicates "no state" more naturally than a class with only static members.

## `replaceAll` for Escape Placeholders

C# uses `string.Replace` which replaces all occurrences by default. TypeScript's `String.prototype.replace` only replaces the first match unless given a regex with the `g` flag. `replaceAll` matches the C# semantics directly and reads more clearly than a regex for literal string replacement.

## Non-Null Assertions on Array Access

With `noUncheckedIndexedAccess` enabled, `lines[i]` returns `string | undefined`. The parser's loop invariants guarantee the index is in bounds, so `lines[i]!` is used throughout. This is a deliberate choice — the alternative (runtime checks on every access) would add noise without catching real bugs in this context.

## `DocumentBuilder` Uses `this` Return Type

The builder methods return `this` instead of `DocumentBuilder`, making the builder subclass-friendly. This is a TypeScript convention that has no C# equivalent — C# builders return the concrete type.

## How to Run

```bash
cd markdown-parser/typescript
npm install
npx vitest run
```
