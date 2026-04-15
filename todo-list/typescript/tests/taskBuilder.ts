import type { Task } from '../src/todoList.js';

const DEFAULT_ID = 1;
const DEFAULT_TITLE = '';

export class TaskBuilder {
  private _id = DEFAULT_ID;
  private _title = DEFAULT_TITLE;
  private _done = false;
  private _due: string | null = null;

  withId(id: number): this { this._id = id; return this; }
  titled(title: string): this { this._title = title; return this; }
  due(due: string): this { this._due = due; return this; }
  done(): this { this._done = true; return this; }

  build(): Task {
    return { id: this._id, title: this._title, done: this._done, due: this._due };
  }
}
