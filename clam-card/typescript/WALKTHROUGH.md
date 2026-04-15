# Clam Card — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Pure Domain Only

Daily per-zone cap only. No weekly/monthly caps, no return-journey discount, no bank-account collaborator. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The TypeScript Shape

- **Everything lives in one module, `src/card.ts`.** `Zone` (string literal union), `Ride` (interface), `Card`, `JourneyStart`, `UnknownStationError`, and the fare/cap constants sit alongside one another. C# and Python split one type per file; TS idiom is to colocate small related types in one module. Don't force TS to split.
- **`Zone` is a string literal union `'A' | 'B'`**, not an enum. TS's string unions give structural equality at the call site (`zone === 'B'` reads like the spec) and avoid the runtime-object cost of a numeric enum. No one needs `Zone.A` reflection.
- **`Ride` is a readonly interface, not a class.** Every field is `readonly`. Structural equality via `toEqual` handles the ride-record scenario. Same model as C#'s `readonly record struct`; TS expresses it with an interface + plain object literals.
- **`Card` uses `Math.max(0, Math.min(singleFare, remaining))` for cap math** — same arithmetic as C# and Python. Floating-point arithmetic is used directly; the spec fare amounts are all sums of `.00` and `.50`, which JavaScript's IEEE 754 doubles represent exactly.
- **`JourneyStart` is a standalone class** with an internal `completeJourney` hook on `Card`. TS has no sealed-nested-class idiom the way C# does; the `/** @internal */` JSDoc and the `card.ts` module boundary carry the "this is a test-folder hook" signal.
- **`UnknownStationError extends Error`** with the message string from `CardMessages`. Byte-identical to the C# and Python spellings. Tests assert on both the class and the message string.
- **The card holds a `ReadonlyMap<string, Zone>`** of known stations. `Map` is the idiomatic JS answer to C#'s `Dictionary`; the `Readonly` wrapper signals the card does not mutate it.

## Why the Builders Share `tests/cardBuilder.ts`

Both `CardBuilder` and `RideBuilder` live in the same file. TS idiom groups small related builders; neither is long enough to earn its own module. `CardBuilder` stages the network (`withZone`) and accepts an `onDay` date for scenario readability (no behaviour at F2). `RideBuilder` constructs `Ride` literals for the ride-equality scenario.

`CardBuilder` lands at ~20 lines, `RideBuilder` at ~12 — the combined file is inside the 10–30 line F2 TypeScript budget for the primary builder.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/card.test.ts`, one `it()` per scenario, titles matching the scenario statements.

## How to Run

```bash
cd clam-card/typescript
npm install
npx vitest run
```
