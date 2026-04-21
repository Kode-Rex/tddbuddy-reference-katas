import inspect
from typing import Type

from .assertion_failed_exception import AssertionFailedException
from .test_result import TestResult, TestStatus
from .test_summary import TestSummary


class TestRunner:
    @staticmethod
    def run_all(test_class: Type) -> TestSummary:
        summary = TestSummary()
        instance = test_class()

        methods = inspect.getmembers(instance, predicate=inspect.ismethod)
        test_methods = [(name, method) for name, method in methods if name.startswith("test_")]

        for name, method in test_methods:
            try:
                method()
                summary.add(TestResult(name=name, status=TestStatus.PASS))
            except AssertionFailedException as e:
                summary.add(TestResult(name=name, status=TestStatus.FAIL, message=str(e)))
            except Exception as e:
                message = f"{type(e).__name__}: {e}"
                summary.add(TestResult(name=name, status=TestStatus.ERROR, message=message))

        return summary
