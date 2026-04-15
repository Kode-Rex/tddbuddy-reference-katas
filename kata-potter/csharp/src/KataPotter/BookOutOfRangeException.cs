namespace KataPotter;

public class BookOutOfRangeException : ArgumentOutOfRangeException
{
    public BookOutOfRangeException() : base(null, BasketMessages.BookOutOfRange) { }
}
