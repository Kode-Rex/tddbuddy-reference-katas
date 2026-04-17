import pytest

from expense_report import (
    Category,
    EmptyReportError,
    FinalizedReportError,
    InvalidAmountError,
    InvalidStatusTransitionError,
    Money,
    ReportExceedsMaximumError,
    ReportStatus,
    spending_policy,
)

from .expense_item_builder import ExpenseItemBuilder
from .report_builder import ReportBuilder


# --- Expense Items ---


def test_an_expense_item_within_its_category_limit_is_not_flagged_as_over_limit():
    item = ExpenseItemBuilder().as_meal(45).build()
    assert item.is_over_limit is False


def test_an_expense_item_exceeding_its_category_limit_is_flagged_as_over_limit():
    item = ExpenseItemBuilder().as_meal(60).build()
    assert item.is_over_limit is True


@pytest.mark.parametrize(
    "category,expected",
    [
        (Category.MEALS, Money(50)),
        (Category.TRAVEL, Money(500)),
        (Category.ACCOMMODATION, Money(200)),
        (Category.EQUIPMENT, Money(1000)),
        (Category.OTHER, Money(100)),
    ],
)
def test_each_category_has_the_correct_per_item_limit(category: Category, expected: Money):
    assert spending_policy.limit_for(category) == expected


# --- Creating and Adding Expenses ---


def test_a_new_report_starts_in_draft_status():
    report = ReportBuilder().build()
    assert report.status == ReportStatus.DRAFT


def test_adding_an_expense_to_a_draft_report_increases_the_total():
    report = ReportBuilder().build()
    report.add_expense(ExpenseItemBuilder().as_meal(30).build())
    assert report.total == Money(30)


def test_adding_an_expense_with_zero_amount_is_rejected():
    report = ReportBuilder().build()
    with pytest.raises(InvalidAmountError, match="Amount must be positive"):
        report.add_expense(ExpenseItemBuilder().with_amount(0).build())


def test_adding_an_expense_with_negative_amount_is_rejected():
    report = ReportBuilder().build()
    with pytest.raises(InvalidAmountError, match="Amount must be positive"):
        report.add_expense(ExpenseItemBuilder().with_amount(-10).build())


# --- Submitting ---


def test_submitting_a_draft_report_moves_it_to_pending():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .build()
    )
    report.submit()
    assert report.status == ReportStatus.PENDING


def test_submitting_an_empty_report_is_rejected():
    report = ReportBuilder().build()
    with pytest.raises(EmptyReportError, match="Cannot submit an empty report"):
        report.submit()


def test_submitting_a_report_over_5000_total_is_rejected():
    builder = ReportBuilder()
    for _ in range(6):
        builder.with_expense_from(lambda b: b.as_equipment(1000))
    report = builder.build()
    with pytest.raises(ReportExceedsMaximumError, match=r"Report total exceeds the \$5,000 maximum"):
        report.submit()


# --- Approval Rules ---


def test_a_report_under_2500_with_no_over_limit_items_does_not_require_approval():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .with_expense_from(lambda b: b.as_travel(200))
        .build()
    )
    assert report.requires_approval is False
    assert report.approval_reason == "No"


def test_a_report_with_an_over_limit_item_requires_approval():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(60))
        .build()
    )
    assert report.requires_approval is True
    assert report.approval_reason == "Yes (over-limit items)"


def test_a_report_over_2500_total_requires_approval_regardless_of_individual_items():
    builder = ReportBuilder()
    for _ in range(6):
        builder.with_expense_from(lambda b: b.as_travel(500))
    report = builder.build()
    assert report.requires_approval is True
    assert report.approval_reason == "Yes (total exceeds $2,500)"


# --- Approve and Reject ---


def test_approving_a_pending_report_moves_it_to_approved():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .submitted()
        .build()
    )
    report.approve()
    assert report.status == ReportStatus.APPROVED


def test_approving_a_non_pending_report_is_rejected():
    report = ReportBuilder().build()
    with pytest.raises(InvalidStatusTransitionError, match="Only pending reports can be approved"):
        report.approve()


def test_rejecting_a_pending_report_with_a_reason_moves_it_to_rejected():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .submitted()
        .build()
    )
    report.reject("Over budget")
    assert report.status == ReportStatus.REJECTED
    assert report.rejection_reason == "Over budget"


def test_rejecting_a_non_pending_report_is_rejected():
    report = ReportBuilder().build()
    with pytest.raises(InvalidStatusTransitionError, match="Only pending reports can be rejected"):
        report.reject("Reason")


# --- Finalized Report Constraints ---


def test_adding_an_expense_to_an_approved_report_is_rejected():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .approved()
        .build()
    )
    with pytest.raises(FinalizedReportError, match="Cannot modify a finalized report"):
        report.add_expense(ExpenseItemBuilder().as_meal(20).build())


def test_adding_an_expense_to_a_rejected_report_is_rejected():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .rejected()
        .build()
    )
    with pytest.raises(FinalizedReportError, match="Cannot modify a finalized report"):
        report.add_expense(ExpenseItemBuilder().as_meal(20).build())


# --- Reopen ---


def test_reopening_a_rejected_report_moves_it_back_to_draft():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .rejected("Policy violation")
        .build()
    )
    report.reopen()
    assert report.status == ReportStatus.DRAFT


def test_reopening_a_non_rejected_report_is_rejected():
    report = (
        ReportBuilder()
        .with_expense_from(lambda b: b.as_meal(30))
        .submitted()
        .build()
    )
    with pytest.raises(InvalidStatusTransitionError, match="Only rejected reports can be reopened"):
        report.reopen()


# --- Summary Output ---


def test_summary_lists_each_expense_with_category_description_amount_and_over_limit_flag():
    report = (
        ReportBuilder()
        .with_employee_name("Alice Johnson")
        .with_expense_from(lambda b: b.as_meal(45).with_description("Team lunch"))
        .with_expense_from(lambda b: b.as_travel(350).with_description("Flight to NYC"))
        .with_expense_from(lambda b: b.as_meal(62).with_description("Client dinner"))
        .with_expense_from(lambda b: b.as_equipment(1200).with_description("Laptop"))
        .build()
    )

    report.submit()
    summary = report.print_summary()

    assert "Expense Report: Alice Johnson" in summary
    assert "Status: Pending" in summary
    assert "Meals: Team lunch" in summary
    assert "$45.00" in summary
    assert "Travel: Flight to NYC" in summary
    assert "$350.00" in summary
    assert "Client dinner  $62.00 [OVER LIMIT]" in summary
    assert "Laptop  $1,200.00 [OVER LIMIT]" in summary
    assert "Total: $1,657.00" in summary
    assert "Requires Approval: Yes (over-limit items)" in summary
