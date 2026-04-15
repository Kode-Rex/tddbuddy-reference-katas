// TypeScript idiom: small related types colocated in one module.
// The shared spec names the entity "Task" (see ../SCENARIOS.md); TS has no
// built-in Task collision so the name is kept as-is.

const FIRST_TASK_ID = 1;

export interface Task {
  readonly id: number;
  readonly title: string;
  readonly done: boolean;
  readonly due: string | null;
}

// Message string is identical byte-for-byte across C#, TypeScript, and Python.
// The format "task <id> not found" is part of the spec (see ../SCENARIOS.md).
export class TaskNotFoundError extends Error {
  readonly id: number;

  constructor(id: number) {
    super(`task ${id} not found`);
    this.name = 'TaskNotFoundError';
    this.id = id;
  }
}

export class TodoList {
  private readonly _tasks: Task[] = [];
  private _nextId = FIRST_TASK_ID;

  add(title: string, due: string | null = null): Task {
    const task: Task = { id: this._nextId++, title, done: false, due };
    this._tasks.push(task);
    return task;
  }

  markDone(id: number): void {
    const index = this._indexOrThrow(id);
    const existing = this._tasks[index]!;
    if (!existing.done) {
      this._tasks[index] = { ...existing, done: true };
    }
  }

  remove(id: number): void {
    const index = this._indexOrThrow(id);
    this._tasks.splice(index, 1);
  }

  tasks(): readonly Task[] {
    return [...this._tasks];
  }

  pending(): readonly Task[] {
    return this._tasks.filter((t) => !t.done);
  }

  completed(): readonly Task[] {
    return this._tasks.filter((t) => t.done);
  }

  private _indexOrThrow(id: number): number {
    const index = this._tasks.findIndex((t) => t.id === id);
    if (index < 0) throw new TaskNotFoundError(id);
    return index;
  }
}
