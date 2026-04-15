import { Cache, DEFAULT_CAPACITY, DEFAULT_TTL_MS } from '../src/Cache.js';
import { FixedClock } from './FixedClock.js';

export class CacheBuilder<V> {
  private capacity = DEFAULT_CAPACITY;
  private ttlMs = DEFAULT_TTL_MS;
  private start = new Date(Date.UTC(2026, 0, 1));
  private clock: FixedClock | null = null;

  withCapacity(capacity: number): this {
    this.capacity = capacity;
    return this;
  }

  withTtlMs(ttlMs: number): this {
    this.ttlMs = ttlMs;
    return this;
  }

  startingAt(start: Date): this {
    this.start = start;
    return this;
  }

  withClock(clock: FixedClock): this {
    this.clock = clock;
    return this;
  }

  build(): { cache: Cache<V>; clock: FixedClock } {
    const clock = this.clock ?? new FixedClock(this.start);
    const cache = new Cache<V>(this.capacity, this.ttlMs, clock);
    return { cache, clock };
  }
}
