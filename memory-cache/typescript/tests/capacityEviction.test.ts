import { describe, it, expect } from 'vitest';
import { CacheBuilder } from './CacheBuilder.js';

describe('Cache — capacity and LRU eviction', () => {
  it('filling the cache to capacity does not evict', () => {
    const { cache } = new CacheBuilder<string>().withCapacity(3).build();
    cache.put('a', '1');
    cache.put('b', '2');
    cache.put('c', '3');

    expect(cache.size).toBe(3);
    expect(cache.contains('a')).toBe(true);
    expect(cache.contains('b')).toBe(true);
    expect(cache.contains('c')).toBe(true);
  });

  it('putting a new key when at capacity evicts the least-recently-used key', () => {
    const { cache } = new CacheBuilder<string>().withCapacity(3).build();
    cache.put('a', '1');
    cache.put('b', '2');
    cache.put('c', '3');

    cache.put('d', '4');

    expect(cache.size).toBe(3);
    expect(cache.contains('a')).toBe(false);
    expect(cache.contains('d')).toBe(true);
  });

  it('getting a key refreshes recency so it is not the next evicted', () => {
    const { cache } = new CacheBuilder<string>().withCapacity(3).build();
    cache.put('a', '1');
    cache.put('b', '2');
    cache.put('c', '3');

    cache.get('a');
    cache.put('d', '4');

    expect(cache.contains('a')).toBe(true);
    expect(cache.contains('b')).toBe(false);
  });

  it('replacing an existing key refreshes recency so it is not the next evicted', () => {
    const { cache } = new CacheBuilder<string>().withCapacity(3).build();
    cache.put('a', '1');
    cache.put('b', '2');
    cache.put('c', '3');

    cache.put('a', '1-new');
    cache.put('d', '4');

    expect(cache.contains('a')).toBe(true);
    expect(cache.get('a')).toBe('1-new');
    expect(cache.contains('b')).toBe(false);
  });
});
