# Linked List — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

A **singly linked list** holds values in order. The list tracks a `head` node; each node carries a value and a reference to the next node (or `null`/`None`). The list must not delegate its storage to a built-in array or list type — nodes and pointers are the storage.

Indexing is **zero-based**. A negative index and an index at or above `size()` are out of bounds for `get`, `remove`, and `insertAt`; the single exception is `insertAt(size(), value)`, which is equivalent to `append(value)`.

Errors are raised as the language's idiomatic "out of range" exception (`ArgumentOutOfRangeException` in C#, `RangeError` in TypeScript, `IndexError` in Python). Error messages are identical across languages.

## API

- **`append(value)`** — add `value` to the end.
- **`prepend(value)`** — add `value` to the beginning.
- **`size()`** — number of elements.
- **`get(index)`** — value at `index`; error if out of bounds.
- **`remove(index)`** — remove and return the value at `index`; error if out of bounds (including any index on an empty list).
- **`insertAt(index, value)`** — insert `value` at `index`, shifting subsequent elements; `insertAt(size(), value)` is equivalent to `append(value)`; error if `index < 0` or `index > size()`.
- **`contains(value)`** — `true` iff the value appears in the list.
- **`indexOf(value)`** — index of the first occurrence, or `-1` if absent.
- **`toArray()`** — values in order, as an array/list (for test assertions).

## Test Scenarios

1. **A new list is empty** — `size()` is `0`; `toArray()` is `[]`.
2. **Appending to an empty list yields a single element** — `append(1)` gives `[1]`, `size()` is `1`.
3. **Appending preserves insertion order** — `append(1); append(2); append(3)` gives `[1, 2, 3]`.
4. **Prepending to an empty list yields a single element** — `prepend(1)` gives `[1]`.
5. **Prepending puts the value at the front** — `append(1); append(2); prepend(0)` gives `[0, 1, 2]`.
6. **Get returns the value at the given index** — on `[0, 1, 2]`, `get(0)` is `0` and `get(2)` is `2`.
7. **Get on an out-of-range index raises** — on `[0, 1, 2]`, `get(5)` raises "index out of range: 5".
8. **Get on a negative index raises** — on `[0, 1, 2]`, `get(-1)` raises "index out of range: -1".
9. **Remove returns the value and shifts subsequent elements** — on `[0, 1, 2]`, `remove(1)` returns `1` and leaves `[0, 2]`.
10. **Remove the head returns the first value and leaves the tail** — on `[0, 2]`, `remove(0)` returns `0` and leaves `[2]`.
11. **Remove the only element leaves an empty list** — on `[2]`, `remove(0)` returns `2` and leaves `[]`.
12. **Remove on an empty list raises** — `remove(0)` on `[]` raises "index out of range: 0".
13. **Contains finds an existing value** — on `[2]`, `contains(2)` is `true`; `contains(99)` is `false`.
14. **IndexOf returns the first occurrence** — on `[2]`, `indexOf(2)` is `0`; `indexOf(99)` is `-1`.
15. **InsertAt the head shifts existing elements** — on `[2]`, `insertAt(0, 5)` gives `[5, 2]`.
16. **InsertAt the middle shifts subsequent elements** — on `[5, 2]`, `insertAt(1, 7)` gives `[5, 7, 2]`.
17. **InsertAt size() is equivalent to append** — on `[5, 7, 2]`, `insertAt(3, 9)` gives `[5, 7, 2, 9]`.
18. **InsertAt an out-of-range index raises** — on `[5, 7, 2]`, `insertAt(10, 9)` raises "index out of range: 10"; `insertAt(-1, 9)` raises "index out of range: -1".
