namespace CsvQuery;

public class CsvTable
{
    public IReadOnlyList<string> Headers { get; }
    public IReadOnlyList<Row> Rows { get; }

    public CsvTable(IReadOnlyList<string> headers, IReadOnlyList<Row> rows)
    {
        Headers = headers;
        Rows = rows;
    }

    public void ValidateColumn(string column)
    {
        if (!Headers.Contains(column))
            throw new UnknownColumnException(column);
    }
}
