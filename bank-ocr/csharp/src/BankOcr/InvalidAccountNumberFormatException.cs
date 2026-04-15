namespace BankOcr;

public class InvalidAccountNumberFormatException : Exception
{
    public InvalidAccountNumberFormatException(string message) : base(message) { }
}
