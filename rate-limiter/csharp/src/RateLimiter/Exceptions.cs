namespace RateLimiter;

public class LimiterRuleInvalidException : Exception
{
    public LimiterRuleInvalidException(string message) : base(message) { }
}
