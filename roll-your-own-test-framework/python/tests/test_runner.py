from roll_your_own_test_framework import TestRunner, TestStatus
from tests.test_suite_builder import TestSuiteBuilder


class TestTestRunner:
    # --- Discovery ---

    def test_empty_suite_discovers_zero_tests(self) -> None:
        suite = TestSuiteBuilder().build()

        summary = TestRunner.run_all(suite)

        assert summary.run == 0

    def test_suite_with_three_tests_discovers_and_runs_all_three(self) -> None:
        suite = (
            TestSuiteBuilder()
            .with_passing_test("test_one")
            .with_passing_test("test_two")
            .with_passing_test("test_three")
            .build()
        )

        summary = TestRunner.run_all(suite)

        assert summary.run == 3

    # --- Execution and Reporting ---

    def test_passing_test_is_reported_as_pass(self) -> None:
        suite = TestSuiteBuilder().with_passing_test("test_my_test").build()

        summary = TestRunner.run_all(suite)

        assert len(summary.results) == 1
        assert summary.results[0].status == TestStatus.PASS

    def test_failing_assertion_is_reported_as_fail_with_message(self) -> None:
        suite = (
            TestSuiteBuilder()
            .with_failing_test("test_my_test", "expected 5 but got 3")
            .build()
        )

        summary = TestRunner.run_all(suite)

        assert len(summary.results) == 1
        assert summary.results[0].status == TestStatus.FAIL
        assert summary.results[0].message == "expected 5 but got 3"

    def test_unhandled_exception_is_reported_as_error_with_exception_info(self) -> None:
        suite = (
            TestSuiteBuilder()
            .with_erroring_test("test_my_test", "something went wrong")
            .build()
        )

        summary = TestRunner.run_all(suite)

        assert len(summary.results) == 1
        assert summary.results[0].status == TestStatus.ERROR
        assert "something went wrong" in (summary.results[0].message or "")

    def test_multiple_tests_with_mixed_results_produce_correct_summary_counts(self) -> None:
        suite = (
            TestSuiteBuilder()
            .with_passing_test("test_pass1")
            .with_passing_test("test_pass2")
            .with_failing_test("test_fail1")
            .with_erroring_test("test_error1")
            .build()
        )

        summary = TestRunner.run_all(suite)

        assert summary.run == 4
        assert summary.passed == 2
        assert summary.failed == 1
        assert summary.errors == 1
