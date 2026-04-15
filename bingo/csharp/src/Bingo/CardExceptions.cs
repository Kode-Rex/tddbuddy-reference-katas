namespace Bingo;

public class NumberOutOfRangeException : ArgumentOutOfRangeException
{
    public NumberOutOfRangeException() : base(null, CardMessages.NumberOutOfRange) { }
}
