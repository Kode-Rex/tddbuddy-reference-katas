# String Transformer — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one primary entity (`Pipeline`), one small test-folder builder (`PipelineBuilder`), and a family of tiny strategy classes — one per transformation. Read the repo [Gears section](../../README.md#gears--bridging-tdd-and-bdd) for why middle gear is the deliberate choice, not a corner cut.

## Scope — Eight Transformations

The astro-site spec lists **eight required chainable operations** plus four bonus ideas (`cipher`, `slug`, `mask`, immutable branching). **The bonuses are deliberately out of scope.** See the kata [`README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Design at a Glance

```
ITransformation (interface)           Pipeline (class)
  Apply(string) → string                Run(string) → string
       ▲                                     uses List<ITransformation>
       │
  ┌────┴────────────┬─────────┬─────────┬────────┬─────────┬────────┬─────────┐
Capitalise  Reverse  RemoveWhitespace  SnakeCase  CamelCase  Truncate  Repeat  Replace

PipelineBuilder (tests/) ──Build──> Pipeline
```

Four source files under `src/StringTransformer/` (`ITransformation`, `Pipeline`, `TruncationMarker`, `Transformations.cs` with all eight strategies) and one builder under `tests/StringTransformer.Tests/`. That is the whole F2 surface.

## Why Strategy

Every transformation does the same thing at the shape level — take a string, return a string — but each implements a different rule. The spec even names them as an enumeration: *`capitalise()`, `reverse()`, `removeWhitespace()`, …*. The structural match for "many small things with the same interface, swappable, composable into a sequence" is the **Strategy pattern**.

The alternative is a giant `StringTransformer` class with one method per operation. That reads fine for eight operations — but it couples the pipeline's ordering logic to the rule catalogue. Adding `slug()` or `cipher(n)` means editing the pipeline; testing a single rule in isolation means constructing the whole pipeline. Neither scales past the eight-operation starter set.

With Strategy, each rule is a 5–15-line class with one method. `Pipeline` knows about `ITransformation` and nothing else — it could run a hundred transformations it has never seen, so long as they implement the interface. Adding a new transformation is a new class plus one builder method; no existing code changes.

## Why `Pipeline` Is a Class, Not a Record

`Pipeline` holds a list of transformation strategies. Two pipelines built with "the same" transformations are not meaningfully equal — value equality on a sequence of `ITransformation` references would either be reference-comparison (brittle) or require every strategy to define its own equality. Neither is useful. `Pipeline` is a **behaviour container**, not a value — class is right, record would be theatre.

`Capitalise`, `Reverse`, etc. are classes for the same reason: they are behaviour, parameterised or not. `Truncate(5)` and `Truncate(5)` are interchangeable for a caller's purposes but the equality is not load-bearing; the class form costs nothing and keeps the shape consistent.

## Why `PipelineBuilder` Exists — The F2 Signature Pattern

Fifteen scenarios need fifteen slightly different pipelines. Without a builder, each test opens with something like:

```csharp
var pipeline = new Pipeline(new ITransformation[]
{
    new Capitalise(),
    new Reverse(),
});
```

Two transformations land on three lines plus brackets, and every scenario repeats the collection-initialiser noise. With `PipelineBuilder`, the same setup reads:

```csharp
var pipeline = new PipelineBuilder().Capitalise().Reverse().Build();
```

One line, left-to-right, *exactly* matching the spec's chain-call notation (`capitalise().reverse()`). The builder is about twenty lines (including braces) — squarely inside the F2 10–30-line budget. Its only job is to turn a readable scenario into a constructed `Pipeline`; it holds no state beyond an append-only list of strategies.

## Why `TruncationMarker` Is a Named Constant

F2 is Full-Bake mode: named constants win when a business value appears in tests, implementation, and spec. `"..."` appears in the scenario table, the `Truncate` implementation, and (implicitly) the test assertions. Naming it `TruncationMarker.Value` makes the spec-level concept navigable from code — grep one name and you find every reference. In an F1 kata this would likely stay inline; in F2 it gets a name.

## Notes on Individual Transformations

- **`Capitalise`** uppercases every letter that begins a word. "Word start" is defined as *the first character or the first letter following any non-letter*. This keeps the rule consistent across inputs with whitespace, underscores, hyphens, or digits — in particular, `snakeCase().capitalise()` correctly produces `"Hello_World"` because the `w` after the underscore counts as a word start. The cheap alternative ("split on whitespace, capitalise each token") fails this chain.
- **`SnakeCase`** lowercases the whole string and collapses every maximal run of whitespace-or-hyphens into a single underscore, trimming leading and trailing separator runs. The predicate `IsSeparator` captures exactly what counts as a separator and nothing else.
- **`CamelCase`** tokenises on the same separator set, lowercases the first token, and title-cases every subsequent token. `"HELLO WORLD"` normalises to `"helloWorld"` because every character is lowercased before the first letter of each non-initial token is re-uppercased.
- **`Truncate(n)`** appends `TruncationMarker.Value` *only when it actually shortens the input*, matching scenario 10's "short input untouched" case.
- **`Repeat(n)`** with `n <= 0` returns the empty string — a small piece of defensiveness outside the scenario list but aligned with the description's "repeat the string n times" reading.

## What Is Deliberately Not Modeled

- **No `cipher(n)`, `slug()`, or `mask(n)`** — stretch goals; each is a one-class addition if it were in scope.
- **No immutable-branching pipeline** — `Pipeline.Run` is the only operation; there is no `Pipeline.With(transformation)` that returns a new pipeline. The builder handles composition; the pipeline handles execution.
- **No parser** — the kata is exercised through the builder, not through a DSL string like `"capitalise.reverse"`. That would be a parser kata, not a transformation kata.

## Scenario Map

The fifteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/StringTransformer.Tests/PipelineTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd string-transformer/csharp
dotnet test
```
