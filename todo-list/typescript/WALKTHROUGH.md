# Todo List — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — In-Memory List Only

CSV persistence, CLI parsing, sub-tasks, and parent-completion guards are **out of scope**. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The TypeScript Shape

- **`Task` is an interface, not a class.** A task here is a plain data record — no methods, no `this`-sensitive behavior. The interface names the shape; `readonly` on every field gives the immutability guarantee. TS has no `Task` name collision (unlike C# where `System.Threading.Tasks.Task` forced the `TodoTask` rename), so the domain name is kept as-is.
- **`TodoList` is a class** because it owns state (`_tasks`, `_nextId`) plus several methods. The alternative — a factory closing over state — works but the class version reads closer to the aggregate concept the SCENARIOS.md spec describes.
- **All types live in one `src/todoList.ts` module.** The T in F2 TS style conventions: colocate small related types rather than scatter them across three files. `Task`, `TaskNotFoundError`, and `TodoList` are each a handful of lines and read as a single cohesive unit.
- **Due dates are ISO-8601 strings (`"2026-04-20"`), not `Date`.** The kata spec stores dates without time components; `Date` in JS carries a time and a timezone and would need UTC discipline at every boundary. An ISO string round-trips cleanly across JSON and matches the spec's date-only semantics. Callers that need to compute age-vs-today convert at the edge, not in the domain.
- **`TaskNotFoundError` extends `Error`** with `name = 'TaskNotFoundError'` set explicitly — the usual TS pattern for a named domain error. The message string is byte-identical to the C# and Python spellings: `"task <id> not found"`.

## Why Ids Are Monotonic (Same as C#)

`_nextId` is a counter; removed ids are not reused. Same reasoning as the C# walkthrough — a caller holding a reference to "task 2" must never silently have that reference retargeted.

## Why `TaskBuilder` Lives in `tests/`

Same F2 rationale as C#. Runtime task allocation happens through `list.add()`; the builder is for the **assertion side**, letting tests write `expect(task).toEqual(new TaskBuilder().withId(1).titled('buy milk').build())` instead of a positional `{ id: 1, title: 'buy milk', done: false, due: null }` literal. The builder is 22 lines — within the F2 10–30 line budget.

## Scenario Map

The thirteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/todoList.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd todo-list/typescript
npm install
npx vitest run
```
