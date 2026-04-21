using FluentAssertions;
using RollYourOwnTestFramework;
using Xunit;

namespace RollYourOwnTestFramework.Tests;

public class AssertionsTests
{
    // --- assertEqual ---

    [Fact]
    public void Assert_equal_with_equal_values_passes()
    {
        var act = () => Assertions.AssertEqual(5, 5);

        act.Should().NotThrow();
    }

    [Fact]
    public void Assert_equal_with_different_values_fails()
    {
        var act = () => Assertions.AssertEqual(5, 3);

        act.Should().Throw<AssertionFailedException>()
            .WithMessage("expected 5 but got 3");
    }

    // --- assertTrue ---

    [Fact]
    public void Assert_true_with_true_passes()
    {
        var act = () => Assertions.AssertTrue(true);

        act.Should().NotThrow();
    }

    [Fact]
    public void Assert_true_with_false_fails()
    {
        var act = () => Assertions.AssertTrue(false);

        act.Should().Throw<AssertionFailedException>()
            .WithMessage("expected true");
    }

    // --- assertThrows ---

    [Fact]
    public void Assert_throws_with_throwing_function_passes()
    {
        var act = () => Assertions.AssertThrows(() => throw new InvalidOperationException());

        act.Should().NotThrow();
    }

    [Fact]
    public void Assert_throws_with_non_throwing_function_fails()
    {
        var act = () => Assertions.AssertThrows(() => { });

        act.Should().Throw<AssertionFailedException>()
            .WithMessage("expected exception");
    }
}
