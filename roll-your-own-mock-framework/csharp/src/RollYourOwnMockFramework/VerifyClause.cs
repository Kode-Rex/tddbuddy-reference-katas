using System.Dynamic;

namespace RollYourOwnMockFramework;

public class VerifyClause : DynamicObject
{
    private readonly Mock _mock;

    internal VerifyClause(Mock mock)
    {
        _mock = mock;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = new MethodVerification(_mock, binder.Name);
        return true;
    }
}
