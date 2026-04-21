namespace RollYourOwnMockFramework;

public class MethodVerification
{
    private readonly Mock _mock;
    private readonly string _methodName;

    internal MethodVerification(Mock mock, string methodName)
    {
        _mock = mock;
        _methodName = methodName;
    }

    public void WasCalled()
    {
        var called = _mock.Calls.Any(c => c.MethodName == _methodName);
        if (!called)
        {
            throw new VerificationError($"expected {_methodName} to be called but was never called");
        }
    }

    public void WasCalledWith(params object?[] expectedArgs)
    {
        var methodCalls = _mock.Calls.Where(c => c.MethodName == _methodName).ToList();

        if (methodCalls.Count == 0)
        {
            throw new VerificationError(
                $"expected {_methodName}({FormatArgs(expectedArgs)}) to be called but was never called");
        }

        var match = methodCalls.Any(c => ArgsMatch(expectedArgs, c.Args));
        if (!match)
        {
            var actualArgs = methodCalls[0].Args;
            throw new VerificationError(
                $"expected {_methodName}({FormatArgs(expectedArgs)}) to be called but was called with ({FormatArgs(actualArgs)})");
        }
    }

    public void WasCalledTimes(int expectedCount)
    {
        var actualCount = _mock.Calls.Count(c => c.MethodName == _methodName);
        if (actualCount != expectedCount)
        {
            throw new VerificationError(
                $"expected {_methodName} to be called {expectedCount} times but was called {actualCount} times");
        }
    }

    private static string FormatArgs(object?[] args)
    {
        return string.Join(", ", args.Select(a => a?.ToString() ?? "null"));
    }

    private static bool ArgsMatch(object?[] expected, object?[] actual)
    {
        if (expected.Length != actual.Length) return false;
        for (var i = 0; i < expected.Length; i++)
        {
            if (!Equals(expected[i], actual[i])) return false;
        }
        return true;
    }
}
