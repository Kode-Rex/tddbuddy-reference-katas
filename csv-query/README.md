# CSV Query

Data-processing kata: parse CSV input, then query it with a chainable fluent API supporting select, where, orderBy, limit, and count. Excellent for practicing **test data builders** over a multi-step query pipeline with rich comparison logic.

## What this kata teaches

- **Test Data Builders** — `RowBuilder` constructs row dictionaries with defaults; `QueryBuilder` pre-loads a CsvTable so each test starts one line from the interesting part.
- **Domain Exceptions** — `UnknownColumnException` names the rejection; tests assert on the domain concept, not on a generic error type.
- **Chainable Fluent API** — the Query object returns itself from each operation, so callers compose pipelines naturally: `query.where(...).orderBy(...).limit(3).rows()`.
- **Numeric vs. String Comparison** — the where clause auto-detects numeric values at comparison time, keeping stored data as strings.
- **Quoted-Field Parsing** — CSV fields wrapped in double quotes may contain commas; the parser strips quotes and preserves the inner value.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
