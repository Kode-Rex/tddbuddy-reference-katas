# Code Breaker — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Feedback Engine Only

Random secret generation, attempt tracking, and the game loop are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## The TypeScript Shape

- **`Peg` is a const-object plus a derived type** (`Peg.One = 1 as const`, `type Peg = (typeof Peg)[keyof typeof Peg]`) rather than a real `enum`. TypeScript's `enum` keyword has well-known ergonomic issues — reverse mapping, const-enum pitfalls under isolated modules — and the const-object idiom gives the same literal-type safety without them. Tests destructure the names once (`const { One, Two, … } = Peg`) and the builder calls read just like the C# `using static Peg;` version.
- **`Secret`, `Guess`, and `Feedback` are interfaces plus factory functions** (`createSecret`, `createGuess`, `createFeedback`). Classes here would earn nothing — no inheritance, no runtime `instanceof` checks. The interfaces name the shape; the factories close over the peg tuple (or counts) and return frozen-by-convention objects. `readonly` on every field is the TypeScript equivalent of `record` / `frozen dataclass` immutability.
- **`CodePegs = readonly [Peg, Peg, Peg, Peg]`** — a length-4 tuple type. The type system guarantees four elements, which is why the `CODE_LENGTH` constant and the runtime length check can stay as a single number (used for `isWon` derivation) instead of a guard in the factory.
- **Types are colocated in one module** (`src/codeBreaker.ts`). TS idiom is to group small related types; splitting each into its own file the way C# does would fight the single-file module ergonomic. This divergence is one the F2 conventions explicitly allow.

## Why Two Builders Live in `tests/`

Same F2 rationale as C#: twelve scenarios need twelve secret+guess pairs, and without builders each test opens with two raw tuple literals. With `SecretBuilder` and `GuessBuilder`:

```ts
const secret = new SecretBuilder().with(One, Two, Three, Four).build();
const guess  = new GuessBuilder().with(One, Two, Four, Three).build();
```

One fluent line each; the four-peg argument list reads left-to-right as the code. Twelve lines per builder — the F2 budget.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/codeBreaker.test.ts`, one `it()` per scenario. Scenario 1's `5566` substitutes for the spec's `5678` to respect the 1–6 peg range; the feedback behavior is identical.

## How to Run

```bash
cd code-breaker/typescript
npm install
npx vitest run
```
