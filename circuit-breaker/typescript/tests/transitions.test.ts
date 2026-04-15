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

function tripToOpen(breaker: { execute: <T>(op: () => T) => T }) {
  for (let i = 0; i < 3; i++) swallow(() => breaker.execute(fails()));
}

describe('Breaker — Open → HalfOpen and back', () => {
  it('execute before the reset timeout elapses still fails fast', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();
    tripToOpen(breaker);

    clock.advanceMs(29_000);

    expect(() => breaker.execute(succeeds('ok'))).toThrowError(CircuitOpenError);
  });

  it('execute after the reset timeout elapses probes the operation in HalfOpen', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();
    tripToOpen(breaker);
    clock.advanceMs(THIRTY_SECONDS_MS);

    let probed = false;
    breaker.execute(() => { probed = true; return 'ok'; });

    expect(probed).toBe(true);
  });

  it('a successful probe transitions HalfOpen to Closed', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();
    tripToOpen(breaker);
    clock.advanceMs(THIRTY_SECONDS_MS);

    breaker.execute(succeeds('ok'));

    expect(breaker.state).toBe(BreakerState.Closed);
  });

  it('a successful probe resets the consecutive-failure counter', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();
    tripToOpen(breaker);
    clock.advanceMs(THIRTY_SECONDS_MS);
    breaker.execute(succeeds('ok'));

    swallow(() => breaker.execute(fails()));
    swallow(() => breaker.execute(fails()));

    expect(breaker.state).toBe(BreakerState.Closed);
  });

  it('a failed probe transitions HalfOpen back to Open and rethrows', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();
    tripToOpen(breaker);
    clock.advanceMs(THIRTY_SECONDS_MS);

    expect(() => breaker.execute(fails())).toThrowError('boom');
    expect(breaker.state).toBe(BreakerState.Open);
  });

  it('a failed probe restarts the reset timeout', () => {
    const { breaker, clock } = new BreakerBuilder()
      .withThreshold(3).withTimeoutMs(THIRTY_SECONDS_MS).build();
    tripToOpen(breaker);
    clock.advanceMs(THIRTY_SECONDS_MS);
    swallow(() => breaker.execute(fails()));

    clock.advanceMs(29_000);
    expect(() => breaker.execute(succeeds('ok'))).toThrowError(CircuitOpenError);
  });
});
