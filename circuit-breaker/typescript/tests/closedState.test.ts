import { describe, it, expect } from 'vitest';
import { BreakerBuilder } from './BreakerBuilder.js';
import { BreakerState } from '../src/BreakerState.js';

const succeeds = <T>(value: T) => () => value;
const fails = () => () => {
  throw new Error('boom');
};
const swallow = (fn: () => unknown) => {
  try { fn(); } catch { /* swallow */ }
};

describe('Breaker — Closed state', () => {
  it("execute in Closed returns the operation's result on success", () => {
    const { breaker } = new BreakerBuilder().withThreshold(3).build();
    expect(breaker.execute(succeeds('ok'))).toBe('ok');
    expect(breaker.state).toBe(BreakerState.Closed);
  });

  it("execute in Closed rethrows the operation's exception on failure", () => {
    const { breaker } = new BreakerBuilder().withThreshold(3).build();
    expect(() => breaker.execute(fails())).toThrowError('boom');
  });

  it('a single failure in Closed does not trip the breaker', () => {
    const { breaker } = new BreakerBuilder().withThreshold(3).build();
    swallow(() => breaker.execute(fails()));
    expect(breaker.state).toBe(BreakerState.Closed);
  });

  it('reaching the failure threshold in Closed transitions to Open', () => {
    const { breaker } = new BreakerBuilder().withThreshold(3).build();
    for (let i = 0; i < 3; i++) swallow(() => breaker.execute(fails()));
    expect(breaker.state).toBe(BreakerState.Open);
  });

  it('a success in Closed resets the consecutive-failure counter', () => {
    const { breaker } = new BreakerBuilder().withThreshold(3).build();
    swallow(() => breaker.execute(fails()));
    swallow(() => breaker.execute(fails()));
    breaker.execute(succeeds('ok'));
    swallow(() => breaker.execute(fails()));
    swallow(() => breaker.execute(fails()));
    expect(breaker.state).toBe(BreakerState.Closed);
  });

  it('consecutive failures below the threshold stay Closed even after many operations', () => {
    const { breaker } = new BreakerBuilder().withThreshold(3).build();
    for (let i = 0; i < 10; i++) {
      swallow(() => breaker.execute(fails()));
      breaker.execute(succeeds('ok'));
    }
    expect(breaker.state).toBe(BreakerState.Closed);
  });
});
