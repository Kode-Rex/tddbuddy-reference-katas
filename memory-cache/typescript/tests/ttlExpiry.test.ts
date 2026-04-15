import { describe, it, expect } from 'vitest';
import { CacheBuilder } from './CacheBuilder.js';

const ONE_MINUTE_MS = 60_000;

describe('Cache — TTL expiry', () => {
  it('a get after ttl has elapsed returns null', () => {
    const { cache, clock } = new CacheBuilder<string>().withTtlMs(ONE_MINUTE_MS).build();
    cache.put('alpha', 'one');

    clock.advanceMs(ONE_MINUTE_MS);

    expect(cache.get('alpha')).toBeNull();
  });

  it('contains returns false once ttl has elapsed', () => {
    const { cache, clock } = new CacheBuilder<string>().withTtlMs(ONE_MINUTE_MS).build();
    cache.put('alpha', 'one');

    clock.advanceMs(ONE_MINUTE_MS);

    expect(cache.contains('alpha')).toBe(false);
  });

  it('an expired entry is not counted in size after eviction sweep', () => {
    const { cache, clock } = new CacheBuilder<string>().withTtlMs(ONE_MINUTE_MS).build();
    cache.put('alpha', 'one');
    cache.put('beta', 'two');

    clock.advanceMs(ONE_MINUTE_MS);
    cache.evictExpired();

    expect(cache.size).toBe(0);
  });

  it('explicit evictExpired removes all expired entries', () => {
    const { cache, clock } = new CacheBuilder<string>().withTtlMs(ONE_MINUTE_MS).build();
    cache.put('alpha', 'one');
    cache.put('beta', 'two');
    cache.put('gamma', 'three');

    clock.advanceMs(ONE_MINUTE_MS);
    cache.evictExpired();

    expect(cache.contains('alpha')).toBe(false);
    expect(cache.contains('beta')).toBe(false);
    expect(cache.contains('gamma')).toBe(false);
  });

  it('explicit evictExpired leaves live entries intact', () => {
    const { cache, clock } = new CacheBuilder<string>().withTtlMs(ONE_MINUTE_MS).build();
    cache.put('old', 'stale');
    clock.advanceMs(30_000);
    cache.put('fresh', 'alive');

    clock.advanceMs(30_000);
    cache.evictExpired();

    expect(cache.contains('old')).toBe(false);
    expect(cache.contains('fresh')).toBe(true);
    expect(cache.get('fresh')).toBe('alive');
  });

  it('ttl is measured from insertion time not from last access', () => {
    const { cache, clock } = new CacheBuilder<string>().withTtlMs(ONE_MINUTE_MS).build();
    cache.put('alpha', 'one');

    clock.advanceMs(30_000);
    expect(cache.get('alpha')).toBe('one');

    clock.advanceMs(30_000);
    expect(cache.get('alpha')).toBeNull();
  });
});
