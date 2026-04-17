namespace ExpenseReport;

public class ExpenseItem
{
    public ExpenseItem(string description, Money amount, Category category)
    {
        Description = description;
        Amount = amount;
        Category = category;
    }

    public string Description { get; }
    public Money Amount { get; }
    public Category Category { get; }
    public bool IsOverLimit => Amount > SpendingPolicy.LimitFor(Category);
}
