using System.Globalization;
using System.Text;

namespace ExpenseReport;

public class Report
{
    private readonly List<ExpenseItem> _expenses = new();

    public Report(string employeeName)
    {
        EmployeeName = employeeName;
    }

    public string EmployeeName { get; }
    public ReportStatus Status { get; private set; } = ReportStatus.Draft;
    public IReadOnlyList<ExpenseItem> Expenses => _expenses;
    public string? RejectionReason { get; private set; }

    public Money Total => _expenses.Aggregate(Money.Zero, (sum, e) => sum + e.Amount);

    public bool RequiresApproval =>
        _expenses.Any(e => e.IsOverLimit) || Total > SpendingPolicy.ApprovalThreshold;

    public string ApprovalReason
    {
        get
        {
            if (_expenses.Any(e => e.IsOverLimit)) return "Yes (over-limit items)";
            if (Total > SpendingPolicy.ApprovalThreshold) return "Yes (total exceeds $2,500)";
            return "No";
        }
    }

    public void AddExpense(ExpenseItem item)
    {
        if (Status == ReportStatus.Approved || Status == ReportStatus.Rejected)
            throw new FinalizedReportException();
        if (!item.Amount.IsPositive)
            throw new InvalidAmountException();
        _expenses.Add(item);
    }

    public void RemoveExpense(ExpenseItem item)
    {
        if (!_expenses.Remove(item))
            throw new ExpenseNotFoundException();
    }

    public void Submit()
    {
        if (_expenses.Count == 0)
            throw new EmptyReportException();
        if (Total > SpendingPolicy.ReportMaximum)
            throw new ReportExceedsMaximumException();
        Status = ReportStatus.Pending;
    }

    public void Approve()
    {
        if (Status != ReportStatus.Pending)
            throw new InvalidStatusTransitionException("Only pending reports can be approved");
        Status = ReportStatus.Approved;
    }

    public void Reject(string reason)
    {
        if (Status != ReportStatus.Pending)
            throw new InvalidStatusTransitionException("Only pending reports can be rejected");
        Status = ReportStatus.Rejected;
        RejectionReason = reason;
    }

    public void Reopen()
    {
        if (Status != ReportStatus.Rejected)
            throw new InvalidStatusTransitionException("Only rejected reports can be reopened");
        Status = ReportStatus.Draft;
        RejectionReason = null;
    }

    public string PrintSummary()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Expense Report: {EmployeeName}");
        sb.AppendLine($"Status: {Status}");
        sb.AppendLine();
        foreach (var e in _expenses)
        {
            var flag = e.IsOverLimit ? " [OVER LIMIT]" : "";
            var amount = e.Amount.Amount.ToString("N2", CultureInfo.InvariantCulture);
            sb.AppendLine($"  {e.Category}: {e.Description}  ${amount}{flag}");
        }
        sb.AppendLine();
        sb.AppendLine($"Total: ${Total.Amount.ToString("N2", CultureInfo.InvariantCulture)}");
        sb.Append($"Requires Approval: {ApprovalReason}");
        return sb.ToString();
    }
}
