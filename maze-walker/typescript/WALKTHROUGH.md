# Maze Walker — TypeScript Walkthrough

Same design as the C# implementation. This walkthrough covers the TypeScript-specific differences.

## Cell as Interface + Free Functions

TypeScript lacks `record struct`. `Cell` is a plain `{ row, col }` interface with `cellKey()` / `parseKey()` free functions for set-based equality — the same pattern used in Game of Life. The `cardinalNeighbors()` function returns an array of four neighbors rather than using a generator.

## Maze Grid as `CellType[][]`

The maze stores its grid as a nested array rather than C#'s 2D array. Access is `grid[row]![col]` with TypeScript's `noUncheckedIndexedAccess` enforcing the `!` assertion.

## Walker Uses Array-Based Queue

Instead of a `Queue<T>`, the walker uses an array with a `head` index for BFS — idiomatic for small grids where the allocation pattern doesn't matter. String keys (`"row,col"`) serve as map keys since JavaScript objects compare by reference.

## Exception Types Extend Error

TypeScript's `Error` replaces C#'s `Exception`. Each domain exception sets its `name` property so `instanceof` checks work correctly. The message strings are byte-identical across languages.

## How to Run

```bash
cd maze-walker/typescript
npm install
npx vitest run
```
