namespace CsvQuery;

public class Row
{
    private readonly Dictionary<string, string> _values;

    public Row(Dictionary<string, string> values)
    {
        _values = new Dictionary<string, string>(values);
    }

    public string this[string column] =>
        _values.TryGetValue(column, out var value)
            ? value
            : throw new UnknownColumnException(column);

    public IReadOnlyDictionary<string, string> Values => _values;

    public Row Project(IReadOnlyList<string> columns)
    {
        var projected = new Dictionary<string, string>();
        foreach (var col in columns)
        {
            projected[col] = this[col];
        }
        return new Row(projected);
    }
}
