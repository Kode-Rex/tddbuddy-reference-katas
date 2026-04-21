from typing import Any

from .call_record import CallRecord
from .verification_error import VerificationError


def _format_args(args: tuple[Any, ...]) -> str:
    return ", ".join(str(a) for a in args)


def _args_match(expected: tuple[Any, ...], actual: tuple[Any, ...]) -> bool:
    return expected == actual


class _StubConfiguration:
    def __init__(self, mock: "Mock", method_name: str, args: tuple[Any, ...]) -> None:
        self._mock = mock
        self._method_name = method_name
        self._args = args

    def then_return(self, value: Any) -> None:
        self._mock._add_stub(self._method_name, self._args, value)


class _MethodVerification:
    def __init__(self, mock: "Mock", method_name: str) -> None:
        self._mock = mock
        self._method_name = method_name

    def was_called(self) -> None:
        called = any(c.method_name == self._method_name for c in self._mock.calls)
        if not called:
            raise VerificationError(
                f"expected {self._method_name} to be called but was never called"
            )

    def was_called_with(self, *expected_args: Any) -> None:
        method_calls = [
            c for c in self._mock.calls if c.method_name == self._method_name
        ]
        if not method_calls:
            raise VerificationError(
                f"expected {self._method_name}({_format_args(expected_args)}) to be called but was never called"
            )
        match = any(_args_match(expected_args, c.args) for c in method_calls)
        if not match:
            actual_args = method_calls[0].args
            raise VerificationError(
                f"expected {self._method_name}({_format_args(expected_args)}) to be called but was called with ({_format_args(actual_args)})"
            )

    def was_called_times(self, n: int) -> None:
        actual_count = sum(
            1 for c in self._mock.calls if c.method_name == self._method_name
        )
        if actual_count != n:
            raise VerificationError(
                f"expected {self._method_name} to be called {n} times but was called {actual_count} times"
            )


class _WhenProxy:
    def __init__(self, mock: "Mock") -> None:
        self._mock = mock

    def __getattr__(self, name: str) -> Any:
        def capture_args(*args: Any) -> _StubConfiguration:
            return _StubConfiguration(self._mock, name, args)

        return capture_args


class _VerifyProxy:
    def __init__(self, mock: "Mock") -> None:
        self._mock = mock

    def __getattr__(self, name: str) -> _MethodVerification:
        return _MethodVerification(self._mock, name)


class Mock:
    def __init__(self) -> None:
        self._calls: list[CallRecord] = []
        self._stubs: dict[str, list[tuple[tuple[Any, ...], Any]]] = {}

    @property
    def calls(self) -> list[CallRecord]:
        return self._calls

    def __getattr__(self, name: str) -> Any:
        if name.startswith("_"):
            raise AttributeError(name)

        def method_proxy(*args: Any) -> Any:
            self._calls.append(CallRecord(method_name=name, args=args))
            stubs = self._stubs.get(name, [])
            for stub_args, return_value in stubs:
                if _args_match(stub_args, args):
                    return return_value
            return None

        return method_proxy

    def _add_stub(
        self, method_name: str, args: tuple[Any, ...], return_value: Any
    ) -> None:
        if method_name not in self._stubs:
            self._stubs[method_name] = []
        self._stubs[method_name].append((args, return_value))


def create_mock() -> Mock:
    return Mock()


def when(mock: Mock) -> _WhenProxy:
    return _WhenProxy(mock)


def verify(mock: Mock) -> _VerifyProxy:
    return _VerifyProxy(mock)
