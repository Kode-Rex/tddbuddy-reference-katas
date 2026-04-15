import { describe, it, expect } from 'vitest';
import { LimiterBuilder } from './LimiterBuilder.js';

const TEN_SECONDS_MS = 10_000;

describe('Limiter — end-to-end worked example', () => {
  it('fixed-window cycle for alice and bob produces the documented sequence', () => {
    const start = new Date(Date.UTC(2026, 0, 1));
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .startingAt(start)
      .build();

    // t=0,1,2 — alice allowed
    expect(limiter.request('alice').kind).toBe('allowed');
    clock.advanceMs(1_000);
    expect(limiter.request('alice').kind).toBe('allowed');
    clock.advanceMs(1_000);
    expect(limiter.request('alice').kind).toBe('allowed');

    // t=3 — alice rejected with retryAfter=7s
    clock.advanceMs(1_000);
    expect(limiter.request('alice')).toEqual({ kind: 'rejected', retryAfterMs: 7_000 });

    // t=3 — bob allowed (independent)
    expect(limiter.request('bob').kind).toBe('allowed');

    // t=10 — alice's window has elapsed; fresh quota
    clock.advanceMs(7_000);
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('rejected');
  });
});
