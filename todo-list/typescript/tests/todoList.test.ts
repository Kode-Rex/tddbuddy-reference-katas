import { describe, it, expect } from 'vitest';
import { TodoList, TaskNotFoundError } from '../src/todoList.js';
import { TaskBuilder } from './taskBuilder.js';

describe('TodoList', () => {
  it('a new todo list has no tasks', () => {
    const list = new TodoList();
    expect(list.tasks()).toEqual([]);
    expect(list.pending()).toEqual([]);
    expect(list.completed()).toEqual([]);
  });

  it('adding a task returns it with an auto-assigned id of 1', () => {
    const list = new TodoList();
    const task = list.add('buy milk');
    expect(task).toEqual(new TaskBuilder().withId(1).titled('buy milk').build());
  });

  it('adding a task with a due date records the due date', () => {
    const list = new TodoList();
    const task = list.add('ship release', '2026-04-20');
    expect(task).toEqual(
      new TaskBuilder().withId(1).titled('ship release').due('2026-04-20').build(),
    );
  });

  it('ids are assigned sequentially in insertion order', () => {
    const list = new TodoList();
    const a = list.add('a');
    const b = list.add('b');
    const c = list.add('c');
    expect([a.id, b.id, c.id]).toEqual([1, 2, 3]);
  });

  it('all tasks listing returns tasks in insertion order', () => {
    const list = new TodoList();
    list.add('a');
    list.add('b');
    list.add('c');
    expect(list.tasks().map((t) => t.title)).toEqual(['a', 'b', 'c']);
  });

  it('marking a task as complete flips its done flag', () => {
    const list = new TodoList();
    list.add('a');
    list.markDone(1);
    expect(list.tasks()[0]!.done).toBe(true);
  });

  it('pending filter excludes completed tasks', () => {
    const list = new TodoList();
    list.add('a');
    list.add('b');
    list.add('c');
    list.markDone(2);
    expect(list.pending().map((t) => t.title)).toEqual(['a', 'c']);
  });

  it('completed filter excludes pending tasks', () => {
    const list = new TodoList();
    list.add('a');
    list.add('b');
    list.add('c');
    list.markDone(2);
    expect(list.completed().map((t) => t.title)).toEqual(['b']);
  });

  it('removing a task deletes it from the list', () => {
    const list = new TodoList();
    list.add('a');
    list.add('b');
    list.remove(2);
    expect(list.tasks().map((t) => t.id)).toEqual([1]);
  });

  it('ids are not reused after removal', () => {
    const list = new TodoList();
    list.add('a');
    list.add('b');
    list.add('c');
    list.remove(2);
    const next = list.add('d');
    expect(next.id).toBe(4);
  });

  it('marking an unknown id raises task-not-found', () => {
    const list = new TodoList();
    expect(() => list.markDone(99)).toThrow(new TaskNotFoundError(99));
    expect(() => list.markDone(99)).toThrow('task 99 not found');
  });

  it('removing an unknown id raises task-not-found', () => {
    const list = new TodoList();
    expect(() => list.remove(99)).toThrow(new TaskNotFoundError(99));
    expect(() => list.remove(99)).toThrow('task 99 not found');
  });

  it('marking an already-completed task is idempotent', () => {
    const list = new TodoList();
    list.add('a');
    list.markDone(1);
    expect(() => list.markDone(1)).not.toThrow();
    expect(list.tasks()[0]!.done).toBe(true);
  });
});
