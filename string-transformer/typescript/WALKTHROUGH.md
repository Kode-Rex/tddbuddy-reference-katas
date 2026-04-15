# String Transformer — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Eight Transformations

`cipher`, `slug`, `mask`, and immutable branching are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## The TypeScript Shape

- **`Transformation` is an interface, every concrete is a class** implementing `apply(input: string): string`. TypeScript supports both `class implements` and structurally-typed object literals; here, classes win because each transformation needs a distinct identity for reading and, for `Truncate` / `Repeat` / `Replace`, captured constructor parameters. `Pipeline` holds a `readonly Transformation[]` and folds `apply` across it — the shape mirrors `IEnumerable<ITransformation>.Aggregate` in C#.
- **All transformations live in one `pipeline.ts`** alongside the `Pipeline` and `Transformation` interface. That is an F2 TypeScript convention — C# puts one class per file; TS co-locates small related types in one module. The file is ~100 lines including every strategy and the pipeline.
- **Regex is idiomatic.** `RemoveWhitespace` is `replace(/\s+/g, '')`. `isLetter` and `isSeparator` are one-line regex predicates. These are the shortest readable forms and the definitions match the C# and Python equivalents.
- **`Replace` uses `split(target).join(replacement)`** rather than `String.prototype.replaceAll`. Same semantics for a literal target; zero regex-escaping surprises when the target contains regex metacharacters; works on every Node version that runs the rest of the stack.

## Why `PipelineBuilder` Lives in `tests/`

Same F2 rationale as C#: fifteen scenarios need fifteen pipelines; without a builder each test opens with a `new Pipeline([new Capitalise(), new Reverse()])` that mixes construction noise with the scenario's variation. The builder is 28 lines; within the F2 25-line target for TS, give or take.

## Scenario Map

The fifteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/pipeline.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd string-transformer/typescript
npm install
npx vitest run
```
