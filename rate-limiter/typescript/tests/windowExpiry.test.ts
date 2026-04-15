import { describe, it, expect } from 'vitest';
import { LimiterBuilder } from './LimiterBuilder.js';

const TEN_SECONDS_MS = 10_000;

describe('Limiter — window expiry resets count', () => {
  it('a request exactly at the window boundary opens a fresh window and is allowed', () => {
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    clock.advanceMs(TEN_SECONDS_MS);
    expect(limiter.request('alice').kind).toBe('allowed');
  });

  it('a request after the window has fully elapsed opens a fresh window and is allowed', () => {
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    clock.advanceMs(15_000);
    expect(limiter.request('alice').kind).toBe('allowed');
  });

  it('after a window resets the full quota is available again', () => {
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    clock.advanceMs(TEN_SECONDS_MS);
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('rejected');
  });
});
