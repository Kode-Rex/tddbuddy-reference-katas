namespace RateLimiter;

public class Limiter
{
    public const int DefaultMaxRequests = 100;
    public static readonly TimeSpan DefaultWindowDuration = TimeSpan.FromSeconds(60);

    private readonly Rule _rule;
    private readonly IClock _clock;
    private readonly Dictionary<string, WindowState> _windows = new();

    public Limiter(Rule rule, IClock clock)
    {
        _rule = rule;
        _clock = clock;
    }

    public Decision Request(string key)
    {
        var now = _clock.Now();

        if (!_windows.TryGetValue(key, out var state) || now >= state.Start + _rule.Window)
        {
            _windows[key] = new WindowState(now, 1);
            return new Decision.Allowed();
        }

        if (state.Count < _rule.MaxRequests)
        {
            _windows[key] = state with { Count = state.Count + 1 };
            return new Decision.Allowed();
        }

        var retryAfter = (state.Start + _rule.Window) - now;
        return new Decision.Rejected(retryAfter);
    }

    private record struct WindowState(DateTime Start, int Count);
}
