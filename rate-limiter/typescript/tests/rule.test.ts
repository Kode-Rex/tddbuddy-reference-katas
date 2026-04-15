import { describe, it, expect } from 'vitest';
import { Rule } from '../src/Rule.js';
import { LimiterRuleInvalidError } from '../src/errors.js';

describe('Rule — construction and validation', () => {
  it('a rule with positive maxRequests and positive window is valid', () => {
    const rule = new Rule(3, 10_000);
    expect(rule.maxRequests).toBe(3);
    expect(rule.windowMs).toBe(10_000);
  });

  it('a rule rejects zero maxRequests with LimiterRuleInvalidError', () => {
    expect(() => new Rule(0, 10_000)).toThrowError(
      new LimiterRuleInvalidError('Max requests must be positive'),
    );
  });

  it('a rule rejects negative maxRequests with LimiterRuleInvalidError', () => {
    expect(() => new Rule(-1, 10_000)).toThrowError(
      new LimiterRuleInvalidError('Max requests must be positive'),
    );
  });

  it('a rule rejects zero window with LimiterRuleInvalidError', () => {
    expect(() => new Rule(3, 0)).toThrowError(
      new LimiterRuleInvalidError('Window must be positive'),
    );
  });

  it('a rule rejects negative window with LimiterRuleInvalidError', () => {
    expect(() => new Rule(3, -1)).toThrowError(
      new LimiterRuleInvalidError('Window must be positive'),
    );
  });
});
