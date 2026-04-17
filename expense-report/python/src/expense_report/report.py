from __future__ import annotations

from functools import reduce

from .expense_item import ExpenseItem
from .money import Money
from .report_status import ReportStatus
from . import spending_policy
from .exceptions import (
    EmptyReportError,
    FinalizedReportError,
    InvalidAmountError,
    InvalidStatusTransitionError,
    ReportExceedsMaximumError,
    ExpenseNotFoundError,
)


class Report:
    def __init__(self, employee_name: str) -> None:
        self._employee_name = employee_name
        self._expenses: list[ExpenseItem] = []
        self._status = ReportStatus.DRAFT
        self._rejection_reason: str | None = None

    @property
    def employee_name(self) -> str:
        return self._employee_name

    @property
    def status(self) -> ReportStatus:
        return self._status

    @property
    def expenses(self) -> list[ExpenseItem]:
        return list(self._expenses)

    @property
    def rejection_reason(self) -> str | None:
        return self._rejection_reason

    @property
    def total(self) -> Money:
        return reduce(lambda s, e: s + e.amount, self._expenses, Money.zero())

    @property
    def requires_approval(self) -> bool:
        return any(e.is_over_limit for e in self._expenses) or self.total > spending_policy.APPROVAL_THRESHOLD

    @property
    def approval_reason(self) -> str:
        if any(e.is_over_limit for e in self._expenses):
            return "Yes (over-limit items)"
        if self.total > spending_policy.APPROVAL_THRESHOLD:
            return "Yes (total exceeds $2,500)"
        return "No"

    def add_expense(self, item: ExpenseItem) -> None:
        if self._status in (ReportStatus.APPROVED, ReportStatus.REJECTED):
            raise FinalizedReportError()
        if not item.amount.is_positive:
            raise InvalidAmountError()
        self._expenses.append(item)

    def remove_expense(self, item: ExpenseItem) -> None:
        try:
            self._expenses.remove(item)
        except ValueError:
            raise ExpenseNotFoundError() from None

    def submit(self) -> None:
        if not self._expenses:
            raise EmptyReportError()
        if self.total > spending_policy.REPORT_MAXIMUM:
            raise ReportExceedsMaximumError()
        self._status = ReportStatus.PENDING

    def approve(self) -> None:
        if self._status != ReportStatus.PENDING:
            raise InvalidStatusTransitionError("Only pending reports can be approved")
        self._status = ReportStatus.APPROVED

    def reject(self, reason: str) -> None:
        if self._status != ReportStatus.PENDING:
            raise InvalidStatusTransitionError("Only pending reports can be rejected")
        self._status = ReportStatus.REJECTED
        self._rejection_reason = reason

    def reopen(self) -> None:
        if self._status != ReportStatus.REJECTED:
            raise InvalidStatusTransitionError("Only rejected reports can be reopened")
        self._status = ReportStatus.DRAFT
        self._rejection_reason = None

    def print_summary(self) -> str:
        lines: list[str] = []
        lines.append(f"Expense Report: {self._employee_name}")
        lines.append(f"Status: {self._status.value}")
        lines.append("")
        for e in self._expenses:
            flag = " [OVER LIMIT]" if e.is_over_limit else ""
            amount = e.amount.format()
            lines.append(f"  {e.category.value}: {e.description}  ${amount}{flag}")
        lines.append("")
        lines.append(f"Total: ${self.total.format()}")
        lines.append(f"Requires Approval: {self.approval_reason}")
        return "\n".join(lines)
