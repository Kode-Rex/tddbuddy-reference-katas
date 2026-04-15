import { LimiterRuleInvalidError } from './errors.js';

export class Rule {
  readonly maxRequests: number;
  readonly windowMs: number;

  constructor(maxRequests: number, windowMs: number) {
    if (maxRequests <= 0) {
      throw new LimiterRuleInvalidError('Max requests must be positive');
    }
    if (windowMs <= 0) {
      throw new LimiterRuleInvalidError('Window must be positive');
    }
    this.maxRequests = maxRequests;
    this.windowMs = windowMs;
  }
}
