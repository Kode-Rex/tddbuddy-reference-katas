using FluentAssertions;
using Xunit;

namespace ExpenseReport.Tests;

public class ReportTests
{
    // --- Expense Items ---

    [Fact]
    public void An_expense_item_within_its_category_limit_is_not_flagged_as_over_limit()
    {
        var item = new ExpenseItemBuilder().AsMeal(45m).Build();

        item.IsOverLimit.Should().BeFalse();
    }

    [Fact]
    public void An_expense_item_exceeding_its_category_limit_is_flagged_as_over_limit()
    {
        var item = new ExpenseItemBuilder().AsMeal(60m).Build();

        item.IsOverLimit.Should().BeTrue();
    }

    [Theory]
    [InlineData(Category.Meals, 50)]
    [InlineData(Category.Travel, 500)]
    [InlineData(Category.Accommodation, 200)]
    [InlineData(Category.Equipment, 1000)]
    [InlineData(Category.Other, 100)]
    public void Each_category_has_the_correct_per_item_limit(Category category, decimal expected)
    {
        SpendingPolicy.LimitFor(category).Should().Be(new Money(expected));
    }

    // --- Creating and Adding Expenses ---

    [Fact]
    public void A_new_report_starts_in_Draft_status()
    {
        var report = new ReportBuilder().Build();

        report.Status.Should().Be(ReportStatus.Draft);
    }

    [Fact]
    public void Adding_an_expense_to_a_draft_report_increases_the_total()
    {
        var report = new ReportBuilder().Build();

        report.AddExpense(new ExpenseItemBuilder().AsMeal(30m).Build());

        report.Total.Should().Be(new Money(30m));
    }

    [Fact]
    public void Adding_an_expense_with_zero_amount_is_rejected()
    {
        var report = new ReportBuilder().Build();

        var act = () => report.AddExpense(new ExpenseItemBuilder().WithAmount(0m).Build());

        act.Should().Throw<InvalidAmountException>()
            .WithMessage("Amount must be positive");
    }

    [Fact]
    public void Adding_an_expense_with_negative_amount_is_rejected()
    {
        var report = new ReportBuilder().Build();

        var act = () => report.AddExpense(new ExpenseItemBuilder().WithAmount(-10m).Build());

        act.Should().Throw<InvalidAmountException>()
            .WithMessage("Amount must be positive");
    }

    // --- Submitting ---

    [Fact]
    public void Submitting_a_draft_report_moves_it_to_Pending()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .Build();

        report.Submit();

