# Maze Walker — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Cell** | A `(row, col)` coordinate in the maze. Value type with equality by coordinates |
| **CellType** | Enumeration: `Open`, `Wall`, `Start`, `End` |
| **Maze** | Immutable rectangular grid of cells. Knows its dimensions, start position, and end position |
| **Walker** | Navigates a maze using BFS (breadth-first search) to find the shortest path from start to end |
| **Path** | Ordered list of cells from start to end (inclusive). Empty when no path exists |
| **Direction** | One of the four cardinal movements: up, down, left, right. No diagonal movement |
| **MazeBuilder** | Fluent builder that constructs a `Maze` from a string-art representation |
| **WalkerBuilder** | Fluent builder that constructs a `Walker` configured with a maze |
| **NoStartCellException** | Thrown when a maze has no start cell |
| **NoEndCellException** | Thrown when a maze has no end cell |
| **MultipleStartCellsException** | Thrown when a maze has more than one start cell |
| **MultipleEndCellsException** | Thrown when a maze has more than one end cell |

## String-Art Maze Format

Mazes are specified as multi-line strings:
- `#` — wall
- `.` — open cell
- `S` — start cell (exactly one required)
- `E` — end cell (exactly one required)

Example:
```
S.#
..#
#.E
```

## Domain Rules

- A maze must have exactly one start cell and exactly one end cell.
- Movement is restricted to the four cardinal directions: up, down, left, right.
- A walker cannot move through walls or outside the maze boundary.
- The walker uses BFS to find the **shortest** path from start to end.
- The path includes both the start and end cells.
- When no path exists, the result is an empty path.

## Test Scenarios

### Maze Construction

1. **A maze can be built from a string-art representation**
   - Given `S.E`, the maze has 1 row, 3 columns, start at (0,0), end at (0,2).

2. **A maze identifies walls correctly**
   - Given `S#E`, the cell at (0,1) is a wall.

3. **A maze without a start cell throws NoStartCellException**
   - Given `..E`, building throws with message "Maze must have exactly one start cell."

4. **A maze without an end cell throws NoEndCellException**
   - Given `S..`, building throws with message "Maze must have exactly one end cell."

5. **A maze with multiple start cells throws MultipleStartCellsException**
   - Given `S.S\n..E`, building throws with message "Maze must have exactly one start cell."

6. **A maze with multiple end cells throws MultipleEndCellsException**
   - Given `S.E\n..E`, building throws with message "Maze must have exactly one end cell."

### Trivial Paths

7. **Start adjacent to end finds a two-cell path**
   - Given `SE`, the path is [(0,0), (0,1)].

8. **A straight horizontal corridor finds the path**
   - Given `S..E`, the path is [(0,0), (0,1), (0,2), (0,3)].

9. **A straight vertical corridor finds the path**
   - Given `S\n.\nE`, the path is [(0,0), (1,0), (2,0)].

### No Path

10. **A wall between start and end returns an empty path**
    - Given `S#E`, the path is empty.

11. **A maze with no reachable end returns an empty path**
    - Given:
      ```
      S.#
      .##
      ##E
      ```
      The path is empty.

### Shortest Path

12. **Walker finds the shortest path around a wall**
    - Given:
      ```
      S.
      #.
      E.
      ```
      The shortest path is [(0,0), (0,1), (1,1), (2,1), (2,0)] with length 5.

13. **Walker picks the shortest of two possible routes**
    - Given:
      ```
      S.E
      ...
      ```
      The shortest path is [(0,0), (0,1), (0,2)] with length 3.

14. **Walker navigates a winding corridor**
    - Given:
      ```
      S.#
      .#.
      ..E
      ```
      The path is [(0,0), (1,0), (2,0), (2,1), (2,2)] with length 5.

### Larger Mazes

15. **Walker solves a 5x5 maze**
    - Given:
      ```
      S.#..
      .#...
      ...#.
      .#..E
      .....
      ```
      A path exists from S to E.

16. **Walker solves a maze requiring backtrack-like exploration**
    - Given:
      ```
      S..#.
      ##...
      ...#E
      ```
      A path exists from S to E.

### Path Properties

17. **The path starts at the start cell**
    - For any solvable maze, the first cell in the path equals the maze's start cell.

18. **The path ends at the end cell**
    - For any solvable maze, the last cell in the path equals the maze's end cell.

19. **Each step in the path is to an adjacent cell**
    - For any solvable maze, consecutive cells in the path differ by exactly one in either row or column (not both).

20. **The path contains no walls**
    - For any solvable maze, no cell in the path is a wall.
