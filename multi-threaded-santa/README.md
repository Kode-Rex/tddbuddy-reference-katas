# Multi-Threaded Santa

Concurrency kata: build a four-stage present-delivery pipeline with bounded queues, configurable worker counts, a single-delivery sleigh constraint, and elf-pool cost accounting.

## What this kata teaches

- **Concurrent Pipeline** — four stages (Make, Wrap, Load, Deliver) connected by bounded queues, each stage running its own workers.
- **Bounded Queues** — queues with maximum capacity that block producers when full and consumers when empty. The fundamental backpressure mechanism.
- **Single-Resource Constraint** — the sleigh can only be used by one delivery worker, and loading must pause while delivering. A classic mutex/exclusion problem.
- **Elf Pool** — a shared worker pool that stages draw from. Workers are launched, not terminated — only reassigned.
- **Cost Calculation** — `elves x time = cookies`. The domain incentivizes minimizing both worker count and elapsed time.
- **Deterministic Testing of Concurrent Code** — start sequential (no threads), prove correctness, then layer concurrency on top. Never assert on timing.

## The "start sequential" approach

The hint in the kata spec is the key teaching point: build the pipeline as a single-threaded sequential processor first. All four stages run in order on a single thread. This proves the domain logic (present lifecycle, queue capacity, cost formula) before any concurrency enters the picture.

Only after the sequential pipeline is fully tested do you add:
1. Bounded queues with blocking semantics
2. Multi-worker stages pulling from input queues
3. The sleigh exclusion constraint
4. The elf pool that dispatches workers

This layered approach produces tests that are deterministic and fast — no timing assertions, no sleep-based synchronization, no flaky CI runs.

## Language-specific concurrency models

Each language uses its idiomatic concurrency primitives:

- **C#** — `Channel<T>` for bounded queues, `Task` for workers, `SemaphoreSlim` for sleigh exclusion, `CancellationToken` for shutdown.
- **TypeScript** — Node.js is single-threaded; "concurrency" is modeled as an async pipeline with `Promise`-based stages and a bounded async queue using `setTimeout`/`setImmediate` for backpressure. The walkthrough documents this distinction prominently.
- **Python** — `threading.Thread` for workers, `queue.Queue` (already supports bounded capacity), `threading.Event` for coordination, `threading.Lock` for sleigh exclusion.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
