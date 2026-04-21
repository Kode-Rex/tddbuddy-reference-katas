using System.Threading.Channels;

namespace MultiThreadedSanta;

/// <summary>
/// Thread-safe bounded queue backed by <see cref="Channel{T}"/>.
/// Producers block when full; consumers block when empty.
/// Call <see cref="Complete"/> to signal no more items will be enqueued.
/// </summary>
public sealed class BoundedQueue<T>
{
    private readonly Channel<T> _channel;

    public int Capacity { get; }

    public BoundedQueue(int capacity)
    {
        Capacity = capacity;
        _channel = Channel.CreateBounded<T>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleWriter = false,
            SingleReader = false
        });
    }

    /// <summary>Returns the number of items currently in the queue.</summary>
    public int Count => _channel.Reader.Count;

    /// <summary>
    /// Enqueue an item. Blocks (asynchronously) if the queue is at capacity.
    /// Returns false if the queue has been completed.
    /// </summary>
    public async ValueTask<bool> EnqueueAsync(T item, CancellationToken ct = default)
    {
        try
        {
            await _channel.Writer.WriteAsync(item, ct);
            return true;
        }
        catch (ChannelClosedException)
        {
            return false;
        }
    }

    /// <summary>
    /// Try to enqueue an item without waiting.
    /// Returns false if the queue is full or completed.
    /// </summary>
    public bool TryEnqueue(T item)
    {
        return _channel.Writer.TryWrite(item);
    }

    /// <summary>
    /// Dequeue an item. Blocks (asynchronously) if the queue is empty.
    /// Returns (true, item) or (false, default) when the queue is completed and empty.
    /// </summary>
    public async ValueTask<(bool Success, T? Item)> DequeueAsync(CancellationToken ct = default)
    {
        try
        {
            var item = await _channel.Reader.ReadAsync(ct);
            return (true, item);
        }
        catch (ChannelClosedException)
        {
            return (false, default);
        }
    }

    /// <summary>
    /// Signal that no more items will be enqueued.
    /// Consumers will drain remaining items and then see completion.
    /// </summary>
    public void Complete()
    {
        _channel.Writer.TryComplete();
    }

    /// <summary>Async enumerable over all items until completion.</summary>
    public IAsyncEnumerable<T> ReadAllAsync(CancellationToken ct = default)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}
