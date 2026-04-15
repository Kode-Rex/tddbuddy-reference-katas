from string_calculator.calculator import add


def test_empty_string_returns_zero():
    assert add("") == 0
