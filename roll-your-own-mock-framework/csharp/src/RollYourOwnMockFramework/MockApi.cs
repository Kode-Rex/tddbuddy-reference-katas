namespace RollYourOwnMockFramework;

public static class MockApi
{
    public static Mock CreateMock()
    {
        return new Mock();
    }

    public static dynamic When(Mock mock)
    {
        return new WhenClause(mock);
    }

    public static dynamic Verify(Mock mock)
    {
        return new VerifyClause(mock);
    }
}
