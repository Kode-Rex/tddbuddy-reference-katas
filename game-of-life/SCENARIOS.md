# Game of Life — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Cell** | A `(row, col)` coordinate on the infinite plane. Value type with equality by coordinates |
| **Grid** | Immutable set of living cells. The domain entity. `tick()` produces the next generation |
| **Living Cell** | A cell present in the grid's set |
| **Dead Cell** | Any cell not present in the grid's set (infinite — never enumerated) |
| **Neighbor** | One of the eight cells orthogonally and diagonally adjacent to a cell |
| **Tick** | One application of the four rules, producing a new `Grid` |
| **Still Life** | A pattern unchanged by `tick()` (e.g., Block) |
| **Oscillator** | A pattern that cycles through a fixed period of states (e.g., Blinker, period 2) |
| **Spaceship** | A pattern that translates across the grid over time (e.g., Glider, period 4) |

## Domain Rules

- A **live cell** with **fewer than two** live neighbors **dies** (underpopulation).
- A **live cell** with **two or three** live neighbors **survives** to the next generation.
- A **live cell** with **more than three** live neighbors **dies** (overcrowding).
- A **dead cell** with **exactly three** live neighbors **becomes alive** (reproduction).

## Test Scenarios

### Empty and Trivial

1. **An empty grid ticks to an empty grid**
2. **A single living cell dies after one tick**

### Individual Rules

3. **A live cell with zero neighbors dies (underpopulation)**
4. **A live cell with one neighbor dies (underpopulation)**
5. **A live cell with two neighbors survives**
6. **A live cell with three neighbors survives**
7. **A live cell with four neighbors dies (overcrowding)**
8. **A dead cell with exactly three neighbors becomes alive (reproduction)**

### Still Lifes

9. **Block (2x2 square) is stable after one tick**
10. **Block remains stable after multiple ticks**

### Oscillators

11. **Horizontal blinker becomes vertical after one tick**
12. **Vertical blinker becomes horizontal after one tick**
13. **Blinker returns to its original state after two ticks (period 2)**

### Spaceships

14. **Glider translates one cell down and right after four ticks**

### Grid Queries

15. **isAlive returns true for a living cell**
16. **isAlive returns false for a dead cell**
17. **livingCells returns all living cells in the grid**
18. **An empty grid has no living cells**
