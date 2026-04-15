# Todo List — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one aggregate (`TaskList`), one value (`TodoTask`), one domain exception, one small test-folder builder (`TaskBuilder`). No collaborators, no persistence, no CSV.

## Scope — In-Memory List Only

The original TDD Buddy Todo List prompt describes a CLI (`todo.exe task -t ... -d ...`), CSV persistence, sub-tasks with parent ids, and a rule that parents cannot complete until children complete. **All of that is deliberately out of scope here.** See the kata [`README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Design at a Glance

```
TaskList (aggregate) ──Add──> TodoTask (record)
  _tasks: List<TodoTask>        Id: int
  _nextId: int                  Title: string
                                Done: bool
                                Due: DateOnly?
  MarkDone(id)
  Remove(id)        ──throws──> TaskNotFoundException
  Tasks()
  Pending()
  Completed()

TaskBuilder (tests/) ──Build──> TodoTask
```

Three files under `src/TodoList/` (the aggregate, the value, the domain exception) and one builder under `tests/TodoList.Tests/`. That is the whole F2 surface.

## The `Task` Naming Collision

`Task` is a reserved-in-practice name in .NET: `System.Threading.Tasks.Task` is imported by virtually every file via `ImplicitUsings`. Naming the domain entity `Task` would force every call site to disambiguate with `using` aliases or fully-qualified names, and any test that ever touched async code would have to fight the compiler about which `Task` it meant. The SCENARIOS.md spec names the entity "Task" as domain vocabulary; the C# implementation renames it to **`TodoTask`** to keep the concept clear at the call site. The ubiquitous language doesn't change — only the type name does.

The aggregate has a parallel issue: naming the class `TodoList` inside namespace `TodoList` works but reads awkwardly (`TodoList.TodoList` in cross-namespace references). The aggregate is therefore **`TaskList`** — the namespace carries "todo", the class carries "list of tasks", and the combination reads cleanly in both directions. TypeScript and Python keep the name `TodoList` because neither language has this collision pressure.

## Why `TodoTask` Is a `record`

Tasks have identity (`Id`), but that identity is just another field on a value-shaped thing. Two `TodoTask` values with the same id, title, done, and due are equal — this is what tests assert against builder output. `record` gives value equality, immutability via `with { Done = true }` for `MarkDone`, and a useful `ToString()` for failure diagnostics. The alternative — a mutable class — would force the aggregate to mutate in place, which is fine for the internal list but noisier when tests want to compare "the task I expect" with "the task the list returned".

## Why `MarkDone` Uses `with` Rewrite Rather Than Mutation

Because `TodoTask` is a record, the aggregate's `MarkDone` does `_tasks[index] = existing with { Done = true }`. This keeps `TodoTask` instances immutable — any reference a caller held to the pre-marked task still shows `Done=false`, which is the correct value-semantics behavior. The aggregate owns the collection; the tasks inside it are values that get replaced, not mutated.

`MarkDone` is also **idempotent**: marking an already-complete task is a no-op. The `if (!existing.Done)` guard makes this explicit rather than relying on "re-assigning the same value is harmless". Scenario 13 codifies the idempotence — callers shouldn't have to check `.Done` before calling `MarkDone`.

## Why Ids Are Not Reused

The aggregate holds `_nextId` as a monotonic counter, not `_tasks.Count + 1`. Reusing ids after removal would break any caller that held a reference to the removed id (imagine a UI showing a task marked-done, then the user removes it, then the user adds another — the UI's reference to "task 2" would now point at a different task). Monotonic ids make the domain error `TaskNotFoundException` meaningful: the message `"task 99 not found"` is the truth, not "task 99 has been overwritten."

Scenario 10 codifies this: `remove(2)` then `add` yields id `4`, not a recycled `2`.

## Why `TaskNotFoundException` Is a Named Domain Type

The aggregate rejects unknown ids on `MarkDone` and `Remove`. A naked `InvalidOperationException` or `KeyNotFoundException` would make tests catch by message string, which couples the test to formatting. A named `TaskNotFoundException` lets tests catch by type and assert the message as a separate concern. The message string — `"task <id> not found"` — is byte-identical across C#, TypeScript, and Python, because the message is part of the cross-language spec.

## Why `TaskBuilder` Exists — The F2 Payoff

At first glance a todo list doesn't need a builder: the aggregate already allocates ids, so `list.Add("title")` is the natural setup line. The builder earns its keep on the **assertion side**: tests want to say "I expect the returned task to look like `{ id: 1, title: 'buy milk', done: false, due: null }`", and without the builder that is a constructor call with four positional arguments, three of which are always the default for that scenario.

```csharp
// Without the builder:
task.Should().Be(new TodoTask(1, "buy milk", false, null));

// With the builder:
task.Should().Be(new TaskBuilder().WithId(1).Titled("buy milk").Build());
```

The builder version names the variation and defaults the rest. Nineteen lines of builder including braces — at the low end of the F2 10–30 line budget.

## What Is Deliberately Not Modeled

- **No CSV read/write** — there is no persistence here, so there is no storage collaborator.
- **No argument parser** — the kata's own "Hint" says not to write one.
- **No sub-tasks** — the aggregate holds a flat list. A parent-child tree is an F3 extension.
- **No `IClock`** — the kata does not track due-vs-today, only stores the due date.
- **`MarkUndone`** is not modeled — the kata spec is one-way (mark complete). Adding it is a two-line extension if a consumer ever needs it.

Every omission above points at an F3 extension. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## Scenario Map

The thirteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/TodoList.Tests/TaskListTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd todo-list/csharp
dotnet test
```
