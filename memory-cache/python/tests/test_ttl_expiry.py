from __future__ import annotations

from datetime import timedelta

from .cache_builder import CacheBuilder

ONE_MINUTE = timedelta(minutes=1)


def test_a_get_after_ttl_has_elapsed_returns_none():
    cache, clock = CacheBuilder[str]().with_ttl(ONE_MINUTE).build()
    cache.put("alpha", "one")

    clock.advance(ONE_MINUTE)

    assert cache.get("alpha") is None


def test_contains_returns_false_once_ttl_has_elapsed():
    cache, clock = CacheBuilder[str]().with_ttl(ONE_MINUTE).build()
    cache.put("alpha", "one")

    clock.advance(ONE_MINUTE)

    assert cache.contains("alpha") is False


def test_an_expired_entry_is_not_counted_in_size_after_eviction_sweep():
    cache, clock = CacheBuilder[str]().with_ttl(ONE_MINUTE).build()
    cache.put("alpha", "one")
    cache.put("beta", "two")

    clock.advance(ONE_MINUTE)
    cache.evict_expired()

    assert cache.size == 0


def test_explicit_evict_expired_removes_all_expired_entries():
    cache, clock = CacheBuilder[str]().with_ttl(ONE_MINUTE).build()
    cache.put("alpha", "one")
    cache.put("beta", "two")
    cache.put("gamma", "three")

    clock.advance(ONE_MINUTE)
    cache.evict_expired()

    assert cache.contains("alpha") is False
    assert cache.contains("beta") is False
    assert cache.contains("gamma") is False


def test_explicit_evict_expired_leaves_live_entries_intact():
    cache, clock = CacheBuilder[str]().with_ttl(ONE_MINUTE).build()
    cache.put("old", "stale")
    clock.advance(timedelta(seconds=30))
    cache.put("fresh", "alive")

    clock.advance(timedelta(seconds=30))
    cache.evict_expired()

    assert cache.contains("old") is False
    assert cache.contains("fresh") is True
    assert cache.get("fresh") == "alive"


def test_ttl_is_measured_from_insertion_time_not_from_last_access():
    cache, clock = CacheBuilder[str]().with_ttl(ONE_MINUTE).build()
    cache.put("alpha", "one")

    clock.advance(timedelta(seconds=30))
    assert cache.get("alpha") == "one"

    clock.advance(timedelta(seconds=30))
    assert cache.get("alpha") is None
