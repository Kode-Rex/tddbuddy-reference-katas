import pytest

from csv_query import UnknownColumnError

from .query_builder import QueryBuilder


# --- Parsing ---


def test_parsing_csv_produces_rows_with_correct_column_values():
    query = QueryBuilder().build()
    rows = query.rows
    assert rows[0].get("name") == "Alice"
    assert rows[0].get("age") == "35"
    assert rows[0].get("city") == "London"
    assert rows[0].get("salary") == "75000"


def test_parsing_csv_with_only_a_header_row_produces_zero_rows():
    query = QueryBuilder().with_csv("name,age,city,salary").build()
    assert len(query.rows) == 0


def test_parsing_csv_with_a_single_data_row_produces_one_row():
    query = QueryBuilder().with_csv("name,age\nAlice,35").build()
    assert len(query.rows) == 1
    assert query.rows[0].get("name") == "Alice"


def test_parsing_quoted_fields_strips_quotes_and_preserves_commas():
    query = QueryBuilder().with_csv('name,age,city\n"Smith, Jr.",45,London').build()
    assert query.rows[0].get("name") == "Smith, Jr."


# --- Select ---


def test_selecting_a_single_column_returns_only_that_column():
    query = QueryBuilder().build()
    rows = query.select("name").rows
    assert rows[0].columns == ["name"]
    assert rows[0].get("name") == "Alice"
    assert rows[4].get("name") == "Eve"


def test_selecting_multiple_columns_returns_them_in_requested_order():
    query = QueryBuilder().build()
    rows = query.select("city", "name").rows
    assert rows[0].columns == ["city", "name"]
    assert rows[0].get("city") == "London"
    assert rows[0].get("name") == "Alice"


def test_selecting_an_unknown_column_raises_unknown_column_error():
    query = QueryBuilder().build()
    with pytest.raises(UnknownColumnError, match="Unknown column: invalid_column"):
        query.select("invalid_column")


# --- Where — equality and inequality ---


def test_where_equal_filters_to_matching_rows():
    query = QueryBuilder().build()
    rows = query.where("city", "=", "London").rows
    assert len(rows) == 2
    assert rows[0].get("name") == "Alice"
    assert rows[1].get("name") == "Charlie"


def test_where_not_equal_excludes_matching_rows():
    query = QueryBuilder().build()
    rows = query.where("city", "!=", "London").rows
    assert len(rows) == 3
    assert [r.get("name") for r in rows] == ["Bob", "Diana", "Eve"]


def test_where_on_a_value_with_no_matches_returns_empty():
    query = QueryBuilder().build()
    rows = query.where("city", "=", "Tokyo").rows
    assert len(rows) == 0


# --- Where — numeric comparisons ---


def test_where_greater_than_compares_numerically():
    query = QueryBuilder().build()
    rows = query.where("age", ">", "30").rows
    assert len(rows) == 3
    assert [r.get("name") for r in rows] == ["Alice", "Charlie", "Diana"]


def test_where_less_than_compares_numerically():
    query = QueryBuilder().build()
    rows = query.where("age", "<", "30").rows
    assert len(rows) == 2
    assert [r.get("name") for r in rows] == ["Bob", "Eve"]


def test_where_greater_than_or_equal_includes_the_boundary():
    query = QueryBuilder().build()
    rows = query.where("age", ">=", "35").rows
    assert len(rows) == 2
    assert [r.get("name") for r in rows] == ["Alice", "Charlie"]


def test_where_less_than_or_equal_includes_the_boundary():
    query = QueryBuilder().build()
    rows = query.where("age", "<=", "28").rows
    assert len(rows) == 2
    assert [r.get("name") for r in rows] == ["Bob", "Eve"]


# --- Where — string fallback ---


def test_where_compares_as_strings_when_values_are_non_numeric():
    query = QueryBuilder().build()
    rows = query.where("city", ">", "London").rows
    assert len(rows) == 2
    assert [r.get("name") for r in rows] == ["Bob", "Eve"]


# --- OrderBy ---


def test_order_by_ascending_sorts_numerically():
    query = QueryBuilder().build()
    rows = query.order_by("age", "asc").rows
    assert [r.get("name") for r in rows] == ["Bob", "Eve", "Diana", "Alice", "Charlie"]


def test_order_by_descending_sorts_numerically():
    query = QueryBuilder().build()
    rows = query.order_by("salary", "desc").rows
    assert [r.get("name") for r in rows] == ["Charlie", "Alice", "Diana", "Eve", "Bob"]


def test_order_by_sorts_strings_lexicographically():
    query = QueryBuilder().build()
    rows = query.order_by("name", "asc").rows
    assert [r.get("name") for r in rows] == ["Alice", "Bob", "Charlie", "Diana", "Eve"]


# --- Limit ---


def test_limit_restricts_the_result_set_to_n_rows():
    query = QueryBuilder().build()
    rows = query.limit(2).rows
    assert len(rows) == 2
    assert rows[0].get("name") == "Alice"
    assert rows[1].get("name") == "Bob"


def test_limit_larger_than_row_count_returns_all_rows():
    query = QueryBuilder().build()
    rows = query.limit(100).rows
    assert len(rows) == 5


# --- Count ---


def test_count_returns_the_total_number_of_rows():
    query = QueryBuilder().build()
    assert query.count() == 5


def test_count_after_where_returns_the_filtered_count():
    query = QueryBuilder().build()
    assert query.where("city", "=", "Paris").count() == 2


# --- Chaining ---


def test_where_then_select_returns_filtered_projected_rows():
    query = QueryBuilder().build()
    rows = query.where("age", ">", "30").select("name").rows
    assert len(rows) == 3
    assert [r.get("name") for r in rows] == ["Alice", "Charlie", "Diana"]


def test_where_then_order_by_then_limit_chains_correctly():
    query = QueryBuilder().build()
    rows = query.where("age", ">=", "35").order_by("salary", "desc").limit(1).rows
    assert len(rows) == 1
    assert rows[0].get("name") == "Charlie"


def test_where_then_count_returns_zero_when_no_rows_match():
    query = QueryBuilder().build()
    assert query.where("city", "=", "Tokyo").count() == 0
