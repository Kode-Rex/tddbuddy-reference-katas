using System.Globalization;

namespace CsvQuery;

public class Query
{
    private readonly CsvTable _table;
    private List<Row> _rows;

    public Query(CsvTable table)
    {
        _table = table;
        _rows = new List<Row>(table.Rows);
    }

    public Query Select(params string[] columns)
    {
        foreach (var col in columns)
            _table.ValidateColumn(col);

        _rows = _rows.Select(r => r.Project(columns)).ToList();
        return this;
    }

    public Query Where(string column, string op, string value)
    {
        _table.ValidateColumn(column);
        _rows = _rows.Where(r => Evaluate(r[column], op, value)).ToList();
        return this;
    }

    public Query OrderBy(string column, string direction)
    {
        _table.ValidateColumn(column);
        var ascending = direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

        _rows = _rows.OrderBy(r => r[column], new SmartComparer(ascending)).ToList();
        return this;
    }

    public Query Limit(int n)
    {
        _rows = _rows.Take(n).ToList();
        return this;
    }

    public int Count() => _rows.Count;

    public IReadOnlyList<Row> Rows => _rows;

    private static bool Evaluate(string cellValue, string op, string filterValue)
    {
        if (IsNumeric(cellValue) && IsNumeric(filterValue))
        {
            var cell = double.Parse(cellValue, CultureInfo.InvariantCulture);
            var filter = double.Parse(filterValue, CultureInfo.InvariantCulture);
            return op switch
            {
                "=" => cell == filter,
                "!=" => cell != filter,
                ">" => cell > filter,
                "<" => cell < filter,
                ">=" => cell >= filter,
                "<=" => cell <= filter,
                _ => false
            };
        }

        var cmp = string.Compare(cellValue, filterValue, StringComparison.Ordinal);
        return op switch
        {
            "=" => cmp == 0,
            "!=" => cmp != 0,
            ">" => cmp > 0,
            "<" => cmp < 0,
            ">=" => cmp >= 0,
            "<=" => cmp <= 0,
            _ => false
        };
    }

    private static bool IsNumeric(string value) =>
        double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);

    private class SmartComparer : IComparer<string>
    {
        private readonly int _direction;

        public SmartComparer(bool ascending)
        {
            _direction = ascending ? 1 : -1;
        }

        public int Compare(string? x, string? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1 * _direction;
            if (y == null) return 1 * _direction;

            if (IsNumeric(x) && IsNumeric(y))
            {
                var nx = double.Parse(x, CultureInfo.InvariantCulture);
                var ny = double.Parse(y, CultureInfo.InvariantCulture);
                return nx.CompareTo(ny) * _direction;
            }

            return string.Compare(x, y, StringComparison.Ordinal) * _direction;
        }
    }
}
