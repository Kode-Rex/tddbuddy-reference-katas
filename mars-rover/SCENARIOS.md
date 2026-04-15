# Mars Rover — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **the rover command interpreter only**: position, heading, movement, rotation, grid wrapping, and obstacle detection. Rendering, CLI loops, alternative command formats, and multi-rover coordination are **out of scope** — see the top-level [`README.md`](README.md#scope--pure-domain-only) for the full stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Rover** | An immutable aggregate of `position = (x, y)`, `heading: Direction`, `grid: (width, height)`, `obstacles: Set<(x, y)>`, `status: MovementStatus`, `lastObstacle: (x, y)?`. Exposes `execute(commands: string) → Rover`. |
| **Direction** | Typed enum `N | E | S | W`. Named `Direction.North`, `Direction.East`, etc. per language idiom. |
| **Command** | Typed enum `F | B | L | R`. The character form (`'F'`, etc.) is the wire format; the enum is the parsed form. |
| **MovementStatus** | Typed enum `Moving | Blocked`. A freshly built rover is `Moving`; it flips to `Blocked` the first time a move is rejected by an obstacle and stays `Blocked` for the remainder of that `execute` call. A subsequent `execute` on a blocked rover still reads the obstacle set — callers who want to "reset" build a new rover. |
| **Grid** | A `(width, height)` rectangle. Coordinates are zero-indexed: `(0, 0)` is top-left, `(width - 1, height - 1)` is bottom-right. `N` forward is `y - 1`, `S` forward is `y + 1`. |
| **Wrap** | Stepping off one edge re-enters from the opposite edge. `(x + width) mod width` and `(y + height) mod height`. |
| **Obstacle** | A `(x, y)` square that blocks `F` and `B` moves into it. Rover stops at the square *before* the obstacle; `lastObstacle` records the blocked destination. |
| **RoverBuilder** | Test-folder fluent builder that produces a `Rover`. `.at(x, y).facing(Direction).onGrid(w, h).withObstacleAt(x, y).build()`. |
| **CommandBuilder** | Test-folder fluent builder that produces a command string. `.forward().backward().left().right().build()` emits `"FBLR"`. Scenarios with short explicit sequences (e.g. `"FFLFF"`) can pass the literal string directly; the builder earns its keep when a test needs a narrative ("three forwards, turn right, two forwards"). |

## Domain Rules

- A rover's starting `status` is `Moving` with `lastObstacle` unset.
- `F` moves one square in the heading direction; `B` moves one square opposite the heading.
- `L` rotates the heading one quarter-turn counter-clockwise (`N → W → S → E → N`). `R` rotates one quarter-turn clockwise (`N → E → S → W → N`).
- The grid wraps on both axes. Default grid is `100 × 100` (`DefaultGridWidth = 100`, `DefaultGridHeight = 100`).
- Before each move (`F` or `B`), the rover computes the wrapped destination and checks the obstacle set. If the destination is an obstacle, the rover does not move, sets `status = Blocked`, sets `lastObstacle` to the destination coordinates, and ignores the remaining commands in the sequence.
- Rotation commands (`L`, `R`) execute regardless of status *during the same `execute` call* — once a rover is `Blocked`, all remaining commands in that call are discarded.
- Unknown command characters raise a domain exception. Message: `"unknown command"`.
- An empty command string leaves the rover unchanged (same position, heading, status).
- Exception types per language: `UnknownCommandException` (C# — subclass of `ArgumentException`), `UnknownCommandError` (TS — subclass of `Error`), `UnknownCommandError` (Python — subclass of `ValueError`). The message string is byte-identical across all three.

### Exception Messages

| Rule | Message |
|------|---------|
| command character not in `F B L R` | `"unknown command"` |

## Test Scenarios

1. **New rover reports its starting position, heading, and Moving status** — `RoverBuilder().at(3, 4).facing(North).onGrid(100, 100).build()` has `position == (3, 4)`, `heading == North`, `status == Moving`, `lastObstacle` unset.
2. **Forward moves one square in the direction the rover is facing** — a rover at `(5, 5)` facing `North` executing `"F"` lands at `(5, 4)`; facing `East` lands at `(6, 5)`; facing `South` lands at `(5, 6)`; facing `West` lands at `(4, 5)`.
3. **Backward moves one square opposite the heading** — a rover at `(5, 5)` facing `North` executing `"B"` lands at `(5, 6)`; facing `East` lands at `(4, 5)`.
4. **Left rotates the heading counter-clockwise** — a rover facing `North` executing `"L"` now faces `West`; `"LL"` faces `South`; `"LLL"` faces `East`; `"LLLL"` faces `North` again. Position does not change.
5. **Right rotates the heading clockwise** — a rover facing `North` executing `"R"` now faces `East`; `"RR"` faces `South`; `"RRR"` faces `West`; `"RRRR"` faces `North` again. Position does not change.
6. **Execute applies commands in order** — a rover at `(0, 0)` facing `North` on a 100x100 grid executing `"FFRFF"` lands at `(2, -2 mod 100) = (2, 98)` facing `East`. (Two forwards wrap north; turn right; two forwards east.)
7. **Kata-brief example** — a rover at `(0, 0)` facing `South` on a 100x100 grid executing `"FFLFF"` lands at `(2, 2)` facing `East`.
8. **Wrapping across the north edge** — a rover at `(0, 0)` facing `North` executing `"F"` on a 100x100 grid lands at `(0, 99)`.
9. **Wrapping across the east edge** — a rover at `(99, 50)` facing `East` executing `"F"` on a 100x100 grid lands at `(0, 50)`.
10. **Obstacle blocks a forward move** — a rover at `(0, 0)` facing `East` with an obstacle at `(1, 0)` executing `"F"` stays at `(0, 0)`, `status == Blocked`, `lastObstacle == (1, 0)`.
11. **Remaining commands after block are discarded** — a rover at `(0, 0)` facing `East` with an obstacle at `(2, 0)` executing `"FFRFF"` lands at `(1, 0)` facing `East`, `status == Blocked`, `lastObstacle == (2, 0)`. (First `F` moves to `(1, 0)`; second `F` blocks at `(2, 0)`; `R` and the trailing `F F` are not applied.)
12. **Obstacle blocks a backward move** — a rover at `(2, 0)` facing `East` with an obstacle at `(1, 0)` executing `"B"` stays at `(2, 0)`, `status == Blocked`, `lastObstacle == (1, 0)`.
13. **Empty command string leaves the rover unchanged** — `rover.execute("")` returns a rover with the same position, heading, status, and obstacle set.
14. **Unknown command character raises an error** — `rover.execute("FX")` raises `UnknownCommandException` / `UnknownCommandError` with message `"unknown command"`. The unknown command is detected as it is encountered; commands before it have already been applied, but the failing `execute` call signals the error rather than returning a partially-executed rover.
15. **RoverBuilder and CommandBuilder produce the literal the test describes** — `RoverBuilder().at(2, 3).facing(West).onGrid(10, 10).withObstacleAt(1, 3).build()` has `position == (2, 3)`, `heading == West`, the single obstacle; `CommandBuilder().forward().forward().left().right().backward().build() == "FFLRB"`.
