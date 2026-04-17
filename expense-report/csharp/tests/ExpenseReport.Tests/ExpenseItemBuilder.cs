namespace ExpenseReport.Tests;

public class ExpenseItemBuilder
{
    private string _description = "Office supplies";
    private decimal _amount = 25m;
    private Category _category = Category.Other;

    public ExpenseItemBuilder WithDescription(string description) { _description = description; return this; }
    public ExpenseItemBuilder WithAmount(decimal amount) { _amount = amount; return this; }
    public ExpenseItemBuilder WithCategory(Category category) { _category = category; return this; }

    public ExpenseItemBuilder AsMeal(decimal amount = 30m)
        => WithCategory(Category.Meals).WithAmount(amount).WithDescription("Team lunch");

    public ExpenseItemBuilder AsTravel(decimal amount = 200m)
        => WithCategory(Category.Travel).WithAmount(amount).WithDescription("Flight");

    public ExpenseItemBuilder AsAccommodation(decimal amount = 150m)
        => WithCategory(Category.Accommodation).WithAmount(amount).WithDescription("Hotel stay");

    public ExpenseItemBuilder AsEquipment(decimal amount = 800m)
        => WithCategory(Category.Equipment).WithAmount(amount).WithDescription("Laptop");

    public ExpenseItem Build() => new(_description, new Money(_amount), _category);
}
