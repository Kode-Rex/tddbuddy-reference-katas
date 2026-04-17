namespace CsvQuery.Tests;

public class QueryBuilder
{
    private string _csv = "name,age,city,salary\nAlice,35,London,75000\nBob,28,Paris,55000\nCharlie,42,London,90000\nDiana,31,Berlin,65000\nEve,28,Paris,60000";

    public QueryBuilder WithCsv(string csv) { _csv = csv; return this; }

    public Query Build()
    {
        var table = CsvParser.Parse(_csv);
        return new Query(table);
    }
}
