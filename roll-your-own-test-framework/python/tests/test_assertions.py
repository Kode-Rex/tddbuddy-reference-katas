import pytest

from roll_your_own_test_framework import (
    AssertionFailedException,
    assert_equal,
    assert_true,
    assert_throws,
)


class TestAssertions:
    # --- assert_equal ---

    def test_assert_equal_with_equal_values_passes(self) -> None:
        assert_equal(5, 5)  # should not raise

    def test_assert_equal_with_different_values_fails(self) -> None:
        with pytest.raises(AssertionFailedException, match="expected 5 but got 3"):
            assert_equal(5, 3)

    # --- assert_true ---

    def test_assert_true_with_true_passes(self) -> None:
        assert_true(True)  # should not raise

    def test_assert_true_with_false_fails(self) -> None:
        with pytest.raises(AssertionFailedException, match="expected true"):
            assert_true(False)

    # --- assert_throws ---

    def test_assert_throws_with_throwing_function_passes(self) -> None:
        def thrower() -> None:
            raise RuntimeError("boom")

        assert_throws(thrower)  # should not raise

    def test_assert_throws_with_non_throwing_function_fails(self) -> None:
        with pytest.raises(AssertionFailedException, match="expected exception"):
            assert_throws(lambda: None)
