from __future__ import annotations

import pytest

from memory_cache import CacheCapacityInvalidError, CacheTtlInvalidError

from .cache_builder import CacheBuilder


def test_a_new_cache_has_size_zero():
    cache, _ = CacheBuilder[str]().build()
    assert cache.size == 0


def test_a_new_cache_contains_no_keys():
    cache, _ = CacheBuilder[str]().build()
    assert cache.contains("anything") is False


def test_cache_rejects_non_positive_capacity_with_CacheCapacityInvalidError():
    with pytest.raises(CacheCapacityInvalidError) as excinfo:
        CacheBuilder[str]().with_capacity(0).build()
    assert str(excinfo.value) == "Capacity must be positive"


def test_cache_rejects_non_positive_ttl_with_CacheTtlInvalidError():
    from datetime import timedelta
    with pytest.raises(CacheTtlInvalidError) as excinfo:
        CacheBuilder[str]().with_ttl(timedelta(0)).build()
    assert str(excinfo.value) == "TTL must be positive"


def test_putting_a_key_then_getting_it_returns_the_stored_value():
    cache, _ = CacheBuilder[str]().build()
    cache.put("alpha", "one")
    assert cache.get("alpha") == "one"


def test_getting_a_missing_key_returns_none():
    cache, _ = CacheBuilder[str]().build()
    assert cache.get("absent") is None


def test_put_increases_the_size_by_one():
    cache, _ = CacheBuilder[str]().build()
    cache.put("alpha", "one")
    assert cache.size == 1


def test_putting_the_same_key_twice_replaces_the_value_without_growing_the_size():
    cache, _ = CacheBuilder[str]().build()
    cache.put("alpha", "one")
    cache.put("alpha", "two")
    assert cache.size == 1
    assert cache.get("alpha") == "two"


def test_contains_returns_true_for_a_stored_key():
    cache, _ = CacheBuilder[str]().build()
    cache.put("alpha", "one")
    assert cache.contains("alpha") is True


def test_contains_returns_false_for_a_missing_key():
    cache, _ = CacheBuilder[str]().build()
    assert cache.contains("absent") is False
