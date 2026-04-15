using FluentAssertions;
using Xunit;

namespace FluentCalc.Tests;

public class CalculatorTests
{
    [Fact]
    public void A_new_calculators_result_is_zero()
    {
        new Calculator().Result().Should().Be(0);
    }

    [Fact]
    public void Seeding_sets_the_starting_value()
    {
        new Calculator().Seed(10).Result().Should().Be(10);
    }

    [Fact]
    public void Plus_adds_to_the_seeded_value()
    {
        new Calculator().Seed(10).Plus(5).Result().Should().Be(15);
    }

    [Fact]
    public void Minus_subtracts_from_the_seeded_value()
    {
        new Calculator().Seed(10).Minus(4).Result().Should().Be(6);
    }

    [Fact]
    public void Operations_chain_in_order()
    {
        new Calculator().Seed(10).Plus(5).Plus(5).Result().Should().Be(20);
    }

    [Fact]
    public void Subsequent_Seed_calls_are_ignored()
    {
        new Calculator().Seed(10).Seed(99).Plus(5).Result().Should().Be(15);
    }

    [Fact]
    public void Plus_before_Seed_is_ignored()
    {
        new Calculator().Plus(5).Seed(10).Result().Should().Be(10);
    }

    [Fact]
    public void Undo_reverses_the_most_recent_operation()
    {
        new Calculator().Seed(10).Plus(5).Undo().Result().Should().Be(10);
    }

    [Fact]
    public void Undo_twice_reverses_two_operations()
    {
        new Calculator().Seed(10).Plus(5).Minus(2).Undo().Undo().Result().Should().Be(10);
    }

    [Fact]
    public void Undo_with_nothing_to_undo_is_a_no_op()
    {
        new Calculator().Undo().Result().Should().Be(0);
        new Calculator().Seed(10).Undo().Undo().Result().Should().Be(10);
    }

    [Fact]
    public void Redo_replays_the_most_recently_undone_operation()
    {
        new Calculator()
            .Seed(10).Plus(5).Minus(2).Undo().Undo().Redo()
            .Result().Should().Be(15);
    }

    [Fact]
    public void Redo_with_nothing_to_redo_is_a_no_op()
    {
        new Calculator().Seed(10).Plus(5).Redo().Result().Should().Be(15);
    }

    [Fact]
    public void A_new_operation_after_undo_clears_the_redo_stack()
    {
        new Calculator()
            .Seed(10).Plus(5).Undo().Plus(3).Redo()
            .Result().Should().Be(13);
    }

    [Fact]
    public void The_full_undo_redo_example_from_the_spec()
    {
        new Calculator()
            .Seed(10).Plus(5).Minus(2).Undo().Undo().Redo()
            .Result().Should().Be(15);
    }

    [Fact]
    public void Save_clears_history_so_Undo_has_no_effect()
    {
        new Calculator()
            .Seed(10).Plus(5).Minus(2).Save().Undo().Redo().Undo().Plus(5)
            .Result().Should().Be(18);
    }
}
