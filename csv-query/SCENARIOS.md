# CSV Query — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **CsvTable** | Parsed representation of CSV data — a header row plus zero or more data rows |
| **Row** | A single data record; a dictionary from column name to string value |
| **Column** | A named field in the header; values are strings that may represent numbers |
| **Query** | A chainable pipeline of operations (select, where, orderBy, limit) applied to a CsvTable |
| **Operator** | A comparison function used in where clauses: `=`, `!=`, `>`, `<`, `>=`, `<=` |
| **RowBuilder** | Test helper that constructs Row dictionaries with sensible defaults |
| **QueryBuilder** | Test helper that constructs a Query pre-loaded with sample CSV data |

## Domain Rules

- A **CsvTable** is constructed from a raw CSV string; the first line is always the header
- Column names are **case-sensitive** and preserve whitespace
- Values are always stored as **strings**; numeric comparisons parse to numbers at comparison time
- **Select** projects specific columns; selecting a column that does not exist in the header raises `UnknownColumnException` with message `"Unknown column: <name>"`
- **Where** filters rows by applying an operator; the column must exist (same exception)
- Numeric comparison is used when **both** the cell value and the filter value parse as numbers; otherwise string comparison applies
- **OrderBy** sorts rows; the column must exist; direction is `"asc"` or `"desc"`; numeric values sort numerically, strings lexicographically
- **Limit** restricts the result set to the first N rows; N must be positive
- **Count** returns the number of rows in the current result set (after all filters)
- Operations are **chainable** and applied in pipeline order
- Quoted fields: a field wrapped in double quotes may contain commas; the quotes are stripped from the value
- An empty CSV (header only) produces zero rows; queries over it return empty results or count 0

## Test Scenarios

### Parsing

1. **Parsing CSV produces rows with correct column values**
2. **Parsing CSV with only a header row produces zero rows**
3. **Parsing CSV with a single data row produces one row**
4. **Parsing quoted fields strips quotes and preserves commas**

### Select

5. **Selecting a single column returns only that column**
6. **Selecting multiple columns returns them in requested order**
7. **Selecting an unknown column raises UnknownColumnException**

### Where — equality and inequality

8. **Where equal filters to matching rows**
9. **Where not-equal excludes matching rows**
10. **Where on a value with no matches returns empty**

### Where — numeric comparisons

11. **Where greater-than compares numerically**
12. **Where less-than compares numerically**
13. **Where greater-than-or-equal includes the boundary**
14. **Where less-than-or-equal includes the boundary**

### Where — string fallback

15. **Where compares as strings when values are non-numeric**

### OrderBy

16. **OrderBy ascending sorts numerically**
17. **OrderBy descending sorts numerically**
18. **OrderBy sorts strings lexicographically**

### Limit

19. **Limit restricts the result set to N rows**
20. **Limit larger than row count returns all rows**

### Count

21. **Count returns the total number of rows**
22. **Count after where returns the filtered count**

### Chaining

23. **Where then select returns filtered projected rows**
24. **Where then orderBy then limit chains correctly**
25. **Where then count returns zero when no rows match**
