import type { Clock } from './Clock.js';
import { Rule } from './Rule.js';
import { allowed, rejected, type Decision } from './Decision.js';

export const DEFAULT_MAX_REQUESTS = 100;
export const DEFAULT_WINDOW_MS = 60_000;

interface WindowState {
  startMs: number;
  count: number;
}

export class Limiter {
  private readonly rule: Rule;
  private readonly clock: Clock;
  private readonly windows = new Map<string, WindowState>();

  constructor(rule: Rule, clock: Clock) {
    this.rule = rule;
    this.clock = clock;
  }

  request(key: string): Decision {
    const nowMs = this.clock.now().getTime();
    const state = this.windows.get(key);

    if (!state || nowMs >= state.startMs + this.rule.windowMs) {
      this.windows.set(key, { startMs: nowMs, count: 1 });
      return allowed();
    }

    if (state.count < this.rule.maxRequests) {
      state.count += 1;
      return allowed();
    }

    const retryAfterMs = state.startMs + this.rule.windowMs - nowMs;
    return rejected(retryAfterMs);
  }
}
