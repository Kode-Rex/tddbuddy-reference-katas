# Fluent Calc — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- A new `Calculator` starts unseeded; its `Result()` is `0`.
- `Seed(n)` sets the current value. **Only the first call takes effect** — subsequent `Seed` calls on the same calculator are ignored.
- `Plus(n)` adds `n` to the current value. `Minus(n)` subtracts `n`. Each operation is pushed onto an **undo history**.
- Operations applied before `Seed` are ignored (there is nothing to operate on yet).
- `Undo()` reverses the most recent operation and pushes it onto a **redo stack**. With no operation to undo, `Undo()` has no effect.
- `Redo()` replays the most recently undone operation. With no operation to redo, `Redo()` has no effect.
- Any new `Plus`/`Minus` after an `Undo` clears the redo stack (classic undo/redo semantics).
- `Save()` snapshots the current value and **clears both the undo and redo stacks**. Subsequent `Undo`/`Redo` calls have no effect.
- `Result()` returns the current value as an `int`.
- **The calculator never throws.** Any call on any state returns the chain (or the result) without raising.
- All inputs and the returned value are `int`.

Every chained method (`Seed`, `Plus`, `Minus`, `Undo`, `Redo`, `Save`) returns the calculator itself so the chain continues. `Result` is the terminal call returning `int`.

## API

- **`Seed(n)`** — set the seed. Ignored if already seeded.
- **`Plus(n)`** — add `n`. Ignored if unseeded.
- **`Minus(n)`** — subtract `n`. Ignored if unseeded.
- **`Undo()`** — reverse the most recent undoable operation. No-op if nothing to undo.
- **`Redo()`** — replay the most recently undone operation. No-op if nothing to redo.
- **`Save()`** — clear history; after this, `Undo`/`Redo` have no effect.
- **`Result()`** — return the current value.

## Test Scenarios

1. **A new calculator's result is zero** — `new Calculator().Result()` is `0`.
2. **Seeding sets the starting value** — `Seed(10).Result()` is `10`.
3. **Plus adds to the seeded value** — `Seed(10).Plus(5).Result()` is `15`.
4. **Minus subtracts from the seeded value** — `Seed(10).Minus(4).Result()` is `6`.
5. **Operations chain in order** — `Seed(10).Plus(5).Plus(5).Result()` is `20`.
6. **Subsequent Seed calls are ignored** — `Seed(10).Seed(99).Plus(5).Result()` is `15`.
7. **Plus before Seed is ignored** — `Plus(5).Seed(10).Result()` is `10`.
8. **Undo reverses the most recent operation** — `Seed(10).Plus(5).Undo().Result()` is `10`.
9. **Undo twice reverses two operations** — `Seed(10).Plus(5).Minus(2).Undo().Undo().Result()` is `10`.
10. **Undo with nothing to undo is a no-op** — `new Calculator().Undo().Result()` is `0`; `Seed(10).Undo().Undo().Result()` is `10` (Seed itself is not undoable).
11. **Redo replays the most recently undone operation** — `Seed(10).Plus(5).Minus(2).Undo().Undo().Redo().Result()` is `15`.
12. **Redo with nothing to redo is a no-op** — `Seed(10).Plus(5).Redo().Result()` is `15`.
13. **A new operation after undo clears the redo stack** — `Seed(10).Plus(5).Undo().Plus(3).Redo().Result()` is `13` (the `Redo` has nothing to replay).
14. **The full undo/redo example from the spec** — `Seed(10).Plus(5).Minus(2).Undo().Undo().Redo().Result()` is `15`.
15. **Save clears history so Undo has no effect** — `Seed(10).Plus(5).Minus(2).Save().Undo().Redo().Undo().Plus(5).Result()` is `18`.
