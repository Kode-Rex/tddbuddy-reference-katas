namespace RollYourOwnTestFramework;

public static class Assertions
{
    public static void AssertEqual(object expected, object actual)
    {
        if (!Equals(expected, actual))
            throw new AssertionFailedException($"expected {expected} but got {actual}");
    }

    public static void AssertTrue(bool condition)
    {
        if (!condition)
            throw new AssertionFailedException("expected true");
    }

    public static void AssertThrows(Action action)
    {
        try
        {
            action();
        }
        catch
        {
            return;
        }

        throw new AssertionFailedException("expected exception");
    }
}
