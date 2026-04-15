from __future__ import annotations

from datetime import datetime, timedelta, timezone
from typing import Generic, Optional, TypeVar

from memory_cache import Cache, DEFAULT_CAPACITY, DEFAULT_TTL

from .fixed_clock import FixedClock

V = TypeVar("V")


class CacheBuilder(Generic[V]):
    def __init__(self) -> None:
        self._capacity: int = DEFAULT_CAPACITY
        self._ttl: timedelta = DEFAULT_TTL
        self._start: datetime = datetime(2026, 1, 1, tzinfo=timezone.utc)
        self._clock: Optional[FixedClock] = None

    def with_capacity(self, capacity: int) -> "CacheBuilder[V]":
        self._capacity = capacity
        return self

    def with_ttl(self, ttl: timedelta) -> "CacheBuilder[V]":
        self._ttl = ttl
        return self

    def starting_at(self, start: datetime) -> "CacheBuilder[V]":
        self._start = start
        return self

    def with_clock(self, clock: FixedClock) -> "CacheBuilder[V]":
        self._clock = clock
        return self

    def build(self) -> tuple[Cache[V], FixedClock]:
        clock = self._clock if self._clock is not None else FixedClock(self._start)
        cache: Cache[V] = Cache(self._capacity, self._ttl, clock)
        return cache, clock
