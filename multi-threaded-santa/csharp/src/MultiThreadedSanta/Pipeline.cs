namespace MultiThreadedSanta;

/// <summary>
/// Four-stage present pipeline: Make -> Wrap -> Load -> Deliver.
/// Supports both sequential (single-thread) and concurrent (multi-worker) execution.
/// The sleigh constraint: loading and delivering cannot happen simultaneously.
/// </summary>
public sealed class Pipeline
{
    private readonly BoundedQueue<Present> _madeQueue;
    private readonly BoundedQueue<Present> _wrappedQueue;
    private readonly BoundedQueue<Present> _loadedQueue;
    private readonly List<Present> _delivered = new();
    private readonly SemaphoreSlim _sleighLock = new(1, 1);
    private readonly object _deliveredLock = new();

    public BoundedQueue<Present> MadeQueue => _madeQueue;
    public BoundedQueue<Present> WrappedQueue => _wrappedQueue;
    public BoundedQueue<Present> LoadedQueue => _loadedQueue;
    public IReadOnlyList<Present> Delivered
    {
        get { lock (_deliveredLock) return _delivered.ToList().AsReadOnly(); }
    }

    public Pipeline(int madeCapacity = 1000, int wrappedCapacity = 2000, int loadedCapacity = 5000)
    {
        _madeQueue = new BoundedQueue<Present>(madeCapacity);
        _wrappedQueue = new BoundedQueue<Present>(wrappedCapacity);
        _loadedQueue = new BoundedQueue<Present>(loadedCapacity);
    }

    /// <summary>
    /// Process all presents sequentially through the full pipeline.
    /// No concurrency — useful for proving correctness before adding threads.
    /// </summary>
    public async Task ProcessSequentiallyAsync(IReadOnlyList<Present> presents, CancellationToken ct = default)
    {
        foreach (var present in presents)
        {
            present.Make();
            await _madeQueue.EnqueueAsync(present, ct);
        }
        _madeQueue.Complete();

        await foreach (var present in _madeQueue.ReadAllAsync(ct))
        {
            present.Wrap();
            await _wrappedQueue.EnqueueAsync(present, ct);
        }
        _wrappedQueue.Complete();

        await foreach (var present in _wrappedQueue.ReadAllAsync(ct))
        {
            present.Load();
            await _loadedQueue.EnqueueAsync(present, ct);
        }
        _loadedQueue.Complete();

        await foreach (var present in _loadedQueue.ReadAllAsync(ct))
        {
            present.Deliver();
            lock (_deliveredLock) _delivered.Add(present);
        }
    }

    /// <summary>
    /// Process presents concurrently with configurable worker counts per stage.
    /// The delivery stage always uses exactly one worker (sleigh constraint).
    /// Loading acquires the sleigh lock; delivery acquires it too — so they cannot overlap.
    /// </summary>
    public async Task ProcessConcurrentlyAsync(
        IReadOnlyList<Present> presents,
        int makeWorkers = 1,
        int wrapWorkers = 1,
        int loadWorkers = 1,
        CancellationToken ct = default)
    {
        // Feed queue distributes presents to maker workers
        var feedQueue = new BoundedQueue<Present>(presents.Count > 0 ? presents.Count : 1);
        foreach (var p in presents)
            await feedQueue.EnqueueAsync(p, ct);
        feedQueue.Complete();

        // Stage 1: Make — pull from feed, make, push to madeQueue
        var makerTasks = LaunchWorkers(makeWorkers, async () =>
        {
            await foreach (var present in feedQueue.ReadAllAsync(ct))
            {
                present.Make();
                await _madeQueue.EnqueueAsync(present, ct);
            }
        });

        // Stage 2: Wrap — pull from madeQueue, wrap, push to wrappedQueue
        var wrapTasks = LaunchWorkers(wrapWorkers, async () =>
        {
            await foreach (var present in _madeQueue.ReadAllAsync(ct))
            {
                present.Wrap();
                await _wrappedQueue.EnqueueAsync(present, ct);
            }
        });

        // Stage 3: Load — pull from wrappedQueue, acquire sleigh lock, load, push to loadedQueue
        var loadTasks = LaunchWorkers(loadWorkers, async () =>
        {
            await foreach (var present in _wrappedQueue.ReadAllAsync(ct))
            {
                await _sleighLock.WaitAsync(ct);
                try
                {
                    present.Load();
                    await _loadedQueue.EnqueueAsync(present, ct);
                }
                finally
                {
                    _sleighLock.Release();
                }
            }
        });

        // Stage 4: Deliver — single worker, acquires sleigh lock
        var deliverTask = Task.Run(async () =>
        {
            await foreach (var present in _loadedQueue.ReadAllAsync(ct))
            {
                await _sleighLock.WaitAsync(ct);
                try
                {
                    present.Deliver();
                    lock (_deliveredLock) _delivered.Add(present);
                }
                finally
                {
                    _sleighLock.Release();
                }
            }
        }, ct);

        // Cascade completions through the pipeline
        await Task.WhenAll(makerTasks);
        _madeQueue.Complete();

        await Task.WhenAll(wrapTasks);
        _wrappedQueue.Complete();

        await Task.WhenAll(loadTasks);
        _loadedQueue.Complete();

        await deliverTask;
    }

    /// <summary>Returns the total number of workers used across all stages.</summary>
    public static int TotalElves(int makeWorkers, int wrapWorkers, int loadWorkers)
    {
        return makeWorkers + wrapWorkers + loadWorkers + 1; // +1 for delivery
    }

    private static Task[] LaunchWorkers(int count, Func<Task> work)
    {
        return Enumerable.Range(0, count)
            .Select(_ => Task.Run(work))
            .ToArray();
    }
}
