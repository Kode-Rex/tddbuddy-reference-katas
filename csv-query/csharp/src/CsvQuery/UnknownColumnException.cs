namespace CsvQuery;

public class UnknownColumnException : Exception
{
    public UnknownColumnException(string columnName)
        : base($"Unknown column: {columnName}") { }
}
