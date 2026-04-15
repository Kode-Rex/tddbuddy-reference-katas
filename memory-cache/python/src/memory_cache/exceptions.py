class CacheCapacityInvalidError(Exception):
    """Raised when a cache is constructed with a non-positive capacity."""
    pass


class CacheTtlInvalidError(Exception):
    """Raised when a cache is constructed with a non-positive TTL."""
    pass
