# Mars Rover — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Pure Domain Only

No renderer, no CLI, no multi-rover coordination. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The TypeScript Shape

- **Everything lives in one module, `src/rover.ts`.** `Direction`, `Command`, `MovementStatus` are string-literal union types (`'North' | 'East' | 'South' | 'West'`, etc.). The error class, the `Rover` class, and the `DEFAULT_GRID_WIDTH` / `DEFAULT_GRID_HEIGHT` / `RoverMessages` constants sit alongside. C# and Python split one type per file; TS idiom is to colocate small related types in one module. Don't force TS to split.
- **String-literal unions instead of enums** give exhaustiveness checks with zero runtime cost, pretty-print as their string values in test diagnostics (`expected 'North' to be 'East'`), and avoid the numeric-enum reverse-mapping footgun.
- **Obstacles encoded as `"x,y"` strings in a `Set<string>`.** TypeScript `Set` compares object references, not tuple values — `new Set([[1, 0]]).has([1, 0])` is always `false`. Encoding the coordinate pair as a string (or using a nested `Map<number, Set<number>>`) is the standard workaround. We picked the string encoding for the three-line readability cost over a nested-map abstraction.
- **Rotation uses record lookup tables** (`LEFT_OF` / `RIGHT_OF`) rather than a `switch` expression. In TypeScript the record form is slightly tighter and benefits from `Record<Direction, Direction>` enforcing total coverage at compile time — miss a direction and the compiler complains.
- **`Rover` constructor takes a `RoverParams` options bag.** With eight fields, a positional constructor would be error-prone; the named-parameter object pattern reads at the call site (`new Rover({ x: ..., y: ..., ... })`) and the builder assembles it. This is the TS idiom for "record-shaped" entities where C# uses a positional record.
- **Error class subclasses `Error`** with the message from `RoverMessages.unknownCommand` — byte-identical to the C# and Python spellings. Tests assert on both the class (`toThrow(UnknownCommandError)`) and the message string; the latter is the spec contract.
- **`noUncheckedIndexedAccess` is on**, which is why some helper tables use `Record<Direction, X>` (exhaustive keyed access, no `undefined` from the lookup).

## Why Two Builders Live in `tests/`

Same F2 rationale as C#: fifteen scenarios need an assortment of rover starting states and command strings. `RoverBuilder` (`.at().facing().onGrid().withObstacleAt().build()`) assembles the entity; `CommandBuilder` emits `"FFLRB"`-style strings when a test reads better as narrative than as a cryptic character literal. Each builder is under the 25-line TS F2 budget.

## Scenario Map

The fifteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/rover.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd mars-rover/typescript
npm install
npx vitest run
```
