from typing import Type

from roll_your_own_test_framework import AssertionFailedException


class TestSuiteBuilder:
    def __init__(self) -> None:
        self._methods: dict[str, object] = {}

    def with_passing_test(self, name: str = "test_passing") -> "TestSuiteBuilder":
        def passing(self_inner: object) -> None:
            pass

        self._methods[name] = passing
        return self

    def with_failing_test(
        self, name: str = "test_failing", message: str = "expected 5 but got 3"
    ) -> "TestSuiteBuilder":
        def failing(self_inner: object) -> None:
            raise AssertionFailedException(message)

        self._methods[name] = failing
        return self

    def with_erroring_test(
        self, name: str = "test_erroring", message: str = "something went wrong"
    ) -> "TestSuiteBuilder":
        def erroring(self_inner: object) -> None:
            raise RuntimeError(message)

        self._methods[name] = erroring
        return self

    def build(self) -> Type:
        return type("DynamicTestSuite", (), dict(self._methods))
