namespace RollYourOwnMockFramework;

public class StubConfiguration
{
    private readonly Mock _mock;
    private readonly string _methodName;
    private readonly object?[] _args;

    internal StubConfiguration(Mock mock, string methodName, object?[] args)
    {
        _mock = mock;
        _methodName = methodName;
        _args = args;
    }

    public void ThenReturn(object? value)
    {
        _mock.AddStub(_methodName, _args, value);
    }
}
