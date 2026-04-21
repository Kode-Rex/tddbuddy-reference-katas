# Multi-Threaded Santa — C# Walkthrough

This is a **concurrency kata** — the hardest category in the repo. It ships in one commit because the design was understood before coding, but the walkthrough explains the layered approach that makes concurrent code testable.

## The Core Insight: Start Sequential

The single most important design decision is **not** starting with threads. The `Pipeline.ProcessSequentiallyAsync` method runs all four stages on one thread, proving that presents flow correctly through Make -> Wrap -> Load -> Deliver before any concurrency enters the picture.

Three sequential pipeline tests lock down correctness: single present, multiple presents, order preservation. These tests are simple, fast, and deterministic. If the sequential pipeline is wrong, no amount of threading will fix it.

## `BoundedQueue<T>` — `Channel<T>` Does the Heavy Lifting

.NET's `Channel<T>` is purpose-built for producer-consumer pipelines. `BoundedChannelOptions` gives us:
- **Max capacity** — producers block when full (backpressure)
- **Multi-writer, multi-reader safety** — no manual locking needed
- **Completion signaling** — `Complete()` tells consumers to drain and stop

The `BoundedQueue<T>` wrapper exposes this cleanly: `EnqueueAsync`, `DequeueAsync`, `TryEnqueue` (non-blocking, used in tests), `Complete`, and `ReadAllAsync` for async enumeration.

The queue tests use `TryEnqueue` to verify capacity limits without blocking — no timeouts, no flakiness.

## The Sleigh Lock — `SemaphoreSlim`

The kata's trickiest constraint: **no presents can be loaded while the sleigh is delivering**. A `SemaphoreSlim(1, 1)` acts as a mutex shared between the Load and Deliver stages. Both acquire it before operating; only one proceeds at a time.

This means loading workers sometimes wait for a delivery to finish, and the delivery worker sometimes waits for a load to finish. The semaphore handles the fairness — no starvation, no deadlock (both stages release promptly).

## Concurrent Pipeline — Cascading Completions

`ProcessConcurrentlyAsync` launches workers per stage:
1. **Feed queue** distributes raw presents to maker workers
2. Makers complete -> `madeQueue.Complete()` signals wrappers to drain
3. Wrappers complete -> `wrappedQueue.Complete()` signals loaders
4. Loaders complete -> `loadedQueue.Complete()` signals delivery
5. Delivery drains and returns

Each stage is a loop over `ReadAllAsync` — when the input queue completes and empties, the loop exits. `Task.WhenAll` waits for all workers in a stage, then calls `Complete()` on the output queue. This cascade guarantees no items are lost.

## Why No Timing Assertions

Concurrency tests assert on **outcomes**, never on timing:
- All presents reach `Delivered` state
- The delivered count matches the input count
- All IDs are accounted for

No `Thread.Sleep`, no "this should take less than X ms." The tests run with zero simulated processing delay — concurrency correctness is structural, not temporal.

## Cost Calculation

`CostCalculator.CalculateCookies(elfCount, elapsed)` is a pure function: `elves * seconds = cookies`. `Pipeline.TotalElves(make, wrap, load)` adds the implicit delivery worker. The cost tests verify the arithmetic in isolation — they don't measure real elapsed time.

## Scenario Map

The nineteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live across four test files:
- `PresentTests.cs` — scenarios 1-5 (present lifecycle)
- `BoundedQueueTests.cs` — scenarios 6-9 (queue behavior)
- `SequentialPipelineTests.cs` — scenarios 10-12 (sequential pipeline)
- `ConcurrentPipelineTests.cs` — scenarios 13-16 (concurrent pipeline)
- `CostCalculatorTests.cs` — scenarios 17-19 (cost calculation)

## How to Run

```bash
cd multi-threaded-santa/csharp
dotnet test
```
