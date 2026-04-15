namespace CircuitBreaker;

public class BreakerThresholdInvalidException : Exception
{
    public BreakerThresholdInvalidException(string message) : base(message) { }
}

public class BreakerTimeoutInvalidException : Exception
{
    public BreakerTimeoutInvalidException(string message) : base(message) { }
}

public class CircuitOpenException : Exception
{
    public CircuitOpenException(string message) : base(message) { }
}
