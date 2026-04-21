import { BoundedQueue } from './BoundedQueue.js';
import { Present } from './Present.js';

/**
 * Four-stage present pipeline: Make -> Wrap -> Load -> Deliver.
 *
 * In Node.js, "concurrency" means interleaved async work on the event loop,
 * not OS threads. Multiple workers per stage are concurrent Promises that
 * yield at each await point, allowing other workers to progress.
 *
 * The sleigh lock is an async mutex — only one load or deliver operation
 * proceeds at a time. Other operations await their turn.
 */
export class Pipeline {
  readonly madeQueue: BoundedQueue<Present>;
  readonly wrappedQueue: BoundedQueue<Present>;
  readonly loadedQueue: BoundedQueue<Present>;
  private readonly _delivered: Present[] = [];
  private sleighLocked = false;
  private sleighWaiters: Array<() => void> = [];

  get delivered(): readonly Present[] {
    return [...this._delivered];
  }

  constructor(
    madeCapacity = 1000,
    wrappedCapacity = 2000,
    loadedCapacity = 5000,
  ) {
    this.madeQueue = new BoundedQueue<Present>(madeCapacity);
    this.wrappedQueue = new BoundedQueue<Present>(wrappedCapacity);
    this.loadedQueue = new BoundedQueue<Present>(loadedCapacity);
  }

  /**
   * Process all presents sequentially. No concurrency.
   */
  async processSequentially(presents: readonly Present[]): Promise<void> {
    for (const p of presents) {
      p.make();
      await this.madeQueue.enqueue(p);
    }
    this.madeQueue.complete();

    for await (const p of this.madeQueue.readAll()) {
      p.wrap();
      await this.wrappedQueue.enqueue(p);
    }
    this.wrappedQueue.complete();

    for await (const p of this.wrappedQueue.readAll()) {
      p.load();
      await this.loadedQueue.enqueue(p);
    }
    this.loadedQueue.complete();

    for await (const p of this.loadedQueue.readAll()) {
      p.deliver();
      this._delivered.push(p);
    }
  }

  /**
   * Process presents concurrently with configurable worker counts.
   * Delivery always uses one worker (sleigh constraint).
   */
  async processConcurrently(
    presents: readonly Present[],
    makeWorkers = 1,
    wrapWorkers = 1,
    loadWorkers = 1,
  ): Promise<void> {
    // Feed queue distributes presents to makers
    const feedQueue = new BoundedQueue<Present>(
      presents.length > 0 ? presents.length : 1,
    );
    for (const p of presents) {
      await feedQueue.enqueue(p);
    }
    feedQueue.complete();

    // Stage 1: Make
    const makersDone = this.launchWorkers(makeWorkers, async () => {
      for await (const p of feedQueue.readAll()) {
        p.make();
        await this.madeQueue.enqueue(p);
      }
    });

    // Stage 2: Wrap
    const wrappersDone = this.launchWorkers(wrapWorkers, async () => {
      for await (const p of this.madeQueue.readAll()) {
        p.wrap();
        await this.wrappedQueue.enqueue(p);
      }
    });

    // Stage 3: Load (acquires sleigh lock)
    const loadersDone = this.launchWorkers(loadWorkers, async () => {
      for await (const p of this.wrappedQueue.readAll()) {
        await this.acquireSleigh();
        try {
          p.load();
          await this.loadedQueue.enqueue(p);
        } finally {
          this.releaseSleigh();
        }
      }
    });

    // Stage 4: Deliver (single worker, acquires sleigh lock)
    const deliveryDone = (async () => {
      for await (const p of this.loadedQueue.readAll()) {
        await this.acquireSleigh();
        try {
          p.deliver();
          this._delivered.push(p);
        } finally {
          this.releaseSleigh();
        }
      }
    })();

    // Cascade completions
    await makersDone;
    this.madeQueue.complete();

    await wrappersDone;
    this.wrappedQueue.complete();

    await loadersDone;
    this.loadedQueue.complete();

    await deliveryDone;
  }

  static totalElves(
    makeWorkers: number,
    wrapWorkers: number,
    loadWorkers: number,
  ): number {
    return makeWorkers + wrapWorkers + loadWorkers + 1; // +1 for delivery
  }

  private async acquireSleigh(): Promise<void> {
    while (this.sleighLocked) {
      await new Promise<void>((resolve) => this.sleighWaiters.push(resolve));
    }
    this.sleighLocked = true;
  }

  private releaseSleigh(): void {
    this.sleighLocked = false;
    const waiter = this.sleighWaiters.shift();
    if (waiter) waiter();
  }

  private launchWorkers(count: number, work: () => Promise<void>): Promise<void[]> {
    return Promise.all(Array.from({ length: count }, () => work()));
  }
}
