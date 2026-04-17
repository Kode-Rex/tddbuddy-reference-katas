using FluentAssertions;
using Xunit;

namespace CsvQuery.Tests;

public class CsvQueryTests
{
    // --- Parsing ---

    [Fact]
    public void Parsing_csv_produces_rows_with_correct_column_values()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Rows;

        rows[0]["name"].Should().Be("Alice");
        rows[0]["age"].Should().Be("35");
        rows[0]["city"].Should().Be("London");
        rows[0]["salary"].Should().Be("75000");
    }

    [Fact]
    public void Parsing_csv_with_only_a_header_row_produces_zero_rows()
    {
        var query = new QueryBuilder().WithCsv("name,age,city,salary").Build();

        query.Rows.Should().BeEmpty();
    }

    [Fact]
    public void Parsing_csv_with_a_single_data_row_produces_one_row()
    {
        var query = new QueryBuilder().WithCsv("name,age\nAlice,35").Build();

        query.Rows.Should().HaveCount(1);
        query.Rows[0]["name"].Should().Be("Alice");
    }

    [Fact]
    public void Parsing_quoted_fields_strips_quotes_and_preserves_commas()
    {
        var query = new QueryBuilder()
            .WithCsv("name,age,city\n\"Smith, Jr.\",45,London")
            .Build();

        query.Rows[0]["name"].Should().Be("Smith, Jr.");
    }

    // --- Select ---

    [Fact]
    public void Selecting_a_single_column_returns_only_that_column()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Select("name").Rows;

        rows[0].Values.Keys.Should().BeEquivalentTo(new[] { "name" });
        rows[0]["name"].Should().Be("Alice");
        rows[4]["name"].Should().Be("Eve");
    }

    [Fact]
    public void Selecting_multiple_columns_returns_them_in_requested_order()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Select("city", "name").Rows;

        rows[0].Values.Keys.Should().ContainInOrder("city", "name");
        rows[0]["city"].Should().Be("London");
        rows[0]["name"].Should().Be("Alice");
    }

    [Fact]
    public void Selecting_an_unknown_column_raises_UnknownColumnException()
    {
        var query = new QueryBuilder().Build();

        var act = () => query.Select("invalid_column");

        act.Should().Throw<UnknownColumnException>()
            .WithMessage("Unknown column: invalid_column");
    }

    // --- Where — equality and inequality ---

    [Fact]
    public void Where_equal_filters_to_matching_rows()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("city", "=", "London").Rows;

        rows.Should().HaveCount(2);
        rows[0]["name"].Should().Be("Alice");
        rows[1]["name"].Should().Be("Charlie");
    }

    [Fact]
    public void Where_not_equal_excludes_matching_rows()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("city", "!=", "London").Rows;

        rows.Should().HaveCount(3);
        rows.Select(r => r["name"]).Should().BeEquivalentTo("Bob", "Diana", "Eve");
    }

    [Fact]
    public void Where_on_a_value_with_no_matches_returns_empty()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("city", "=", "Tokyo").Rows;

        rows.Should().BeEmpty();
    }

    // --- Where — numeric comparisons ---

    [Fact]
    public void Where_greater_than_compares_numerically()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("age", ">", "30").Rows;

        rows.Should().HaveCount(3);
        rows.Select(r => r["name"]).Should().BeEquivalentTo("Alice", "Charlie", "Diana");
    }

    [Fact]
    public void Where_less_than_compares_numerically()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("age", "<", "30").Rows;

        rows.Should().HaveCount(2);
        rows.Select(r => r["name"]).Should().BeEquivalentTo("Bob", "Eve");
    }

    [Fact]
    public void Where_greater_than_or_equal_includes_the_boundary()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("age", ">=", "35").Rows;

        rows.Should().HaveCount(2);
        rows.Select(r => r["name"]).Should().BeEquivalentTo("Alice", "Charlie");
    }

    [Fact]
    public void Where_less_than_or_equal_includes_the_boundary()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("age", "<=", "28").Rows;

        rows.Should().HaveCount(2);
        rows.Select(r => r["name"]).Should().BeEquivalentTo("Bob", "Eve");
    }

    // --- Where — string fallback ---

    [Fact]
    public void Where_compares_as_strings_when_values_are_non_numeric()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("city", ">", "London").Rows;

        rows.Should().HaveCount(2);
        rows.Select(r => r["name"]).Should().BeEquivalentTo("Bob", "Eve");
    }

    // --- OrderBy ---

    [Fact]
    public void OrderBy_ascending_sorts_numerically()
    {
        var query = new QueryBuilder().Build();

        var rows = query.OrderBy("age", "asc").Rows;

        rows.Select(r => r["name"]).Should()
            .ContainInOrder("Bob", "Eve", "Diana", "Alice", "Charlie");
    }

    [Fact]
    public void OrderBy_descending_sorts_numerically()
    {
        var query = new QueryBuilder().Build();

        var rows = query.OrderBy("salary", "desc").Rows;

        rows.Select(r => r["name"]).Should()
            .ContainInOrder("Charlie", "Alice", "Diana", "Eve", "Bob");
    }

    [Fact]
    public void OrderBy_sorts_strings_lexicographically()
    {
        var query = new QueryBuilder().Build();

        var rows = query.OrderBy("name", "asc").Rows;

        rows.Select(r => r["name"]).Should()
            .ContainInOrder("Alice", "Bob", "Charlie", "Diana", "Eve");
    }

    // --- Limit ---

    [Fact]
    public void Limit_restricts_the_result_set_to_N_rows()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Limit(2).Rows;

        rows.Should().HaveCount(2);
        rows[0]["name"].Should().Be("Alice");
        rows[1]["name"].Should().Be("Bob");
    }

    [Fact]
    public void Limit_larger_than_row_count_returns_all_rows()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Limit(100).Rows;

        rows.Should().HaveCount(5);
    }

    // --- Count ---

    [Fact]
    public void Count_returns_the_total_number_of_rows()
    {
        var query = new QueryBuilder().Build();

        query.Count().Should().Be(5);
    }

    [Fact]
    public void Count_after_where_returns_the_filtered_count()
    {
        var query = new QueryBuilder().Build();

        query.Where("city", "=", "Paris").Count().Should().Be(2);
    }

    // --- Chaining ---

    [Fact]
    public void Where_then_select_returns_filtered_projected_rows()
    {
        var query = new QueryBuilder().Build();

        var rows = query.Where("age", ">", "30").Select("name").Rows;

        rows.Should().HaveCount(3);
        rows.Select(r => r["name"]).Should().BeEquivalentTo("Alice", "Charlie", "Diana");
    }

    [Fact]
    public void Where_then_orderBy_then_limit_chains_correctly()
    {
        var query = new QueryBuilder().Build();

        var rows = query
            .Where("age", ">=", "35")
            .OrderBy("salary", "desc")
            .Limit(1)
            .Rows;

        rows.Should().HaveCount(1);
        rows[0]["name"].Should().Be("Charlie");
    }

    [Fact]
    public void Where_then_count_returns_zero_when_no_rows_match()
    {
        var query = new QueryBuilder().Build();

        query.Where("city", "=", "Tokyo").Count().Should().Be(0);
    }
}
