namespace VideoClubRental;

public class RegistrationRejectedException : Exception
{
    public RegistrationRejectedException(string message) : base(message) { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}

public class NoCopiesAvailableException : Exception
{
    public NoCopiesAvailableException(string message) : base(message) { }
}

public class OverdueRentalException : Exception
{
    public OverdueRentalException(string message) : base(message) { }
}

public class NoActiveRentalException : Exception
{
    public NoActiveRentalException(string message) : base(message) { }
}

public class TitleNotInCatalogException : Exception
{
    public TitleNotInCatalogException(string message) : base(message) { }
}
