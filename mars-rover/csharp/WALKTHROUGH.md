# Mars Rover — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one primary entity (`Rover`), two small test-folder builders (`RoverBuilder`, `CommandBuilder`), three value enums (`Direction`, `Command`, `MovementStatus`), and one domain exception (`UnknownCommandException`).

## Scope — Pure Domain Only

No renderer, no CLI, no multi-rover coordination. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Design at a Glance

```
Direction enum: North | East | South | West        Command enum: Forward | Backward | Left | Right
MovementStatus enum: Moving | Blocked

Rover (immutable)
  ├── Position           : (int, int)
  ├── Heading            : Direction
  ├── GridWidth/Height   : int
  ├── Obstacles          : IReadOnlySet<(int, int)>
  ├── Status             : MovementStatus
  ├── LastObstacle       : (int, int)?
  └── Execute(commands)  : Rover          (returns new rover; stops at first obstacle)

RoverBuilder (tests/)                              CommandBuilder (tests/)
  ├── At(x, y)                                       ├── Forward() / Backward()
  ├── Facing(Direction)                              ├── Left() / Right()
  ├── OnGrid(w, h)                                   └── Build() : string
  ├── WithObstacleAt(x, y)
  └── Build() : Rover
```

## Why `Rover` Is Immutable

Every call to `Execute(commands)` returns a **new** `Rover` rather than mutating the receiver. Three wins fall out of that choice:

1. **Tests can hold references to intermediate states** and assert on them without worrying that a later move corrupted earlier assertions. `var start = ...; start.Execute("F"); start.Position` still reads the original.
2. **The builder pattern composes naturally** — `RoverBuilder` mutates an internal field set and only materialises a `Rover` at `Build()`.
3. **No partial state across calls.** A `Blocked` rover is a *result*, not a mutation; the caller that wants to "try again without the obstacle" builds a new rover rather than poking at flags.

## Why `MovementStatus` and `LastObstacle` Belong on the Rover

The kata brief says: "move the rover up to the last possible point and reports the obstacle." The natural shape is a result object — something like `ExecutionResult { Rover, Status, Obstacle? }`. We collapsed that into the `Rover` itself because:

- Every field the result would carry is already a property *of the rover at that moment*. `status` is "what happened last time I tried to move"; `lastObstacle` is "the square that stopped me." Pushing those onto a parallel result type duplicates state.
- Chaining stays clean: `rover.Execute("FF").Execute("RR")` reads without unwrapping. The second `Execute` on a blocked rover still honors the block — no commands run until a fresh rover is built.
- Tests assert `after.Status.Should().Be(Blocked)` and `after.LastObstacle.Should().Be((1, 0))` directly, no destructuring.

The tradeoff: `Rover` carries a nullable `LastObstacle`. C# tuples nullify cleanly (`(int, int)?`), so the cost is an enum flag we'd need anyway.

## Why the Grid Origin Is Top-Left With Y Increasing Southward

The kata brief's worked example (`(0, 0)` facing `S`, commands `FFLFF` → `(2, 2)`) only works if `S forward = y + 1`. That pins the convention — `N` decreases `y`, `S` increases `y`, `E` increases `x`, `W` decreases `x`. Wrapping is standard modular arithmetic on both axes; `Mod(value, modulus)` handles the negative case (`(x % m + m) % m`).

Named constants: `DefaultGridWidth = 100`, `DefaultGridHeight = 100` — the F2 policy favors names over inline literals when the number has meaning.

## Why Two Builders

`RoverBuilder` is the primary entity builder. `CommandBuilder` is sugar: a short chain that emits `"FFLRB"`-style strings. Scenarios with tight literals (`"F"`, `"FFLFF"`) pass strings directly — the `CommandBuilder` earns its keep when a test reads as narrative ("two forwards, turn, two forwards, backward") rather than a cryptic five-character literal. Both builders live in `tests/MarsRover.Tests/` and together stay under the F2 30-line budget per builder.

## Why Domain-Specific `UnknownCommandException`

One exception, not three. The kata has only one invariant violation worth naming — an unparseable command character. We subclass `ArgumentException` because the offender is a user-provided string, not an invalid internal state. Message is `RoverMessages.UnknownCommand = "unknown command"`, byte-identical across C#/TS/Python.

Obstacle blocking is **not** an exception. A blocked rover is a valid result state, not an error condition — the kata brief asks for the rover to "report" the obstacle, which it does via `Status` and `LastObstacle`. Throwing on block would force every call site into `try/catch` to recover a state the domain already has a first-class representation for.

## Why `Direction` / `Command` / `MovementStatus` Are Enums

Three tiny enums, one per file. Each names four (or two) states that the code branches on. The C#/Python idiom is one-type-per-file; TS collapses them into `src/rover.ts` — that divergence is noted in the TS walkthrough rather than forced into alignment.

Rotation and movement use `switch` expressions over the enum rather than math on integer direction indices. The `switch` reads in the same order a human names the directions, and a reader verifies correctness by eyeballing `North => West` rather than decoding `(d + 3) % 4`.

## What Is Deliberately Not Modeled

- **No renderer** — `Rover.ToString()` would be a nice-to-have; tests use `Position`, `Heading`, `Status` directly.
- **No command history** — `Execute` forgets the sequence. Tests that need intermediate states chain calls.
- **No multi-rover world** — no collaborator interface, no shared grid state.
- **No lowercase or whitespace tolerance** — the spec's command character set is `F B L R`, upper-case. Anything else is an `UnknownCommandException`.

Each omission points at an F3 extension.

## Scenario Map

The fifteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/MarsRover.Tests/RoverTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd mars-rover/csharp
dotnet test
```
