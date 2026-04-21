using FluentAssertions;
using RollYourOwnMockFramework;
using Xunit;
using static RollYourOwnMockFramework.MockApi;

namespace RollYourOwnMockFramework.Tests;

public class MockTests
{
    // --- Mock Creation ---

    [Fact]
    public void Create_mock_methods_are_callable_without_error()
    {
        dynamic mock = CreateMock();

        var act = () => mock.add(2, 3);

        act.Should().NotThrow();
    }

    [Fact]
    public void Unstubbed_method_returns_null()
    {
        dynamic mock = CreateMock();

        object? result = mock.add(2, 3);

        result.Should().BeNull();
    }

    // --- Stub Configuration ---

    [Fact]
    public void Stub_return_value_for_matching_args()
    {
        var mock = CreateMock();
        ((StubConfiguration)When(mock).add(2, 3)).ThenReturn(5);

        object? result = ((dynamic)mock).add(2, 3);

        result.Should().Be(5);
    }

    [Fact]
    public void Stub_different_arg_sets_return_their_own_values()
    {
        var mock = CreateMock();
        ((StubConfiguration)When(mock).add(2, 3)).ThenReturn(5);
        ((StubConfiguration)When(mock).add(1, 1)).ThenReturn(2);

        object? result1 = ((dynamic)mock).add(2, 3);
        object? result2 = ((dynamic)mock).add(1, 1);

        result1.Should().Be(5);
        result2.Should().Be(2);
    }

    [Fact]
    public void Unstubbed_args_return_null_even_when_other_args_are_stubbed()
    {
        var mock = CreateMock();
        ((StubConfiguration)When(mock).add(2, 3)).ThenReturn(5);

        object? result = ((dynamic)mock).add(9, 9);

        result.Should().BeNull();
    }

    // --- Verification — wasCalled ---

    [Fact]
    public void Verify_called_method_passes()
    {
        var mock = CreateMock();
        ((dynamic)mock).add(2, 3);

        var act = () => ((MethodVerification)Verify(mock).add).WasCalled();

        act.Should().NotThrow();
    }

    [Fact]
    public void Verify_uncalled_method_fails_with_message()
    {
        var mock = CreateMock();

        var act = () => ((MethodVerification)Verify(mock).add).WasCalled();

        act.Should().Throw<VerificationError>()
            .WithMessage("expected add to be called but was never called");
    }

    // --- Verification — wasCalledWith ---

    [Fact]
    public void Verify_called_with_correct_args_passes()
    {
        var mock = CreateMock();
        ((dynamic)mock).add(2, 3);

        var act = () => ((MethodVerification)Verify(mock).add).WasCalledWith(2, 3);

        act.Should().NotThrow();
    }

    [Fact]
    public void Verify_called_with_wrong_args_fails_with_message()
    {
        var mock = CreateMock();
        ((dynamic)mock).add(2, 3);

        var act = () => ((MethodVerification)Verify(mock).add).WasCalledWith(1, 1);

        act.Should().Throw<VerificationError>()
            .WithMessage("expected add(1, 1) to be called but was called with (2, 3)");
    }

    [Fact]
    public void Verify_was_called_with_on_uncalled_method_fails()
    {
        var mock = CreateMock();

        var act = () => ((MethodVerification)Verify(mock).add).WasCalledWith(1, 1);

        act.Should().Throw<VerificationError>()
            .WithMessage("expected add(1, 1) to be called but was never called");
    }

    // --- Verification — wasCalledTimes ---

    [Fact]
    public void Verify_call_count_matches()
    {
        var mock = CreateMock();
        ((dynamic)mock).add(2, 3);
        ((dynamic)mock).add(2, 3);

        var act = () => ((MethodVerification)Verify(mock).add).WasCalledTimes(2);

        act.Should().NotThrow();
    }

    [Fact]
    public void Verify_call_count_mismatch_fails_with_message()
    {
        var mock = CreateMock();
        ((dynamic)mock).add(2, 3);

        var act = () => ((MethodVerification)Verify(mock).add).WasCalledTimes(2);

        act.Should().Throw<VerificationError>()
            .WithMessage("expected add to be called 2 times but was called 1 times");
    }
}
