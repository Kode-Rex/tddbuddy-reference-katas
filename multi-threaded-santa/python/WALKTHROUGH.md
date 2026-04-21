# Multi-Threaded Santa — Python Walkthrough

Same design as C# and TypeScript. Python uses real OS threads (`threading.Thread`) with the GIL, which gives an interesting middle ground between C#'s fully preemptive threads and TypeScript's cooperative async.

## Python's GIL and Why It Still Matters

Python's Global Interpreter Lock (GIL) means only one thread executes Python bytecode at a time. So why use threads at all?

Because the pipeline pattern is about **coordination**, not parallelism. Each stage blocks on I/O (queue reads/writes), and the GIL releases during blocking operations. The threads genuinely interleave — one thread blocks on an empty queue while another processes a present. For this kata's teaching purpose, the threading model is real and the concurrency concerns (shared state, locks, completion signaling) are genuine.

For CPU-bound parallelism, Python would need `multiprocessing`. But the kata's domain — stages connected by queues — maps naturally to `threading`.

## `queue.Queue` — Already Bounded

Python's standard library `queue.Queue` supports `maxsize` out of the box. No need for a custom bounded channel like C#'s `Channel<T>`. The `BoundedQueue` wrapper adds:
- A `complete()` method via `threading.Event` — signals consumers to stop after draining
- An `items()` iterator that loops `dequeue()` until completion
- A `try_enqueue` method for non-blocking capacity tests

The dequeue method uses a polling loop with a short timeout (0.1s) so it can check the completion flag between attempts. This is the standard Python pattern for interruptible blocking reads.

## Sleigh Lock — `threading.Lock`

The sleigh exclusion uses a simple `threading.Lock`. Both the load and deliver workers acquire it before operating. Python's `with` statement makes the acquire-release pattern clean and exception-safe.

Unlike TypeScript's async mutex (which is safe because Node is single-threaded), Python's lock is a real kernel-level synchronization primitive. Two threads genuinely contend for it, and the OS scheduler decides who wins.

## Completion Cascade

The concurrent pipeline follows the same cascade as C# and TypeScript:
1. Feed queue distributes presents to maker threads
2. All maker threads join -> `made_queue.complete()`
3. All wrapper threads join -> `wrapped_queue.complete()`
4. All loader threads join -> `loaded_queue.complete()`
5. Deliver thread joins

Each worker iterates `queue.items()` — a custom iterator that calls `dequeue()` until it gets `(False, None)`, indicating the queue is completed and empty.

## Daemon Threads

All worker threads are created as daemon threads. This ensures they don't keep the process alive if something goes wrong — the main thread's `.join()` calls are the orderly shutdown path, and daemon status is the safety net.

## Scenario Map

Nineteen scenarios across five test files, matching [`../SCENARIOS.md`](../SCENARIOS.md) exactly.

## How to Run

```bash
cd multi-threaded-santa/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
