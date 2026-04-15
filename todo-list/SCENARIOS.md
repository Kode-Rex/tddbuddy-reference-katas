# Todo List — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **an in-memory `TodoList` aggregate of flat tasks** with add, mark-done, remove, and status-filtered listing. CSV persistence, CLI parsing, and sub-task trees are **out of scope** — see the top-level [`README.md`](README.md#scope--in-memory-todolist-only) for the full stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Task** | An immutable value with `id` (int, auto-assigned by the list), `title` (string), `done` (bool), and an optional `due` date. Two tasks with the same field values are equal. |
| **TodoList** | The aggregate. Owns id allocation, task ordering, and the mark/remove lifecycle. Exposes `add(title, due?)`, `markDone(id)`, `remove(id)`, `tasks()`, `pending()`, `completed()`. |
| **TaskBuilder** | Test-folder fluent builder that produces a `Task`. Defaults: `id=1`, empty title, `done=false`, no due date. Used to construct expected `Task` values for assertions — the aggregate is still the source of truth at runtime. |
| **pending** | A task whose `done` flag is `false`. Mirrors the kata spec's "Incomplete" status. |
| **completed** | A task whose `done` flag is `true`. |
| **TaskNotFound** | The rejection raised when `markDone` or `remove` is called with an id that the list does not contain. Message string is identical byte-for-byte across all three languages: `"task <id> not found"`. |

## Domain Rules

- Ids are 1-indexed and assigned in insertion order. The first `add` returns id `1`, the second returns `2`, and so on.
- Ids are **not reused** after a removal: removing task `2` and then adding a new task yields id `3`, not a recycled `2`.
- `add(title, due?)` accepts an optional due date. When omitted the task's `due` is null/None/undefined depending on language idiom.
- `tasks()` returns every task in insertion order regardless of status.
- `pending()` returns tasks whose `done` is `false`, in insertion order.
- `completed()` returns tasks whose `done` is `true`, in insertion order.
- `markDone(id)` flips the task's `done` flag to `true`. Marking an already-completed task is a no-op (idempotent).
- `remove(id)` deletes the task from the list. Subsequent filter queries will not return it.
- `markDone(id)` and `remove(id)` raise a `TaskNotFound` domain error when the id is unknown. The message is `"task <id> not found"`.

## Test Scenarios

1. **A new todo list has no tasks** — `tasks()`, `pending()`, and `completed()` are all empty.
2. **Adding a task returns it with an auto-assigned id of 1** — the first `add("buy milk")` returns a `Task` with `id=1`, `title="buy milk"`, `done=false`, no due date.
3. **Adding a task with a due date records the due date** — `add("ship release", 2026-04-20)` returns a task whose `due` is `2026-04-20`.
4. **Ids are assigned sequentially in insertion order** — three successive `add` calls return tasks with ids `1`, `2`, `3`.
5. **All tasks listing returns tasks in insertion order** — after adding `"a"`, `"b"`, `"c"`, `tasks()` returns them in that order.
6. **Marking a task as complete flips its done flag** — `markDone(1)` causes `tasks()[0].done` to be `true`.
7. **Pending filter excludes completed tasks** — with `"a"`, `"b"`, `"c"` added and task `2` marked done, `pending()` returns only `"a"` and `"c"`.
8. **Completed filter excludes pending tasks** — with `"a"`, `"b"`, `"c"` added and task `2` marked done, `completed()` returns only `"b"`.
9. **Removing a task deletes it from the list** — after `remove(2)`, no filter returns task `2` and `tasks()` has two entries.
10. **Ids are not reused after removal** — add three tasks (ids 1, 2, 3), `remove(2)`, then add another; the new task has id `4`, not `2`.
11. **Marking an unknown id raises task-not-found** — `markDone(99)` on a list without id `99` raises a domain error whose message is `"task 99 not found"`.
12. **Removing an unknown id raises task-not-found** — `remove(99)` on a list without id `99` raises a domain error whose message is `"task 99 not found"`.
13. **Marking an already-completed task is idempotent** — calling `markDone(1)` twice leaves the task with `done=true` and does not raise.
