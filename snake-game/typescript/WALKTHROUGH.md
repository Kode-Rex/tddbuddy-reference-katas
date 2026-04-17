# Snake Game — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (bounded grid, mutable Game aggregate, immutable Snake movement, injectable FoodSpawner, wall/self collision detection).

This note captures only the TypeScript deltas.

## Direction as a String Union

In C#, `Direction` is an enum with extension methods. In TS, `Direction` is a `'Up' | 'Down' | 'Left' | 'Right'` string union type — no runtime enum overhead, and the exhaustive switch expression in `move()` provides the same compile-time safety. `isOpposite` is a free function with a lookup record.

## Position as a Plain Interface

TypeScript objects don't have structural equality like C#'s `record struct`. Two `{ x: 0, y: 0 }` objects are not `===` to each other. The `positionsEqual` function handles value comparison where needed (collision detection, food check). Unlike the Game of Life kata, we don't need to store positions in a `Set`, so there's no `positionKey` needed at runtime — though it's exported for potential future use.

## GameState as a String Union

`'Playing' | 'GameOver' | 'Won'` — same approach as Direction. No enum class needed.

## Game Constructor Overload

TS doesn't have `internal` visibility. Instead, the `Game` constructor accepts optional parameters (`snake?`, `food?`, `score?`, `state?`) with defaults. The `BoardBuilder` passes all parameters explicitly. This achieves the same testability without access modifiers.

## Scenario Map

Twenty-three scenarios across eight test files:

- `tests/initialState.test.ts` — scenarios 1–4
- `tests/basicMovement.test.ts` — scenarios 5–8
- `tests/directionChanges.test.ts` — scenarios 9–13
- `tests/eatingFood.test.ts` — scenarios 14–16
- `tests/wallCollisions.test.ts` — scenarios 17–20
- `tests/selfCollision.test.ts` — scenario 21
- `tests/gameOverBehavior.test.ts` — scenario 22
- `tests/winning.test.ts` — scenario 23

One `it()` per scenario; test names lowercase-match the SCENARIOS titles.

## How to Run

```bash
cd snake-game/typescript
npm install
npx vitest run
```
