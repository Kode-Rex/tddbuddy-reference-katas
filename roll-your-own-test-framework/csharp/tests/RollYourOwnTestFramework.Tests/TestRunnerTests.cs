using FluentAssertions;
using RollYourOwnTestFramework;
using Xunit;

namespace RollYourOwnTestFramework.Tests;

public class TestRunnerTests
{
    // --- Discovery ---

    [Fact]
    public void Empty_suite_discovers_zero_tests()
    {
        var suite = new TestSuiteBuilder().Build();

        var summary = TestRunner.RunAll(suite);

        summary.Run.Should().Be(0);
    }

    [Fact]
    public void Suite_with_three_tests_discovers_and_runs_all_three()
    {
        var suite = new TestSuiteBuilder()
            .WithPassingTest("Test1")
            .WithPassingTest("Test2")
            .WithPassingTest("Test3")
            .Build();

        var summary = TestRunner.RunAll(suite);

        summary.Run.Should().Be(3);
    }

    // --- Execution and Reporting ---

    [Fact]
    public void Passing_test_is_reported_as_pass()
    {
        var suite = new TestSuiteBuilder()
            .WithPassingTest("MyTest")
            .Build();

        var summary = TestRunner.RunAll(suite);

        summary.Results.Should().ContainSingle()
            .Which.Status.Should().Be(TestStatus.Pass);
    }

    [Fact]
    public void Failing_assertion_is_reported_as_fail_with_message()
    {
        var suite = new TestSuiteBuilder()
            .WithFailingTest("MyTest", "expected 5 but got 3")
            .Build();

        var summary = TestRunner.RunAll(suite);

        var result = summary.Results.Should().ContainSingle().Subject;
        result.Status.Should().Be(TestStatus.Fail);
        result.Message.Should().Be("expected 5 but got 3");
    }

    [Fact]
    public void Unhandled_exception_is_reported_as_error_with_exception_info()
    {
        var suite = new TestSuiteBuilder()
            .WithErroringTest("MyTest", "something went wrong")
            .Build();

        var summary = TestRunner.RunAll(suite);

        var result = summary.Results.Should().ContainSingle().Subject;
        result.Status.Should().Be(TestStatus.Error);
        result.Message.Should().Contain("something went wrong");
    }

    [Fact]
    public void Multiple_tests_with_mixed_results_produce_correct_summary_counts()
    {
        var suite = new TestSuiteBuilder()
            .WithPassingTest("Pass1")
            .WithPassingTest("Pass2")
            .WithFailingTest("Fail1")
            .WithErroringTest("Error1")
            .Build();

        var summary = TestRunner.RunAll(suite);

        summary.Run.Should().Be(4);
        summary.Passed.Should().Be(2);
        summary.Failed.Should().Be(1);
        summary.Errors.Should().Be(1);
    }
}
