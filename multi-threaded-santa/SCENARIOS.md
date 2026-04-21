# Multi-Threaded Santa — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Present** | The work item that flows through all four pipeline stages |
| **Stage** | One step in the pipeline: Make, Wrap, Load, or Deliver |
| **Bounded Queue** | A thread-safe queue with a maximum capacity; producers block when full, consumers block when empty |
| **Pipeline** | The four stages connected by bounded queues, processing presents from start to finish |
| **Elf** | A worker (thread) assigned to a stage; drawn from the shared Elf Pool |
| **Elf Pool** | The shared pool of workers that stages draw from |
| **Sleigh** | The single delivery resource; only one delivery at a time, and loading pauses during delivery |
| **Cookies** | The cost unit: `number of elves x elapsed seconds` |

## Domain Rules

- A present must pass through all four stages in order: Make -> Wrap -> Load -> Deliver
- Each stage has an input queue and an output queue (except Make's input and Deliver's output)
- Queues have a configurable maximum capacity
- When a queue is full, the producing stage blocks until space is available
- When a queue is empty, the consuming stage blocks until an item is available
- Only **one** delivery worker operates the sleigh at a time
- **No presents can be loaded while the sleigh is delivering** (sleigh exclusion)
- Workers are drawn from the Elf Pool and cannot be terminated, only reassigned
- Cost = number of elves x total elapsed seconds = cookies
- Processing time per stage is injectable (defaults to zero in tests for determinism)

## Test Scenarios

### Present Lifecycle

1. **A new present starts in the unmade state**
2. **Making a present transitions it to the made state**
3. **Wrapping a made present transitions it to the wrapped state**
4. **Loading a wrapped present transitions it to the loaded state**
5. **Delivering a loaded present transitions it to the delivered state**

### Bounded Queue

6. **Enqueuing an item makes it available for dequeue**
7. **Dequeuing returns items in FIFO order**
8. **A queue reports its current count**
9. **A full queue rejects further enqueues until space is available**

### Sequential Pipeline

10. **A single present flows through all four stages**
11. **Multiple presents all complete the full pipeline**
12. **Presents emerge from the pipeline in order**

### Concurrent Pipeline

13. **Multiple workers can process a stage concurrently**
14. **The sleigh constraint allows only one delivery at a time**
15. **Loading pauses while the sleigh is delivering**
16. **All presents are delivered when the pipeline completes**

### Cost Calculation

17. **Cost is zero when no elves are used**
18. **Cost equals elves multiplied by elapsed seconds**
19. **More elves with shorter time can cost the same as fewer elves with longer time**
