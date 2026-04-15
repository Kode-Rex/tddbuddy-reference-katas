import {
  Breaker,
  DEFAULT_FAILURE_THRESHOLD,
  DEFAULT_RESET_TIMEOUT_MS,
} from '../src/Breaker.js';
import { FixedClock } from './FixedClock.js';

export class BreakerBuilder {
  private threshold = DEFAULT_FAILURE_THRESHOLD;
  private timeoutMs = DEFAULT_RESET_TIMEOUT_MS;
  private start = new Date(Date.UTC(2026, 0, 1));
  private clock: FixedClock | null = null;

  withThreshold(threshold: number): this {
    this.threshold = threshold;
    return this;
  }

  withTimeoutMs(timeoutMs: number): this {
    this.timeoutMs = timeoutMs;
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

  build(): { breaker: Breaker; clock: FixedClock } {
    const clock = this.clock ?? new FixedClock(this.start);
    const breaker = new Breaker(this.threshold, this.timeoutMs, clock);
    return { breaker, clock };
  }
}
