namespace CsvQuery;

public static class CsvParser
{
    public static CsvTable Parse(string csv)
    {
        var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
            return new CsvTable(Array.Empty<string>(), new List<Row>());

        var headers = ParseFields(lines[0]);
        var rows = new List<Row>();

        for (var i = 1; i < lines.Length; i++)
        {
            var fields = ParseFields(lines[i]);
            var values = new Dictionary<string, string>();
            for (var j = 0; j < headers.Length; j++)
            {
                values[headers[j]] = j < fields.Length ? fields[j] : "";
            }
            rows.Add(new Row(values));
        }

        return new CsvTable(headers, rows);
    }

    private static string[] ParseFields(string line)
    {
        var fields = new List<string>();
        var current = "";
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];
            if (ch == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (ch == ',' && !inQuotes)
            {
                fields.Add(current.Trim());
                current = "";
            }
            else
            {
                current += ch;
            }
        }

        fields.Add(current.Trim());
        return fields.ToArray();
    }
}
