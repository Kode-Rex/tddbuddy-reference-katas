# Memory Cache — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Cache<V> ──owns──> LinkedList<Entry>   (recency order: head = most recent, tail = LRU)
   │      ──owns──> Dictionary<string, Node>  (O(1) key lookup)
   │
   ├── Put(key, value) : void     — evicts LRU when at capacity
   ├── Get(key) : V?              — lazily drops expired; refreshes recency on hit
   ├── Contains(key) : bool       — live-entry check; does not refresh recency
   ├── Size : int
   └── EvictExpired() : void      — explicit sweep
   
Collaborator: IClock.Now() — injected; FixedClock in tests
Defaults: DefaultCapacity = 100, DefaultTtl = 60 seconds
```

## Why `Cache<TValue>` Is Generic

The kata brief pins string values, but the *shape* of a cache doesn't care what the values are. Making `Cache<TValue>` generic keeps recency, TTL, and capacity logic free of any value-type assumptions, and tests read the same whether the cache holds strings or session objects. The bonus task on the astro-site page even contemplates `int`, `double`, `MyAwesomeType` — the generic form lets a reader see the seam without the noise of configurable-type plumbing.

See `src/MemoryCache/Cache.cs`.

## Why `IClock`, Not `DateTime.UtcNow`

Six scenarios pivot on time elapsing: TTL expiry via lazy `Get`, `Contains` after expiry, size after sweep, explicit `EvictExpired`, "TTL from insertion, not last access". If those tests called `DateTime.UtcNow` directly, they'd either sleep (slow, flaky) or thread a stopwatch into every assertion.

`IClock.Now()` makes the collaboration explicit. `FixedClock` in the test project is the deterministic fake — not a mocking-library mock, just a tiny `IClock` that remembers the time the test set. The tests read "put at t=0, advance one minute, get returns null" instead of "sleep then hope." That's the **Mocks as Behavioral Specifications** principle: when collaboration *is* part of the behavior, make it an interface.

See `src/MemoryCache/IClock.cs` and `tests/MemoryCache.Tests/FixedClock.cs`.

## Why `CacheBuilder` Returns a Tuple

Most TTL tests need to **advance time after the cache exists** — put now, jump forward a minute, get null. If the builder only returned the cache, the test would have no handle on the clock. Returning `(Cache, FixedClock)` gives the test exactly the two collaborators it drives, without reaching into private fields.

The builder also defaults capacity and TTL to the domain constants, so scenarios that only care about *one* dimension (say, LRU eviction) don't have to spell out the TTL too. `.WithCapacity(3).Build()` is the line an LRU test wants to write.

See `tests/MemoryCache.Tests/CacheBuilder.cs`.

## Why `EvictExpired()` Is an Explicit Sweep

TTL expiry has two possible shapes:

1. **Implicit sweep on every operation** — every `Put`, `Get`, `Contains` starts by purging expired entries. Simple model, but every entry point pays the scan cost and `Size` either walks the index on every read or goes stale between sweeps.
2. **Explicit sweep** — a separate `EvictExpired()` method scans once and drops every stale entry in one pass.

This implementation chose (2), the same shape as `library-management`'s [`ExpireReservations`](../../library-management/csharp/WALKTHROUGH.md#why-expirereservations-is-an-explicit-sweep) and `video-club-rental`'s [`MarkOverdueRentals`](../../video-club-rental/csharp/WALKTHROUGH.md#why-markoverduerentals-is-an-explicit-sweep). The reason is the same: expiration is *a consequence of time passing*, not of any user action. A production caller runs the sweep on a schedule — a background tick, a pressure-triggered compaction, whatever fits the deployment — and the tests model that exact shape. "Advance the clock, call `EvictExpired`, assert size shrinks" reads as the production story.

`Get` still performs **lazy expiry** on the single key it looked up: an expired hit returns null and removes the stale entry on the spot. That's cheap (the key was already in hand) and keeps the caller's mental model — *once the TTL passes, the value is gone* — honest without the caller having to sweep first. `Size` and `Contains` over non-accessed keys are what make the explicit sweep earn its keep; without it, `Size` would either silently lie or pay for a full scan on every read.

See `src/MemoryCache/Cache.cs` (methods `Get` and `EvictExpired`).

## Why LRU Is a LinkedList + Dictionary, Not a Sorted Structure

LRU needs two operations in O(1): *find a key* and *move it to the front on access*. A hash map alone gives find but not cheap reordering. A sorted list gives order but O(n) find. The classic trick — a doubly-linked list for recency paired with a dictionary from key to list-node — gives both.

`Put` is O(1): dictionary lookup, list unlink, list prepend. `Get` on a live entry is the same. Eviction is the tail of the list. The two data structures are always kept in sync inside `Cache` — no external caller sees either — so the invariant *every key in the dictionary points to a node in the list, and only those* stays local to this file.

## Why Domain Exception Types (`CacheCapacityInvalidException`, `CacheTtlInvalidException`)

A zero or negative capacity doesn't form a cache. Neither does a zero TTL. Throwing `ArgumentException` would blur those two failures with "some other argument was wrong somewhere." Named exceptions put the domain rule in the type system: tests `Should().Throw<CacheCapacityInvalidException>()`, and a reader sees the capacity invariant named in the stack trace.

The messages — `"Capacity must be positive"`, `"TTL must be positive"` — are **byte-identical** across the three languages. Only the exception class differs (C# `...Exception`, TS `...Error`, Python error class).

See `src/MemoryCache/Exceptions.cs`.

## Why Three Test Files

The twenty scenarios split naturally into three concerns:

- `PutGetTests.cs` — construction, basic put/get, contains, size (scenarios 1–10)
- `CapacityEvictionTests.cs` — LRU behavior at capacity (scenarios 11–14)
- `TtlExpiryTests.cs` — TTL via lazy `Get` and explicit `EvictExpired` (scenarios 15–20)

One `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## What's Deliberately Not Modeled

The kata brief's bonus section asks for configuration-file-driven TTL and capacity, and runtime-configurable value types. This reference scopes to the twenty core scenarios — every line of domain code earns its keep against a named test. The generic `Cache<V>` already shows how a value-type seam looks; the config-file loader is an application-layer concern, not a domain one.

## How to Run

```bash
cd memory-cache/csharp
dotnet test
```
