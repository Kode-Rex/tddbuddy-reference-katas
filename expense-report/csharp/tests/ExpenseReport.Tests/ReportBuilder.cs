namespace ExpenseReport.Tests;

public class ReportBuilder
{
    private string _employeeName = "Alice Johnson";
    private readonly List<ExpenseItem> _expenses = new();
    private bool _submit;
    private bool _approve;
    private string? _rejectReason;

    public ReportBuilder WithEmployeeName(string name) { _employeeName = name; return this; }

    public ReportBuilder WithExpense(ExpenseItem item) { _expenses.Add(item); return this; }

    public ReportBuilder WithExpense(Action<ExpenseItemBuilder> configure)
    {
        var builder = new ExpenseItemBuilder();
        configure(builder);
        _expenses.Add(builder.Build());
        return this;
    }

    public ReportBuilder Submitted() { _submit = true; return this; }

    public ReportBuilder Approved() { _submit = true; _approve = true; return this; }

    public ReportBuilder Rejected(string reason = "Policy violation")
    {
        _submit = true;
        _rejectReason = reason;
        return this;
    }

    public Report Build()
    {
        var report = new Report(_employeeName);
        foreach (var expense in _expenses)
            report.AddExpense(expense);
        if (_submit)
            report.Submit();
        if (_approve)
            report.Approve();
        if (_rejectReason != null)
            report.Reject(_rejectReason);
        return report;
    }
}
