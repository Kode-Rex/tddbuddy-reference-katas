import { describe, it, expect } from 'vitest';
import { BreakerBuilder } from './BreakerBuilder.js';
import { BreakerState } from '../src/BreakerState.js';
import {
  BreakerThresholdInvalidError,
  BreakerTimeoutInvalidError,
} from '../src/errors.js';

describe('Breaker — construction', () => {
  it('a new breaker is in the Closed state', () => {
    const { breaker } = new BreakerBuilder().build();
    expect(breaker.state).toBe(BreakerState.Closed);
  });

  it('breaker rejects non-positive failure threshold with BreakerThresholdInvalidError', () => {
    expect(() => new BreakerBuilder().withThreshold(0).build())
      .toThrowError(new BreakerThresholdInvalidError('Failure threshold must be positive'));
  });

  it('breaker rejects non-positive reset timeout with BreakerTimeoutInvalidError', () => {
    expect(() => new BreakerBuilder().withTimeoutMs(0).build())
      .toThrowError(new BreakerTimeoutInvalidError('Reset timeout must be positive'));
  });
});
