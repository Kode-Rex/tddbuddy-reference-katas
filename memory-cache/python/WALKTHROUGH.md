# Memory Cache — Python Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md) — read that first for the full rationale (generic cache, injected clock, explicit sweep with cross-links, O(1) LRU, named domain exceptions).

This note captures only the Python deltas.

## `OrderedDict` Replaces LinkedList + Dictionary

Python's `collections.OrderedDict` already *is* a hash map with insertion-order tracking and O(1) `move_to_end`. The C#/TS implementations hand-roll the doubly-linked list + map pairing; Python gets it from the standard library. Convention in this implementation: MRU lives at the **front** (`move_to_end(key, last=False)`), so the LRU is at the **back** and `popitem(last=True)` evicts it.

See `src/memory_cache/cache.py`.

## `Clock` as a `Protocol`

The C# version uses an interface; TS uses a structural interface. Python uses `typing.Protocol` — structural typing without a runtime `isinstance` check. `FixedClock` in the tests doesn't inherit from `Clock`; it just exposes `now() -> datetime`, and the type checker accepts it.

See `src/memory_cache/clock.py`.

## `timedelta` and `datetime`

TTL is a `timedelta`; insertion time is a `datetime`. The builder defaults to `datetime(2026, 1, 1, tzinfo=timezone.utc)` so the tests never construct naive-local times — TTL comparisons are pure UTC arithmetic. The domain constant `DEFAULT_TTL = timedelta(seconds=60)` lives alongside `DEFAULT_CAPACITY = 100`.

## `Cache[V]` Generic

Python 3.11 uses `Generic[V]` with a `TypeVar` rather than the 3.12+ `class Cache[V]` syntax, matching the repo's pinned minimum. The type parameter participates in `get` returning `Optional[V]` so callers can discriminate "not present or expired" (`None`) from a real value.

## Domain Error Classes

`CacheCapacityInvalidError` and `CacheTtlInvalidError` subclass `Exception`, not `ValueError`. The Full-Bake F3 convention is to name the rejection rather than lean on a general-purpose type. The messages — `"Capacity must be positive"`, `"TTL must be positive"` — are byte-identical to the C# and TypeScript implementations.

See `src/memory_cache/exceptions.py`.

## Scenario Map

Twenty scenarios across three test files:

- `tests/test_put_get.py` — scenarios 1–10
- `tests/test_capacity_eviction.py` — scenarios 11–14
- `tests/test_ttl_expiry.py` — scenarios 15–20

## How to Run

```bash
cd memory-cache/python
python -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```
