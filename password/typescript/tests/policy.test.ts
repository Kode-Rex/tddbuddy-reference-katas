import { describe, it, expect } from 'vitest';
import { PolicyBuilder } from './policyBuilder.js';

describe('Policy', () => {
  it('with minLength 8 accepts an 8-character password', () => {
    const policy = new PolicyBuilder().minLength(8).build();
    expect(policy.validate('abcd1234').ok).toBe(true);
  });

  it('with minLength 8 rejects a 6-character password', () => {
    const policy = new PolicyBuilder().minLength(8).build();
    const result = policy.validate('abc123');
    expect(result.ok).toBe(false);
    expect(result.failures).toEqual(['minimum length']);
  });

  it('requiring a digit accepts a password with a digit', () => {
    const policy = new PolicyBuilder().requiresDigit().build();
    expect(policy.validate('password1').ok).toBe(true);
  });

  it('requiring a digit rejects a password with no digit', () => {
    const policy = new PolicyBuilder().requiresDigit().build();
    const result = policy.validate('password');
    expect(result.ok).toBe(false);
    expect(result.failures).toEqual(['requires digit']);
  });

  it('requiring a symbol accepts a password with a symbol', () => {
    const policy = new PolicyBuilder().requiresSymbol().build();
    expect(policy.validate('password!').ok).toBe(true);
  });

  it('requiring a symbol rejects a password with no symbol', () => {
    const policy = new PolicyBuilder().requiresSymbol().build();
    const result = policy.validate('password1');
    expect(result.ok).toBe(false);
    expect(result.failures).toEqual(['requires symbol']);
  });

  it('requiring uppercase rejects an all-lowercase password', () => {
    const policy = new PolicyBuilder().requiresUpper().build();
    const result = policy.validate('password1');
    expect(result.ok).toBe(false);
    expect(result.failures).toEqual(['requires uppercase']);
  });

  it('requiring lowercase rejects an all-uppercase password', () => {
    const policy = new PolicyBuilder().requiresLower().build();
    const result = policy.validate('PASSWORD1');
    expect(result.ok).toBe(false);
    expect(result.failures).toEqual(['requires lowercase']);
  });

  it('with multiple requirements reports every failed rule', () => {
    const policy = new PolicyBuilder()
      .minLength(8)
      .requiresDigit()
      .requiresSymbol()
      .requiresUpper()
      .requiresLower()
      .build();

    expect(policy.validate('Abcd123!').ok).toBe(true);

    const result = policy.validate('abc');
    expect(result.ok).toBe(false);
    expect(result.failures).toEqual([
      'minimum length',
      'requires digit',
      'requires symbol',
      'requires uppercase',
    ]);
  });

  it('PolicyBuilder default is minLength 8 with no character-class requirements', () => {
    const policy = new PolicyBuilder().build();
    expect(policy.validate('abcdefgh').ok).toBe(true);
    const result = policy.validate('abcdefg');
    expect(result.ok).toBe(false);
    expect(result.failures).toEqual(['minimum length']);
  });
});
