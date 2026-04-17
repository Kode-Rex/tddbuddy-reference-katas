# Snake Game — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Position** | A `(x, y)` coordinate on the board. Value type with equality by coordinates |
| **Direction** | One of `Up`, `Down`, `Left`, `Right`. Value type |
| **Snake** | Ordered list of positions (head first). Moves by prepending a new head and removing the tail (unless growing) |
| **Board** | Rectangular grid with fixed dimensions (width x height). Coordinates range from `(0, 0)` to `(width-1, height-1)` |
| **Food** | A position on the board where food exists. One piece at a time |
| **Game** | Aggregate: board + snake + food + score + state. `tick()` advances one step |
| **Tick** | One step of the game loop: move snake, check collisions, check food, update state |
| **GameState** | One of `Playing`, `GameOver`, `Won` |
| **Score** | Number of food items eaten |
| **FoodSpawner** | Injectable function that selects the next food position from available empty cells |

## Domain Rules

- The snake starts at position `(0, 0)` moving **Right**, with a length of 1.
- Each tick, the snake moves one cell in its current direction.
- If the new head position contains food, the snake grows (tail stays) and score increments.
- If the new head position is empty, the snake moves normally (tail follows).
- If the new head position is outside the board or on the snake's own body, the game is over.
- The snake cannot reverse direction (Right cannot change to Left, Up cannot change to Down).
- One piece of food exists on the board at a time.
- When food is eaten, new food spawns at a random empty cell via the injected `FoodSpawner`.
- Food cannot spawn on the snake's body.
- The game is won when the snake fills the entire board.

## Test Scenarios

### Initial State

1. **A new game has the snake at (0, 0) moving right**
2. **A new game has a score of zero**
3. **A new game is in Playing state**
4. **A new game has food on the board**

### Basic Movement

5. **Snake moving right advances x by one**
6. **Snake moving down advances y by one**
7. **Snake moving left decreases x by one**
8. **Snake moving up decreases y by one**

### Direction Changes

9. **Changing direction to down then ticking moves the snake down**
10. **Cannot reverse direction from right to left**
11. **Cannot reverse direction from up to down**
12. **Cannot reverse direction from left to right**
13. **Cannot reverse direction from down to up**

### Eating Food

14. **Snake eats food and grows by one segment**
15. **Eating food increments the score**
16. **New food spawns after eating at the position chosen by the spawner**

### Wall Collisions

17. **Snake hitting the right wall causes game over**
18. **Snake hitting the bottom wall causes game over**
19. **Snake hitting the left wall causes game over**
20. **Snake hitting the top wall causes game over**

### Self Collision

21. **Snake colliding with its own body causes game over**

### Game Over Behavior

22. **Tick has no effect after game over**

### Winning

23. **Game is won when the snake fills the entire board**
