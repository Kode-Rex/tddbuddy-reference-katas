using System.Dynamic;

namespace RollYourOwnMockFramework;

public class WhenClause : DynamicObject
{
    private readonly Mock _mock;

    internal WhenClause(Mock mock)
    {
        _mock = mock;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var methodArgs = args ?? Array.Empty<object?>();
        result = new StubConfiguration(_mock, binder.Name, methodArgs);
        return true;
    }
}
