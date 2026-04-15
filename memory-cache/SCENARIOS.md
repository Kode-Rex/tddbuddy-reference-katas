# Memory Cache — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Cache** | The aggregate root (`Cache<V>`). Owns entries keyed by string, enforces capacity and TTL |
| **Entry** | A value stored under a key with the clock time of insertion |
| **Capacity** | The maximum number of entries the cache holds; a `put` that would exceed it evicts the least-recently-used key |
| **TTL** | Time-to-live: entries are considered expired once `now - insertedAt >= ttl` |
| **Recency** | Order of last access; `get` and `put` on an existing key both refresh recency. Expired lookups do not |
| **Clock** | Collaborator returning "now" — injected so TTL tests control time without sleeping |
| **Explicit Sweep** | `evictExpired()` — separate method that drops all expired entries in one pass |

## Domain Rules

- A new cache is empty; `size` is `0`, `contains(key)` is `false` for every key.
- **Capacity** must be **strictly positive**; zero and negative capacities are rejected.
- **TTL** must be **strictly positive**; zero and negative durations are rejected.
- `put(key, value)` stores an entry stamped with `clock.now()`.
- `put` on an existing key **replaces** the value and refreshes both its insertion time and recency.
- `put` when `size == capacity` and `key` is new **evicts the least-recently-used** entry before inserting.
- `get(key)` returns the stored value, or `null` / `None` if the key is absent or its entry has expired. An expired `get` also **removes** the stale entry and does **not** refresh recency.
- `get` on a live entry **refreshes recency** (but not insertion time — TTL is measured from `put`, not last access).
- `contains(key)` returns `true` only for a key whose entry is present and not expired; a `contains` call does not refresh recency.
- `evictExpired()` removes every entry where `now - insertedAt >= ttl` in a single pass.
- Rejected operations (invalid capacity, invalid TTL) **throw domain exceptions** with byte-identical messages across languages.

## Named Constants

- `DefaultCapacity = 100`
- `DefaultTtl = 60 seconds` (`TimeSpan.FromSeconds(60)` / `60_000 ms` / `timedelta(seconds=60)`)

These defaults match the kata brief; builders override them per scenario.

## Test Scenarios

### Construction

1. **A new cache has size zero**
2. **A new cache contains no keys**
3. **Cache rejects non-positive capacity with CacheCapacityInvalidException**
4. **Cache rejects non-positive TTL with CacheTtlInvalidException**

### Put and Get

5. **Putting a key then getting it returns the stored value**
6. **Getting a missing key returns null**
7. **Put increases the size by one**
8. **Putting the same key twice replaces the value without growing the size**
9. **Contains returns true for a stored key**
10. **Contains returns false for a missing key**

### Capacity and LRU Eviction

11. **Filling the cache to capacity does not evict**
12. **Putting a new key when at capacity evicts the least-recently-used key**
13. **Getting a key refreshes recency so it is not the next evicted**
14. **Replacing an existing key refreshes recency so it is not the next evicted**

### TTL Expiry

15. **A get after TTL has elapsed returns null**
16. **Contains returns false once TTL has elapsed**
17. **An expired entry is not counted in size after eviction sweep**
18. **Explicit evictExpired removes all expired entries**
19. **Explicit evictExpired leaves live entries intact**
20. **TTL is measured from insertion time, not from last access**
