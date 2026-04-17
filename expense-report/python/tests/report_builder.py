from __future__ import annotations

from typing import Callable

from expense_report import ExpenseItem, Report

from .expense_item_builder import ExpenseItemBuilder


class ReportBuilder:
    def __init__(self) -> None:
        self._employee_name = "Alice Johnson"
        self._expenses: list[ExpenseItem] = []
        self._submit = False
        self._approve = False
        self._reject_reason: str | None = None

    def with_employee_name(self, name: str) -> ReportBuilder:
        self._employee_name = name
        return self

    def with_expense(self, item: ExpenseItem) -> ReportBuilder:
        self._expenses.append(item)
        return self

    def with_expense_from(self, configure: Callable[[ExpenseItemBuilder], None]) -> ReportBuilder:
        builder = ExpenseItemBuilder()
        configure(builder)
        self._expenses.append(builder.build())
        return self

    def submitted(self) -> ReportBuilder:
        self._submit = True
        return self

    def approved(self) -> ReportBuilder:
        self._submit = True
        self._approve = True
        return self

    def rejected(self, reason: str = "Policy violation") -> ReportBuilder:
        self._submit = True
        self._reject_reason = reason
        return self

    def build(self) -> Report:
        report = Report(self._employee_name)
        for expense in self._expenses:
            report.add_expense(expense)
        if self._submit:
            report.submit()
        if self._approve:
            report.approve()
        if self._reject_reason is not None:
            report.reject(self._reject_reason)
        return report
