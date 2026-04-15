import { describe, it, expect } from 'vitest';
import { CacheBuilder } from './CacheBuilder.js';
import { CacheCapacityInvalidError, CacheTtlInvalidError } from '../src/errors.js';

describe('Cache — construction and put/get', () => {
  it('a new cache has size zero', () => {
    const { cache } = new CacheBuilder<string>().build();
    expect(cache.size).toBe(0);
  });

  it('a new cache contains no keys', () => {
    const { cache } = new CacheBuilder<string>().build();
    expect(cache.contains('anything')).toBe(false);
  });

  it('cache rejects non-positive capacity with CacheCapacityInvalidError', () => {
    expect(() => new CacheBuilder<string>().withCapacity(0).build())
      .toThrowError(new CacheCapacityInvalidError('Capacity must be positive'));
  });

  it('cache rejects non-positive ttl with CacheTtlInvalidError', () => {
    expect(() => new CacheBuilder<string>().withTtlMs(0).build())
      .toThrowError(new CacheTtlInvalidError('TTL must be positive'));
  });

  it('putting a key then getting it returns the stored value', () => {
    const { cache } = new CacheBuilder<string>().build();
    cache.put('alpha', 'one');
    expect(cache.get('alpha')).toBe('one');
  });

  it('getting a missing key returns null', () => {
    const { cache } = new CacheBuilder<string>().build();
    expect(cache.get('absent')).toBeNull();
  });

  it('put increases the size by one', () => {
    const { cache } = new CacheBuilder<string>().build();
    cache.put('alpha', 'one');
    expect(cache.size).toBe(1);
  });

  it('putting the same key twice replaces the value without growing the size', () => {
    const { cache } = new CacheBuilder<string>().build();
    cache.put('alpha', 'one');
    cache.put('alpha', 'two');
    expect(cache.size).toBe(1);
    expect(cache.get('alpha')).toBe('two');
  });

  it('contains returns true for a stored key', () => {
    const { cache } = new CacheBuilder<string>().build();
    cache.put('alpha', 'one');
    expect(cache.contains('alpha')).toBe(true);
  });

  it('contains returns false for a missing key', () => {
    const { cache } = new CacheBuilder<string>().build();
    expect(cache.contains('absent')).toBe(false);
  });
});
