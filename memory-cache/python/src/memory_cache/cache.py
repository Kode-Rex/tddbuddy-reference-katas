from __future__ import annotations

from collections import OrderedDict
from dataclasses import dataclass
from datetime import datetime, timedelta
from typing import Generic, Optional, TypeVar

from .clock import Clock
from .exceptions import CacheCapacityInvalidError, CacheTtlInvalidError

V = TypeVar("V")

DEFAULT_CAPACITY = 100
DEFAULT_TTL = timedelta(seconds=60)


@dataclass(frozen=True)
class _Entry(Generic[V]):
    value: V
    inserted_at: datetime


class Cache(Generic[V]):
    def __init__(self, capacity: int, ttl: timedelta, clock: Clock) -> None:
        if capacity <= 0:
            raise CacheCapacityInvalidError("Capacity must be positive")
        if ttl <= timedelta(0):
            raise CacheTtlInvalidError("TTL must be positive")
        self._capacity = capacity
        self._ttl = ttl
        self._clock = clock
        # OrderedDict gives us O(1) move-to-end for recency updates.
        self._entries: "OrderedDict[str, _Entry[V]]" = OrderedDict()

    @property
    def size(self) -> int:
        return len(self._entries)

    def put(self, key: str, value: V) -> None:
        now = self._clock.now()
        if key in self._entries:
            self._entries[key] = _Entry(value, now)
            self._entries.move_to_end(key, last=False)
            return

        if len(self._entries) >= self._capacity:
            # popitem(last=True) would remove the MRU; we want LRU.
            # Our convention: move_to_end(last=False) puts MRU at the front,
            # so the LRU is the *last* item. Pop it.
            self._entries.popitem(last=True)

        self._entries[key] = _Entry(value, now)
        self._entries.move_to_end(key, last=False)

    def get(self, key: str) -> Optional[V]:
        entry = self._entries.get(key)
        if entry is None:
            return None
        if self._is_expired(entry):
            del self._entries[key]
            return None
        self._entries.move_to_end(key, last=False)
        return entry.value

    def contains(self, key: str) -> bool:
        entry = self._entries.get(key)
        if entry is None:
            return False
        return not self._is_expired(entry)

    def evict_expired(self) -> None:
        stale = [k for k, e in self._entries.items() if self._is_expired(e)]
        for k in stale:
            del self._entries[k]

    def _is_expired(self, entry: _Entry[V]) -> bool:
        return self._clock.now() - entry.inserted_at >= self._ttl
