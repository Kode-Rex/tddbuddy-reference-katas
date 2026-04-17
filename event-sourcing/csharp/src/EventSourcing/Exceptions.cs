namespace EventSourcing;

public class AccountNotOpenException : Exception
{
    public AccountNotOpenException(string message) : base(message) { }
}

public class AccountClosedException : Exception
{
    public AccountClosedException(string message) : base(message) { }
}

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message) : base(message) { }
}

public class InvalidAmountException : Exception
{
    public InvalidAmountException(string message) : base(message) { }
}

public class NonZeroBalanceException : Exception
{
    public NonZeroBalanceException(string message) : base(message) { }
}
