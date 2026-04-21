from .test_result import TestResult, TestStatus


class TestSummary:
    def __init__(self) -> None:
        self._results: list[TestResult] = []

    @property
    def results(self) -> list[TestResult]:
        return list(self._results)

    @property
    def run(self) -> int:
        return len(self._results)

    @property
    def passed(self) -> int:
        return sum(1 for r in self._results if r.status == TestStatus.PASS)

    @property
    def failed(self) -> int:
        return sum(1 for r in self._results if r.status == TestStatus.FAIL)

    @property
    def errors(self) -> int:
        return sum(1 for r in self._results if r.status == TestStatus.ERROR)

    def add(self, result: TestResult) -> None:
        self._results.append(result)
