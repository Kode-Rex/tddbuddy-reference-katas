import type { Clock } from './Clock.js';
import { BreakerState } from './BreakerState.js';
import {
  BreakerThresholdInvalidError,
  BreakerTimeoutInvalidError,
  CircuitOpenError,
} from './errors.js';

export const DEFAULT_FAILURE_THRESHOLD = 5;
export const DEFAULT_RESET_TIMEOUT_MS = 30_000;

export class Breaker {
  private readonly failureThreshold: number;
  private readonly resetTimeoutMs: number;
  private readonly clock: Clock;

  private _state: BreakerState = BreakerState.Closed;
  private consecutiveFailures = 0;
  private openedAtMs = 0;

  constructor(failureThreshold: number, resetTimeoutMs: number, clock: Clock) {
    if (failureThreshold <= 0) {
      throw new BreakerThresholdInvalidError('Failure threshold must be positive');
    }
    if (resetTimeoutMs <= 0) {
      throw new BreakerTimeoutInvalidError('Reset timeout must be positive');
    }
    this.failureThreshold = failureThreshold;
    this.resetTimeoutMs = resetTimeoutMs;
    this.clock = clock;
  }

  get state(): BreakerState {
    return this._state;
  }

  execute<T>(operation: () => T): T {
    if (this._state === BreakerState.Open) {
      const elapsed = this.clock.now().getTime() - this.openedAtMs;
      if (elapsed >= this.resetTimeoutMs) {
        this._state = BreakerState.HalfOpen;
      } else {
        throw new CircuitOpenError('Circuit is open');
      }
    }

    try {
      const result = operation();
      this.onSuccess();
      return result;
    } catch (err) {
      this.onFailure();
      throw err;
    }
  }

  private onSuccess(): void {
    this.consecutiveFailures = 0;
    this._state = BreakerState.Closed;
  }

  private onFailure(): void {
    if (this._state === BreakerState.HalfOpen) {
      this.trip();
      return;
    }
    this.consecutiveFailures += 1;
    if (this.consecutiveFailures >= this.failureThreshold) {
      this.trip();
    }
  }

  private trip(): void {
    this._state = BreakerState.Open;
    this.openedAtMs = this.clock.now().getTime();
  }
}
