import { describe, it, expect } from 'vitest';
import { LinkedList } from '../src/linkedList.js';

function listOf(...values: number[]): LinkedList<number> {
  const list = new LinkedList<number>();
  for (const value of values) list.append(value);
  return list;
}

describe('LinkedList', () => {
  it('a new list is empty', () => {
    const list = new LinkedList<number>();

    expect(list.size()).toBe(0);
    expect(list.toArray()).toEqual([]);
  });

  it('appending to an empty list yields a single element', () => {
    const list = new LinkedList<number>();

    list.append(1);

    expect(list.size()).toBe(1);
    expect(list.toArray()).toEqual([1]);
  });

  it('appending preserves insertion order', () => {
    const list = new LinkedList<number>();

    list.append(1);
    list.append(2);
    list.append(3);

    expect(list.toArray()).toEqual([1, 2, 3]);
  });

  it('prepending to an empty list yields a single element', () => {
    const list = new LinkedList<number>();

    list.prepend(1);

    expect(list.toArray()).toEqual([1]);
  });

  it('prepending puts the value at the front', () => {
    const list = new LinkedList<number>();
    list.append(1);
    list.append(2);

    list.prepend(0);

    expect(list.toArray()).toEqual([0, 1, 2]);
  });

  it('get returns the value at the given index', () => {
    const list = listOf(0, 1, 2);

    expect(list.get(0)).toBe(0);
    expect(list.get(2)).toBe(2);
  });

  it('get on an out-of-range index raises', () => {
    const list = listOf(0, 1, 2);

    expect(() => list.get(5)).toThrow(new RangeError('index out of range: 5'));
  });

  it('get on a negative index raises', () => {
    const list = listOf(0, 1, 2);

    expect(() => list.get(-1)).toThrow(new RangeError('index out of range: -1'));
  });

  it('remove returns the value and shifts subsequent elements', () => {
    const list = listOf(0, 1, 2);

    const removed = list.remove(1);

    expect(removed).toBe(1);
    expect(list.toArray()).toEqual([0, 2]);
  });

  it('remove the head returns the first value and leaves the tail', () => {
    const list = listOf(0, 2);

    const removed = list.remove(0);

    expect(removed).toBe(0);
    expect(list.toArray()).toEqual([2]);
  });

  it('remove the only element leaves an empty list', () => {
    const list = listOf(2);

    const removed = list.remove(0);

    expect(removed).toBe(2);
    expect(list.size()).toBe(0);
    expect(list.toArray()).toEqual([]);
  });

  it('remove on an empty list raises', () => {
    const list = new LinkedList<number>();

    expect(() => list.remove(0)).toThrow(new RangeError('index out of range: 0'));
  });

  it('contains finds an existing value', () => {
    const list = listOf(2);

    expect(list.contains(2)).toBe(true);
    expect(list.contains(99)).toBe(false);
  });

  it('indexOf returns the first occurrence', () => {
    const list = listOf(2);

    expect(list.indexOf(2)).toBe(0);
    expect(list.indexOf(99)).toBe(-1);
  });

  it('insertAt the head shifts existing elements', () => {
    const list = listOf(2);

    list.insertAt(0, 5);

    expect(list.toArray()).toEqual([5, 2]);
  });

  it('insertAt the middle shifts subsequent elements', () => {
    const list = listOf(5, 2);

    list.insertAt(1, 7);

    expect(list.toArray()).toEqual([5, 7, 2]);
  });

  it('insertAt size() is equivalent to append', () => {
    const list = listOf(5, 7, 2);

    list.insertAt(3, 9);

    expect(list.toArray()).toEqual([5, 7, 2, 9]);
  });

  it('insertAt an out-of-range index raises', () => {
    const list = listOf(5, 7, 2);

    expect(() => list.insertAt(10, 9)).toThrow(new RangeError('index out of range: 10'));
    expect(() => list.insertAt(-1, 9)).toThrow(new RangeError('index out of range: -1'));
  });
});
