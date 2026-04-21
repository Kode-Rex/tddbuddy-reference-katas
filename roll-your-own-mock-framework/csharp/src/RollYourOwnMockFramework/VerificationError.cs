namespace RollYourOwnMockFramework;

public class VerificationError : Exception
{
    public VerificationError(string message) : base(message) { }
}
