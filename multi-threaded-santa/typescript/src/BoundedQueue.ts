/**
 * Async bounded queue with backpressure.
 *
 * In Node.js (single-threaded), "blocking" means awaiting a Promise
 * that resolves when space/items become available. The event loop
 * is never actually blocked — other async work proceeds.
 */
export class BoundedQueue<T> {
  readonly capacity: number;
  private readonly items: T[] = [];
  private completed = false;

  // Waiters for space (producers) and items (consumers)
  private spaceWaiters: Array<() => void> = [];
  private itemWaiters: Array<() => void> = [];

  constructor(capacity: number) {
    this.capacity = capacity;
  }

  get count(): number {
    return this.items.length;
  }

  /**
   * Enqueue an item. Resolves when the item is enqueued.
   * Awaits if the queue is at capacity.
   * Returns false if the queue is completed.
   */
  async enqueue(item: T): Promise<boolean> {
    if (this.completed) return false;

    while (this.items.length >= this.capacity) {
      if (this.completed) return false;
      await new Promise<void>((resolve) => this.spaceWaiters.push(resolve));
    }

    if (this.completed) return false;

    this.items.push(item);
    const waiter = this.itemWaiters.shift();
    if (waiter) waiter();
    return true;
  }

  /**
   * Try to enqueue without waiting. Returns false if full or completed.
   */
  tryEnqueue(item: T): boolean {
    if (this.completed || this.items.length >= this.capacity) return false;
    this.items.push(item);
    const waiter = this.itemWaiters.shift();
    if (waiter) waiter();
    return true;
  }

  /**
   * Dequeue an item. Awaits if the queue is empty.
   * Returns { success: false } when completed and empty.
   */
  async dequeue(): Promise<{ success: true; item: T } | { success: false }> {
    while (this.items.length === 0) {
      if (this.completed) return { success: false };
      await new Promise<void>((resolve) => this.itemWaiters.push(resolve));
      if (this.completed && this.items.length === 0) return { success: false };
    }

    const item = this.items.shift()!;
    const waiter = this.spaceWaiters.shift();
    if (waiter) waiter();
    return { success: true, item };
  }

  /**
   * Signal that no more items will be enqueued.
   */
  complete(): void {
    this.completed = true;
    for (const w of this.spaceWaiters) w();
    this.spaceWaiters = [];
    for (const w of this.itemWaiters) w();
    this.itemWaiters = [];
  }

  /**
   * Async generator that yields all items until completion.
   */
  async *readAll(): AsyncGenerator<T> {
    while (true) {
      const result = await this.dequeue();
      if (!result.success) return;
      yield result.item;
    }
  }
}
