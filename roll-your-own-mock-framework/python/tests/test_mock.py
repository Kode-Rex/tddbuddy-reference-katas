import pytest

from roll_your_own_mock_framework import create_mock, when, verify, VerificationError


class TestMockFramework:
    # --- Mock Creation ---

    def test_create_mock_methods_are_callable_without_error(self) -> None:
        mock = create_mock()

        mock.add(2, 3)  # should not raise

    def test_unstubbed_method_returns_none(self) -> None:
        mock = create_mock()

        result = mock.add(2, 3)

        assert result is None

    # --- Stub Configuration ---

    def test_stub_return_value_for_matching_args(self) -> None:
        mock = create_mock()
        when(mock).add(2, 3).then_return(5)

        result = mock.add(2, 3)

        assert result == 5

    def test_stub_different_arg_sets_return_their_own_values(self) -> None:
        mock = create_mock()
        when(mock).add(2, 3).then_return(5)
        when(mock).add(1, 1).then_return(2)

        assert mock.add(2, 3) == 5
        assert mock.add(1, 1) == 2

    def test_unstubbed_args_return_none_even_when_other_args_are_stubbed(self) -> None:
        mock = create_mock()
        when(mock).add(2, 3).then_return(5)

        result = mock.add(9, 9)

        assert result is None

    # --- Verification — wasCalled ---

    def test_verify_called_method_passes(self) -> None:
        mock = create_mock()
        mock.add(2, 3)

        verify(mock).add.was_called()  # should not raise

    def test_verify_uncalled_method_fails_with_message(self) -> None:
        mock = create_mock()

        with pytest.raises(VerificationError, match="expected add to be called but was never called"):
            verify(mock).add.was_called()

    # --- Verification — wasCalledWith ---

    def test_verify_called_with_correct_args_passes(self) -> None:
        mock = create_mock()
        mock.add(2, 3)

        verify(mock).add.was_called_with(2, 3)  # should not raise

    def test_verify_called_with_wrong_args_fails_with_message(self) -> None:
        mock = create_mock()
        mock.add(2, 3)

        with pytest.raises(
            VerificationError,
            match=r"expected add\(1, 1\) to be called but was called with \(2, 3\)",
        ):
            verify(mock).add.was_called_with(1, 1)

    def test_verify_was_called_with_on_uncalled_method_fails(self) -> None:
        mock = create_mock()

        with pytest.raises(
            VerificationError,
            match=r"expected add\(1, 1\) to be called but was never called",
        ):
            verify(mock).add.was_called_with(1, 1)

    # --- Verification — wasCalledTimes ---

    def test_verify_call_count_matches(self) -> None:
        mock = create_mock()
        mock.add(2, 3)
        mock.add(2, 3)

        verify(mock).add.was_called_times(2)  # should not raise

    def test_verify_call_count_mismatch_fails_with_message(self) -> None:
        mock = create_mock()
        mock.add(2, 3)

        with pytest.raises(
            VerificationError,
            match="expected add to be called 2 times but was called 1 times",
        ):
            verify(mock).add.was_called_times(2)
