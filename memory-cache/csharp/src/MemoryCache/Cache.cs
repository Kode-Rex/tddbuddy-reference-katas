namespace MemoryCache;

public class Cache<TValue>
{
    public const int DefaultCapacity = 100;
    public static readonly TimeSpan DefaultTtl = TimeSpan.FromSeconds(60);

    private readonly int _capacity;
    private readonly TimeSpan _ttl;
    private readonly IClock _clock;

    // LinkedList gives O(1) recency moves; the dictionary indexes into it by key.
    private readonly LinkedList<Entry> _recency = new();
    private readonly Dictionary<string, LinkedListNode<Entry>> _index = new();

    public Cache(int capacity, TimeSpan ttl, IClock clock)
    {
        if (capacity <= 0)
            throw new CacheCapacityInvalidException("Capacity must be positive");
        if (ttl <= TimeSpan.Zero)
            throw new CacheTtlInvalidException("TTL must be positive");

        _capacity = capacity;
        _ttl = ttl;
        _clock = clock;
    }

    public int Size => _index.Count;

    public void Put(string key, TValue value)
    {
        if (_index.TryGetValue(key, out var existing))
        {
            existing.Value = new Entry(key, value, _clock.Now());
            _recency.Remove(existing);
            _recency.AddFirst(existing);
            return;
        }

        if (_index.Count >= _capacity)
        {
            var lru = _recency.Last;
            if (lru is not null)
            {
                _recency.RemoveLast();
                _index.Remove(lru.Value.Key);
            }
        }

        var node = new LinkedListNode<Entry>(new Entry(key, value, _clock.Now()));
        _recency.AddFirst(node);
        _index[key] = node;
    }

    public TValue? Get(string key)
    {
        if (!_index.TryGetValue(key, out var node))
            return default;

        if (IsExpired(node.Value))
        {
            _recency.Remove(node);
            _index.Remove(key);
            return default;
        }

        _recency.Remove(node);
        _recency.AddFirst(node);
        return node.Value.Value;
    }

    public bool Contains(string key)
    {
        if (!_index.TryGetValue(key, out var node))
            return false;
        return !IsExpired(node.Value);
    }

    public void EvictExpired()
    {
        var node = _recency.First;
        while (node is not null)
        {
            var next = node.Next;
            if (IsExpired(node.Value))
            {
                _recency.Remove(node);
                _index.Remove(node.Value.Key);
            }
            node = next;
        }
    }

    private bool IsExpired(Entry entry) => _clock.Now() - entry.InsertedAt >= _ttl;

    private record struct Entry(string Key, TValue Value, DateTime InsertedAt);
}
