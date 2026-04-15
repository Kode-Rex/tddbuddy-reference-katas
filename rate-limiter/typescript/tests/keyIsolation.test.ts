import { describe, it, expect } from 'vitest';
import { LimiterBuilder } from './LimiterBuilder.js';

const TEN_SECONDS_MS = 10_000;

describe('Limiter — keys are isolated', () => {
  it('two different keys have independent counts', () => {
    const { limiter } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();

    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');
    expect(limiter.request('alice').kind).toBe('allowed');

    expect(limiter.request('bob').kind).toBe('allowed');
    expect(limiter.request('bob').kind).toBe('allowed');
    expect(limiter.request('bob').kind).toBe('allowed');
  });

  it('a rejection on one key does not affect another key\'s decisions', () => {
    const { limiter } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');
    expect(limiter.request('alice').kind).toBe('rejected');

    expect(limiter.request('bob').kind).toBe('allowed');
  });

  it('each key\'s window starts from its own first request', () => {
    const { limiter, clock } = new LimiterBuilder()
      .withMaxRequests(3)
      .withWindowMs(TEN_SECONDS_MS)
      .build();
    limiter.request('alice');
    limiter.request('alice');
    limiter.request('alice');

    clock.advanceMs(5_000);
    limiter.request('bob');
    limiter.request('bob');
    limiter.request('bob');

    expect(limiter.request('alice')).toEqual({ kind: 'rejected', retryAfterMs: 5_000 });
    expect(limiter.request('bob')).toEqual({ kind: 'rejected', retryAfterMs: 10_000 });
  });
});
