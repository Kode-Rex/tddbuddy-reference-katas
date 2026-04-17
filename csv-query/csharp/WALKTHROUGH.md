# CSV Query — C# Walkthrough

This kata ships in **middle/high gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

## The Design at a Glance

```
CsvParser.Parse(string) ──> CsvTable(headers, rows)
                                │
Query(CsvTable)                 │
  ├── Select(columns) : Query   │
  ├── Where(col, op, val) : Query
  ├── OrderBy(col, dir) : Query │
  ├── Limit(n) : Query          │
  ├── Count() : int             │
  └── Rows : IReadOnlyList<Row> │

Row ── dictionary from column name to string value
UnknownColumnException ── domain rejection
```

Five pieces. Each earns its keep.

## Why `Row` Is a Type, Not a `Dictionary<string, string>`

A bare dictionary doesn't reject unknown columns — it returns `null` or throws a `KeyNotFoundException`. `Row` wraps the dictionary and throws `UnknownColumnException` with a domain-meaningful message. Tests assert on the domain rejection, not on a framework accident.

`Row.Project(columns)` returns a new Row with only the requested columns. This keeps the projection logic on the entity that owns the data, not scattered across the Query.

## Why `CsvTable` Validates Columns

Both `Query.Select` and `Query.Where` need to reject unknown columns. Rather than duplicating the check, `CsvTable.ValidateColumn` is the single authority on whether a column exists. The table owns the header; it should own the validation.

## Why `Query` Mutates a List (Not LINQ Chains)

Each operation (`Where`, `OrderBy`, `Limit`) mutates the internal row list and returns `this`. This is the simplest model for a chainable pipeline: the caller writes `query.Where(...).OrderBy(...).Limit(3).Rows` and the operations apply in call order.

An immutable-chain alternative would create a new Query per step. That's defensible but adds allocation for no behavioral gain — the query is used once, in one direction, then discarded.

## Why Numeric Detection Lives at Comparison Time

Values are stored as strings. At `Where` or `OrderBy` time, both operands are tested with `double.TryParse`. If both parse, the comparison is numeric; otherwise it falls back to `string.Compare`. This matches the spec requirement: "numeric comparisons for numeric values, string comparisons otherwise."

An alternative would be to detect types at parse time and store typed cells. That adds complexity (a union type or object boxing) for no benefit — the only place types matter is comparison.

## Why `UnknownColumnException` Instead of Returning Empty

The spec says `select("invalid_column")` should produce "Error or empty results." We chose error. An unknown column is a programming mistake — silently returning empty rows hides it. The domain exception names the problem precisely: `"Unknown column: invalid_column"`.

## Scenario Map

Twenty-five scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/CsvQuery.Tests/CsvQueryTests.cs`, one `[Fact]` per scenario.

## How to Run

```bash
cd csv-query/csharp
dotnet test
```
