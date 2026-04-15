namespace MemoryCache;

public class CacheCapacityInvalidException : Exception
{
    public CacheCapacityInvalidException(string message) : base(message) { }
}

public class CacheTtlInvalidException : Exception
{
    public CacheTtlInvalidException(string message) : base(message) { }
}
