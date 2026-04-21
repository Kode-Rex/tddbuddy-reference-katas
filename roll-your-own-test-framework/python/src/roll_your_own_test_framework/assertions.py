from typing import Callable

from .assertion_failed_exception import AssertionFailedException


def assert_equal(expected: object, actual: object) -> None:
    if expected != actual:
        raise AssertionFailedException(f"expected {expected} but got {actual}")


def assert_true(condition: bool) -> None:
    if not condition:
        raise AssertionFailedException("expected true")


def assert_throws(fn: Callable[[], object]) -> None:
    try:
        fn()
    except Exception:
        return
    raise AssertionFailedException("expected exception")
