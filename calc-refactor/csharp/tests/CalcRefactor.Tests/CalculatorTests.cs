using FluentAssertions;
using Xunit;
using static CalcRefactor.Tests.CalculatorBuilder;

namespace CalcRefactor.Tests;

public class CalculatorTests
{
    [Fact]
    public void A_fresh_calculator_displays_zero()
    {
        ACalculator().Build().Display.Should().Be("0");
    }

    [Fact]
    public void Pressing_a_single_digit_displays_that_digit()
    {
        ACalculator().PressKeys("7").Build().Display.Should().Be("7");
    }

    [Fact]
    public void Pressing_multiple_digits_builds_a_multi_digit_operand()
    {
        ACalculator().PressKeys("123").Build().Display.Should().Be("123");
    }

    [Fact]
    public void Leading_zero_is_replaced_by_the_first_non_zero_digit()
    {
        ACalculator().PressKeys("05").Build().Display.Should().Be("5");
    }

    [Fact]
    public void Addition_of_two_operands()
    {
        ACalculator().PressKeys("1+2=").Build().Display.Should().Be("3");
    }

    [Fact]
    public void Subtraction_of_two_operands()
    {
        ACalculator().PressKeys("9-4=").Build().Display.Should().Be("5");
    }

    [Fact]
    public void Multiplication_of_two_operands()
    {
        ACalculator().PressKeys("6*7=").Build().Display.Should().Be("42");
    }

    [Fact]
    public void Integer_division_truncates_toward_zero()
    {
        ACalculator().PressKeys("7/2=").Build().Display.Should().Be("3");
    }

    [Fact]
    public void Division_by_zero_enters_the_error_state()
    {
        ACalculator().PressKeys("5/0=").Build().Display.Should().Be("Error");
    }

    [Fact]
    public void Consecutive_operators_enter_the_error_state()
    {
        ACalculator().PressKeys("1++2=").Build().Display.Should().Be("Error");
    }

    [Fact]
    public void Error_is_sticky_further_keys_are_ignored()
    {
        ACalculator().PressKeys("5/0=123+4=").Build().Display.Should().Be("Error");
    }

    [Fact]
    public void Clear_resets_from_the_error_state()
    {
        ACalculator().PressKeys("5/0=C").Build().Display.Should().Be("0");
    }

    [Fact]
    public void Clear_resets_from_a_normal_state()
    {
        ACalculator().PressKeys("42C").Build().Display.Should().Be("0");
    }

    [Fact]
    public void Equals_with_no_pending_operator_is_a_no_op()
    {
        ACalculator().PressKeys("42=").Build().Display.Should().Be("42");
    }

    [Fact]
    public void Repeated_equals_reapplies_the_last_operator_and_operand()
    {
        ACalculator().PressKeys("2+3==").Build().Display.Should().Be("8");
    }

    [Fact]
    public void Chained_operators_evaluate_left_to_right()
    {
        ACalculator().PressKeys("2+3*4=").Build().Display.Should().Be("20");
    }

    [Fact]
    public void Operator_after_equals_continues_from_the_result()
    {
        ACalculator().PressKeys("2+3=*4=").Build().Display.Should().Be("20");
    }

    [Fact]
    public void A_new_digit_after_equals_starts_a_fresh_calculation()
    {
        ACalculator().PressKeys("2+3=7").Build().Display.Should().Be("7");
        ACalculator().PressKeys("2+3=7+1=").Build().Display.Should().Be("8");
    }

    [Fact]
    public void Negative_results_display_with_a_leading_minus()
    {
        ACalculator().PressKeys("3-9=").Build().Display.Should().Be("-6");
    }

    [Fact]
    public void An_unknown_key_raises_an_argument_error()
    {
        var calculator = new Calculator();

        Action act = () => calculator.Press('x');

        act.Should().Throw<ArgumentException>().WithMessage("unknown key: x*");
    }
}
