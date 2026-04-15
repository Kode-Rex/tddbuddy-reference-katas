# Todo List

A `TodoList` aggregate holds an ordered collection of `Task` values. Callers `add` a task (with an optional due date), `markDone` by id, `remove` by id, and filter the list by status — all tasks, pending only, or completed only. Ids are auto-assigned monotonically and are never reused.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**: one primary aggregate (`TodoList`), one value type (`Task`), one small test-folder builder (`TaskBuilder`). No persistence, no CLI parsing, no collaborators. The teaching point is the builder payoff plus a clean aggregate API that keeps id allocation and filtering inside the domain, not leaking into callers.

## Scope — In-Memory TodoList Only

The original TDD Buddy prompt on the astro-site describes a CLI (`todo.exe task -t ... -d ...`), CSV persistence (`save your data to a text file in csv format`), and three bonus features (sub-tasks with a parent id, parent-header output formatting, and blocking parent completion until children are complete). **All of that is deliberately out of scope here.** CSV serialization is an I/O concern, argument parsing is out of scope per the kata's own "Hint", and the sub-task tree is a richer domain that would need a second entity and a recursion story. The kata spec itself flags these as bonus.

This reference is scoped to the **in-memory list of flat tasks**: add, mark done, remove, list all, list pending, list completed.

### Stretch Goals (Not Implemented Here)

These are deliberately left as extensions:

- **CSV persistence** — read/write a todo file with a storage collaborator
- **CLI argument parsing** — the `todo.exe task -t ... -d ...` / `list -s [All|Incomplete]` / `-c [Id]` commands
- **Sub-tasks** — parent/child relationships on `add(title, due, parentId)`
- **Nested listing** — the `> Child Task <` header rendering under each parent with children
- **Parent-completion guard** — refuse to mark a parent done until every child is done

Each belongs alongside a todo list, but each pulls the kata toward F3 (collaborators, recursion, formatting). See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference does satisfy.
