import { describe, it, expect } from 'vitest';
import { BreakerBuilder } from './BreakerBuilder.js';
import { BreakerState } from '../src/BreakerState.js';
import { CircuitOpenError } from '../src/errors.js';

const THIRTY_SECONDS_MS = 30_000;

const succeeds = <T>(value: T) => () => value;
const fails = () => () => {
  throw new Error('boom');
};
const swallow = (fn: () => unknown) => {
  try { fn(); } catch { /* swallow */ }
};

describe('Breaker — end-to-end cycles', () => {
  it('Closed → Open → HalfOpen → Closed round-trip from the kata brief example', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();

    expect(breaker.execute(succeeds('ok'))).toBe('ok');
    swallow(() => breaker.execute(fails()));
    swallow(() => breaker.execute(fails()));
    swallow(() => breaker.execute(fails()));
    expect(breaker.state).toBe(BreakerState.Open);

    expect(() => breaker.execute(succeeds('ignored'))).toThrowError(CircuitOpenError);

    clock.advanceMs(THIRTY_SECONDS_MS);
    expect(breaker.execute(succeeds('ok'))).toBe('ok');
    expect(breaker.state).toBe(BreakerState.Closed);
  });

  it('Closed → Open → HalfOpen → Open cycle when the probe fails', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();
    for (let i = 0; i < 3; i++) swallow(() => breaker.execute(fails()));
    clock.advanceMs(THIRTY_SECONDS_MS);

    expect(() => breaker.execute(fails())).toThrowError('boom');
    expect(breaker.state).toBe(BreakerState.Open);
  });
});
