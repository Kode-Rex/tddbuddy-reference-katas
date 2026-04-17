class EmptyReportError(Exception):
    def __init__(self) -> None:
        super().__init__("Cannot submit an empty report")


class ReportExceedsMaximumError(Exception):
    def __init__(self) -> None:
        super().__init__("Report total exceeds the $5,000 maximum")


class InvalidStatusTransitionError(Exception):
    def __init__(self, message: str) -> None:
        super().__init__(message)


class ExpenseNotFoundError(Exception):
    def __init__(self) -> None:
        super().__init__("Expense not found")


class InvalidAmountError(Exception):
    def __init__(self) -> None:
        super().__init__("Amount must be positive")


class FinalizedReportError(Exception):
    def __init__(self) -> None:
        super().__init__("Cannot modify a finalized report")
