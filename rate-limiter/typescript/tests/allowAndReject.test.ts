import { describe, it, expect } from 'vitest';
import { LimiterBuilder } from './LimiterBuilder.js';

const TEN_SECONDS_MS = 10_000;

describe('Limiter — allow and reject', () => {
  it('the first request for a key is allowed', () => {
    const { limiter } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();

    expect(limiter.request('alice')).toEqual({ kind: 'allowed' });
  });

  it('requests up to the limit within the window are all allowed', () => {
    const { limiter } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();

    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
  });

  it('each Allowed decision carries no retryAfter', () => {
    const { limiter } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();

    const decision = limiter.request('alice');
    expect(decision).toEqual({ kind: 'allowed' });
    expect('retryAfterMs' in decision).toBe(false);
  });

  it('the request past the limit within the window is rejected', () => {
    const { limiter } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    expect(limiter.request('alice').kind).toBe('rejected');
  });

  it('a rejection reports retryAfter as the remaining window duration', () => {
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    clock.advanceMs(3_000);
    const decision = limiter.request('alice');

    expect(decision).toEqual({ kind: 'rejected', retryAfterMs: 7_000 });
  });

  it('a rejected request does not count against the window', () => {
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    clock.advanceMs(TEN_SECONDS_MS);
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('rejected');
  });

  it('repeated rejections report a decreasing retryAfter as the clock advances', () => {
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    clock.advanceMs(2_000);
    const first = limiter.request('alice');

    clock.advanceMs(3_000);
    const second = limiter.request('alice');

    expect(first).toEqual({ kind: 'rejected', retryAfterMs: 8_000 });
    expect(second).toEqual({ kind: 'rejected', retryAfterMs: 5_000 });
  });
});
