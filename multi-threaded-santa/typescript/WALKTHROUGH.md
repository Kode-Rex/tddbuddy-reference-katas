# Multi-Threaded Santa — TypeScript Walkthrough

Same design as C#, with one fundamental difference: **Node.js is single-threaded.**

## The Elephant in the Room: No Threads

JavaScript's event loop executes one piece of code at a time. There are no OS threads racing against each other, no data races, no need for `Mutex` or `lock`. So why is this kata still interesting in TypeScript?

Because the **async pipeline pattern** is real. `Promise.all` launches multiple workers that interleave at each `await` point. When worker A awaits a queue enqueue, worker B gets a turn. The concurrency is cooperative, not preemptive — but the pipeline architecture, backpressure, and completion cascading are identical to the threaded versions.

This matters in production Node.js: API servers process concurrent requests through middleware pipelines, stream processors chain stages with backpressure, and worker pools dispatch jobs. The patterns are the same; the scheduling model differs.

## BoundedQueue: Promise-Based Backpressure

The C# version uses `Channel<T>`. The TypeScript version hand-rolls the same semantics with an array, waiter queues (arrays of `resolve` callbacks), and a `completed` flag.

When `enqueue` finds the queue at capacity, it pushes a `resolve` callback onto `spaceWaiters` and awaits the resulting Promise. When `dequeue` removes an item, it shifts a waiter off `spaceWaiters` and resolves it — unblocking the producer.

The mirror image handles consumers waiting for items via `itemWaiters`.

`complete()` resolves all waiters, letting them check the `completed` flag and exit.

## Sleigh Lock: Async Mutex

The sleigh lock is a simple boolean + waiter array. `acquireSleigh` spins on a Promise loop until the lock is free. Since Node.js is single-threaded, there's no race between checking `sleighLocked` and setting it — the check-and-set is atomic within one microtask.

This would **not** be safe in a truly multi-threaded environment. The walkthrough for C# and Python explains why `SemaphoreSlim` and `threading.Lock` are necessary there.

## Pipeline Cascade

The concurrent pipeline follows the same structure as C#:
1. Feed queue distributes presents to maker workers
2. `Promise.all(makerWorkers)` completes -> `madeQueue.complete()`
3. Wrappers drain and complete -> `wrappedQueue.complete()`
4. Loaders drain and complete -> `loadedQueue.complete()`
5. Delivery drains and returns

Each stage is a `for await...of` loop over `readAll()`, which yields items from the async generator until the queue is completed and empty.

## What This Teaches About TypeScript Concurrency

The tests are identical in structure to C# and Python. The outcomes are the same: all presents delivered, correct counts, cost formula. The difference is purely in the scheduling model.

A reader comparing the three implementations learns:
- C# has real threads with real data races — needs `Channel<T>` and `SemaphoreSlim`
- Python has real threads with the GIL — needs `queue.Queue` and `threading.Lock`
- TypeScript has cooperative async — needs Promise-based queues and an async mutex

Same pipeline, same tests, three concurrency models.

## Scenario Map

Nineteen scenarios across five test files, matching [`../SCENARIOS.md`](../SCENARIOS.md) exactly.

## How to Run

```bash
cd multi-threaded-santa/typescript
npm install
npm test
```
