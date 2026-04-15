from __future__ import annotations

from .cache_builder import CacheBuilder


def test_filling_the_cache_to_capacity_does_not_evict():
    cache, _ = CacheBuilder[str]().with_capacity(3).build()
    cache.put("a", "1")
    cache.put("b", "2")
    cache.put("c", "3")

    assert cache.size == 3
    assert cache.contains("a") is True
    assert cache.contains("b") is True
    assert cache.contains("c") is True


def test_putting_a_new_key_when_at_capacity_evicts_the_least_recently_used_key():
    cache, _ = CacheBuilder[str]().with_capacity(3).build()
    cache.put("a", "1")
    cache.put("b", "2")
    cache.put("c", "3")

    cache.put("d", "4")

    assert cache.size == 3
    assert cache.contains("a") is False
    assert cache.contains("d") is True


def test_getting_a_key_refreshes_recency_so_it_is_not_the_next_evicted():
    cache, _ = CacheBuilder[str]().with_capacity(3).build()
    cache.put("a", "1")
    cache.put("b", "2")
    cache.put("c", "3")

    cache.get("a")
    cache.put("d", "4")

    assert cache.contains("a") is True
    assert cache.contains("b") is False


def test_replacing_an_existing_key_refreshes_recency_so_it_is_not_the_next_evicted():
    cache, _ = CacheBuilder[str]().with_capacity(3).build()
    cache.put("a", "1")
    cache.put("b", "2")
    cache.put("c", "3")

    cache.put("a", "1-new")
    cache.put("d", "4")

    assert cache.contains("a") is True
    assert cache.get("a") == "1-new"
    assert cache.contains("b") is False
