import { describe, it, expect } from 'vitest';
import { BreakerBuilder } from './BreakerBuilder.js';
import { BreakerState } from '../src/BreakerState.js';
import { CircuitOpenError } from '../src/errors.js';

const THIRTY_SECONDS_MS = 30_000;

const fails = () => () => {
  throw new Error('boom');
};
const swallow = (fn: () => unknown) => {
  try { fn(); } catch { /* swallow */ }
};

function tripped() {
  const { breaker, clock } = new BreakerBuilder()
    .withThreshold(3)
    .withTimeoutMs(THIRTY_SECONDS_MS)
    .build();
  for (let i = 0; i < 3; i++) swallow(() => breaker.execute(fails()));
  return { breaker, clock };
}

describe('Breaker — Open state', () => {
  it('execute in Open throws CircuitOpenError without calling the operation', () => {
    const { breaker } = tripped();
    let called = false;

    expect(() =>
      breaker.execute(() => { called = true; return 'unused'; }),
    ).toThrowError(CircuitOpenError);
    expect(called).toBe(false);
  });

  it('the state remains Open after a fail-fast rejection', () => {
    const { breaker } = tripped();
    swallow(() => breaker.execute(() => 'unused'));
    expect(breaker.state).toBe(BreakerState.Open);
  });

  it('the CircuitOpenError message is "Circuit is open"', () => {
    const { breaker } = tripped();
    expect(() => breaker.execute(() => 'unused'))
      .toThrowError(new CircuitOpenError('Circuit is open'));
  });
});
