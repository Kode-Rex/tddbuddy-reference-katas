namespace LibraryManagement;

public class BookNotInCatalogException : Exception
{
    public BookNotInCatalogException(string message) : base(message) { }
}

public class NoCopiesAvailableException : Exception
{
    public NoCopiesAvailableException(string message) : base(message) { }
}

public class NoActiveLoanException : Exception
{
    public NoActiveLoanException(string message) : base(message) { }
}
