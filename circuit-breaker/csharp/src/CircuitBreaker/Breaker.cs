namespace CircuitBreaker;

public class Breaker
{
    public const int DefaultFailureThreshold = 5;
    public static readonly TimeSpan DefaultResetTimeout = TimeSpan.FromSeconds(30);

    private readonly int _failureThreshold;
    private readonly TimeSpan _resetTimeout;
    private readonly IClock _clock;

    private BreakerState _state = BreakerState.Closed;
    private int _consecutiveFailures;
    private DateTime _openedAt;

    public Breaker(int failureThreshold, TimeSpan resetTimeout, IClock clock)
    {
        if (failureThreshold <= 0)
            throw new BreakerThresholdInvalidException("Failure threshold must be positive");
        if (resetTimeout <= TimeSpan.Zero)
            throw new BreakerTimeoutInvalidException("Reset timeout must be positive");

        _failureThreshold = failureThreshold;
        _resetTimeout = resetTimeout;
        _clock = clock;
    }

    public BreakerState State => _state;

    public T Execute<T>(Func<T> operation)
    {
        if (_state == BreakerState.Open)
        {
            if (_clock.Now() - _openedAt >= _resetTimeout)
            {
                _state = BreakerState.HalfOpen;
            }
            else
            {
                throw new CircuitOpenException("Circuit is open");
            }
        }

        try
        {
            var result = operation();
            OnSuccess();
            return result;
        }
        catch
        {
            OnFailure();
            throw;
        }
    }

    private void OnSuccess()
    {
        _consecutiveFailures = 0;
        _state = BreakerState.Closed;
    }

    private void OnFailure()
    {
        if (_state == BreakerState.HalfOpen)
        {
            Trip();
            return;
        }

        _consecutiveFailures++;
        if (_consecutiveFailures >= _failureThreshold)
        {
            Trip();
        }
    }

    private void Trip()
    {
        _state = BreakerState.Open;
        _openedAt = _clock.Now();
    }
}