        report.Status.Should().Be(ReportStatus.Pending);
    }

    [Fact]
    public void Submitting_an_empty_report_is_rejected()
    {
        var report = new ReportBuilder().Build();

        var act = () => report.Submit();

        act.Should().Throw<EmptyReportException>()
            .WithMessage("Cannot submit an empty report");
    }

    [Fact]
    public void Submitting_a_report_over_5000_total_is_rejected()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsEquipment(1000m))
            .WithExpense(b => b.AsEquipment(1000m))
            .WithExpense(b => b.AsEquipment(1000m))
            .WithExpense(b => b.AsEquipment(1000m))
            .WithExpense(b => b.AsEquipment(1000m))
            .WithExpense(b => b.AsEquipment(1000m))
            .Build();

        var act = () => report.Submit();

        act.Should().Throw<ReportExceedsMaximumException>()
            .WithMessage("Report total exceeds the $5,000 maximum");
    }

    // --- Approval Rules ---

    [Fact]
    public void A_report_under_2500_with_no_over_limit_items_does_not_require_approval()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .WithExpense(b => b.AsTravel(200m))
            .Build();

        report.RequiresApproval.Should().BeFalse();
        report.ApprovalReason.Should().Be("No");
    }

    [Fact]
    public void A_report_with_an_over_limit_item_requires_approval()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(60m))
            .Build();

        report.RequiresApproval.Should().BeTrue();
        report.ApprovalReason.Should().Be("Yes (over-limit items)");
    }

    [Fact]
    public void A_report_over_2500_total_requires_approval_regardless_of_individual_items()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsTravel(500m))
            .WithExpense(b => b.AsTravel(500m))
            .WithExpense(b => b.AsTravel(500m))
            .WithExpense(b => b.AsTravel(500m))
            .WithExpense(b => b.AsTravel(500m))
            .WithExpense(b => b.AsTravel(500m))
            .Build();

        report.RequiresApproval.Should().BeTrue();
        report.ApprovalReason.Should().Be("Yes (total exceeds $2,500)");
    }

    // --- Approve and Reject ---

    [Fact]
    public void Approving_a_pending_report_moves_it_to_Approved()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .Submitted()
            .Build();

        report.Approve();

        report.Status.Should().Be(ReportStatus.Approved);
    }

    [Fact]
    public void Approving_a_non_pending_report_is_rejected()
    {
        var report = new ReportBuilder().Build();

        var act = () => report.Approve();

        act.Should().Throw<InvalidStatusTransitionException>()
            .WithMessage("Only pending reports can be approved");
    }

    [Fact]
    public void Rejecting_a_pending_report_with_a_reason_moves_it_to_Rejected()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .Submitted()
            .Build();

        report.Reject("Over budget");

        report.Status.Should().Be(ReportStatus.Rejected);
        report.RejectionReason.Should().Be("Over budget");
    }

    [Fact]
    public void Rejecting_a_non_pending_report_is_rejected()
    {
        var report = new ReportBuilder().Build();

        var act = () => report.Reject("Reason");

        act.Should().Throw<InvalidStatusTransitionException>()
            .WithMessage("Only pending reports can be rejected");
    }

    // --- Finalized Report Constraints ---

    [Fact]
    public void Adding_an_expense_to_an_approved_report_is_rejected()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .Approved()
            .Build();

        var act = () => report.AddExpense(new ExpenseItemBuilder().AsMeal(20m).Build());

        act.Should().Throw<FinalizedReportException>()
            .WithMessage("Cannot modify a finalized report");
    }

    [Fact]
    public void Adding_an_expense_to_a_rejected_report_is_rejected()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .Rejected()
            .Build();

        var act = () => report.AddExpense(new ExpenseItemBuilder().AsMeal(20m).Build());

        act.Should().Throw<FinalizedReportException>()
            .WithMessage("Cannot modify a finalized report");
    }

    // --- Reopen ---

    [Fact]
    public void Reopening_a_rejected_report_moves_it_back_to_Draft()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .Rejected("Policy violation")
            .Build();

        report.Reopen();

        report.Status.Should().Be(ReportStatus.Draft);
    }

    [Fact]
    public void Reopening_a_non_rejected_report_is_rejected()
    {
        var report = new ReportBuilder()
            .WithExpense(b => b.AsMeal(30m))
            .Submitted()
            .Build();

        var act = () => report.Reopen();

        act.Should().Throw<InvalidStatusTransitionException>()
            .WithMessage("Only rejected reports can be reopened");
    }

    // --- Summary Output ---

    [Fact]
    public void Summary_lists_each_expense_with_category_description_amount_and_over_limit_flag()
    {
        var report = new ReportBuilder()
            .WithEmployeeName("Alice Johnson")
            .WithExpense(b => b.AsMeal(45m).WithDescription("Team lunch"))
            .WithExpense(b => b.AsTravel(350m).WithDescription("Flight to NYC"))
            .WithExpense(b => b.AsMeal(62m).WithDescription("Client dinner"))
            .WithExpense(b => b.AsEquipment(1200m).WithDescription("Laptop"))
            .Build();

        report.Submit();
        var summary = report.PrintSummary();

        summary.Should().Contain("Expense Report: Alice Johnson");
        summary.Should().Contain("Status: Pending");
        summary.Should().Contain("Meals: Team lunch");
        summary.Should().Contain("$45.00");
        summary.Should().Contain("Travel: Flight to NYC");
        summary.Should().Contain("$350.00");
        summary.Should().Contain("Client dinner  $62.00 [OVER LIMIT]");
        summary.Should().Contain("Laptop  $1,200.00 [OVER LIMIT]");
        summary.Should().Contain("Total: $1,657.00");
        summary.Should().Contain("Requires Approval: Yes (over-limit items)");
    }
}
