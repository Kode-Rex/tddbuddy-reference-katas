namespace ExpenseReport;

public static class SpendingPolicy
{
    private static readonly Dictionary<Category, Money> PerItemLimits = new()
    {
        { Category.Meals, new Money(50m) },
        { Category.Travel, new Money(500m) },
        { Category.Accommodation, new Money(200m) },
        { Category.Equipment, new Money(1000m) },
        { Category.Other, new Money(100m) },
    };

    public static readonly Money ReportMaximum = new(5000m);
    public static readonly Money ApprovalThreshold = new(2500m);

    public static Money LimitFor(Category category) => PerItemLimits[category];
}
