namespace CsvQuery.Tests;

public class RowBuilder
{
    private string _name = "Alice";
    private string _age = "35";
    private string _city = "London";
    private string _salary = "75000";

    public RowBuilder WithName(string name) { _name = name; return this; }
    public RowBuilder WithAge(string age) { _age = age; return this; }
    public RowBuilder WithCity(string city) { _city = city; return this; }
    public RowBuilder WithSalary(string salary) { _salary = salary; return this; }

    public Row Build()
    {
        return new Row(new Dictionary<string, string>
        {
            ["name"] = _name,
            ["age"] = _age,
            ["city"] = _city,
            ["salary"] = _salary
        });
    }
}
