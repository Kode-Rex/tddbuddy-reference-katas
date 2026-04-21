using System.Dynamic;

namespace RollYourOwnMockFramework;

public class Mock : DynamicObject
{
    private readonly List<CallRecord> _calls = new();
    private readonly Dictionary<string, List<(object?[] Args, object? ReturnValue)>> _stubs = new();

    public IReadOnlyList<CallRecord> Calls => _calls;

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var methodArgs = args ?? Array.Empty<object?>();
        _calls.Add(new CallRecord(binder.Name, methodArgs));

        result = null;
        if (_stubs.TryGetValue(binder.Name, out var stubs))
        {
            foreach (var stub in stubs)
            {
                if (ArgsMatch(stub.Args, methodArgs))
                {
                    result = stub.ReturnValue;
                    break;
                }
            }
        }

        return true;
    }

    internal void AddStub(string methodName, object?[] args, object? returnValue)
    {
        if (!_stubs.ContainsKey(methodName))
        {
            _stubs[methodName] = new List<(object?[] Args, object? ReturnValue)>();
        }

        _stubs[methodName].Add((args, returnValue));
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
