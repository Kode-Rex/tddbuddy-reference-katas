import pytest

from calc_refactor import Calculator
from tests.calculator_builder import a_calculator


def test_a_fresh_calculator_displays_zero():
    assert a_calculator().build().display == "0"


def test_pressing_a_single_digit_displays_that_digit():
    assert a_calculator().press_keys("7").build().display == "7"


def test_pressing_multiple_digits_builds_a_multi_digit_operand():
    assert a_calculator().press_keys("123").build().display == "123"


def test_leading_zero_is_replaced_by_the_first_non_zero_digit():
    assert a_calculator().press_keys("05").build().display == "5"


def test_addition_of_two_operands():
    assert a_calculator().press_keys("1+2=").build().display == "3"


def test_subtraction_of_two_operands():
    assert a_calculator().press_keys("9-4=").build().display == "5"


def test_multiplication_of_two_operands():
    assert a_calculator().press_keys("6*7=").build().display == "42"


def test_integer_division_truncates_toward_zero():
    assert a_calculator().press_keys("7/2=").build().display == "3"


def test_division_by_zero_enters_the_error_state():
    assert a_calculator().press_keys("5/0=").build().display == "Error"


def test_consecutive_operators_enter_the_error_state():
    assert a_calculator().press_keys("1++2=").build().display == "Error"


def test_error_is_sticky_further_keys_are_ignored():
    assert a_calculator().press_keys("5/0=123+4=").build().display == "Error"


def test_clear_resets_from_the_error_state():
    assert a_calculator().press_keys("5/0=C").build().display == "0"


def test_clear_resets_from_a_normal_state():
    assert a_calculator().press_keys("42C").build().display == "0"


def test_equals_with_no_pending_operator_is_a_no_op():
    assert a_calculator().press_keys("42=").build().display == "42"


def test_repeated_equals_reapplies_the_last_operator_and_operand():
    assert a_calculator().press_keys("2+3==").build().display == "8"


def test_chained_operators_evaluate_left_to_right():
    assert a_calculator().press_keys("2+3*4=").build().display == "20"


def test_operator_after_equals_continues_from_the_result():
    assert a_calculator().press_keys("2+3=*4=").build().display == "20"


def test_a_new_digit_after_equals_starts_a_fresh_calculation():
    assert a_calculator().press_keys("2+3=7").build().display == "7"
    assert a_calculator().press_keys("2+3=7+1=").build().display == "8"


def test_negative_results_display_with_a_leading_minus():
    assert a_calculator().press_keys("3-9=").build().display == "-6"


def test_an_unknown_key_raises_an_argument_error():
    calculator = Calculator()
    with pytest.raises(ValueError, match="unknown key: x"):
        calculator.press("x")
