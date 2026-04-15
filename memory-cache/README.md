# Memory Cache

A bounded, TTL-expiring, in-memory key-value cache. Entries live for a configured time-to-live and the cache evicts its least-recently-used key when capacity is reached. Great for practicing **injected clocks**, **explicit sweeps**, and **generic domain types** with test data builders.

## What this kata teaches

- **Test Data Builders** — `CacheBuilder().WithCapacity(n).WithTtl(...).WithClock(clock).Build()` returns a `(Cache, Clock)` tuple so tests advance time.
- **Injected Clock** — `Clock` is a collaborator interface; `FixedClock` in tests lets TTL expiry tests drive the behavior deterministically.
- **Explicit Sweep** — `evictExpired()` mirrors `library-management`'s `ExpireReservations` and `video-club-rental`'s `MarkOverdueRentals`. Expiry is a consequence of time passing, not of a user action; the sweep is the production caller's responsibility.
- **Domain Exception Types** — `CacheCapacityInvalidException` and `CacheTtlInvalidException` name the rejection, rather than throwing a generic `Error`/`ValueError`.
- **Generic Value Type** — `Cache<V>` (C#/TS) / `Cache[V]` (Python) keeps the cache polymorphic without leaking concrete types into the domain.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
