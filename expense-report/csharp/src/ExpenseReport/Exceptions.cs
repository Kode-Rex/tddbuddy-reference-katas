namespace ExpenseReport;

public class EmptyReportException : Exception
{
    public EmptyReportException() : base("Cannot submit an empty report") { }
}

public class ReportExceedsMaximumException : Exception
{
    public ReportExceedsMaximumException() : base("Report total exceeds the $5,000 maximum") { }
}

public class InvalidStatusTransitionException : Exception
{
    public InvalidStatusTransitionException(string message) : base(message) { }
}

public class ExpenseNotFoundException : Exception
{
    public ExpenseNotFoundException() : base("Expense not found") { }
}

public class InvalidAmountException : Exception
{
    public InvalidAmountException() : base("Amount must be positive") { }
}

public class FinalizedReportException : Exception
{
    public FinalizedReportException() : base("Cannot modify a finalized report") { }
}
