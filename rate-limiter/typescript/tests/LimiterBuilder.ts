import {
  Limiter,
  DEFAULT_MAX_REQUESTS,
  DEFAULT_WINDOW_MS,
} from '../src/Limiter.js';
import { Rule } from '../src/Rule.js';
import { FixedClock } from './FixedClock.js';

export class LimiterBuilder {
  private maxRequests = DEFAULT_MAX_REQUESTS;
  private windowMs = DEFAULT_WINDOW_MS;
  private start = new Date(Date.UTC(2026, 0, 1));
  private clock: FixedClock | null = null;

  withMaxRequests(maxRequests: number): this {
    this.maxRequests = maxRequests;
    return this;
  }

  withWindowMs(windowMs: number): this {
    this.windowMs = windowMs;
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

  build(): { limiter: Limiter; clock: FixedClock } {
    const clock = this.clock ?? new FixedClock(this.start);
    const limiter = new Limiter(new Rule(this.maxRequests, this.windowMs), clock);
    return { limiter, clock };
  }
}
