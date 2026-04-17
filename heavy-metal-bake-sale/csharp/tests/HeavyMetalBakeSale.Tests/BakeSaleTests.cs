using FluentAssertions;

namespace HeavyMetalBakeSale.Tests;

public class BakeSaleTests
{
    // --- Product Setup ---

    [Fact]
    public void A_product_has_a_name_price_purchase_code_and_stock_quantity()
    {
        var product = new ProductBuilder()
            .WithName("Brownie")
            .WithPrice(0.75m)
            .WithPurchaseCode("B")
            .WithStock(48)
            .Build();

        product.Name.Should().Be("Brownie");
        product.Price.Should().Be(new Money(0.75m));
        product.PurchaseCode.Should().Be("B");
        product.Stock.Should().Be(48);
    }

    [Fact]
    public void Default_inventory_contains_brownie_muffin_cake_pop_and_water()
    {
        var sale = BakeSale.CreateDefault();

        sale.Inventory.Should().HaveCount(4);
        sale.Inventory.Select(p => p.Name).Should()
            .ContainInOrder("Brownie", "Muffin", "Cake Pop", "Water");
    }

    // --- Order Totals ---

    [Fact]
    public void Single_brownie_order_totals_0_75()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("B");

        total.Should().Be(new Money(0.75m));
    }

    [Fact]
    public void Single_muffin_order_totals_1_00()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("M");

        total.Should().Be(new Money(1.00m));
    }

    [Fact]
    public void Single_cake_pop_order_totals_1_35()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("C");

        total.Should().Be(new Money(1.35m));
    }

    [Fact]
    public void Single_water_order_totals_1_50()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("W");

        total.Should().Be(new Money(1.50m));
    }

    [Fact]
    public void Multiple_different_items_total_to_the_sum_of_their_prices()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("B,C,W");

        total.Should().Be(new Money(3.60m));
    }

    [Fact]
    public void Duplicate_items_in_an_order_are_each_counted_separately()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("B,B");

        total.Should().Be(new Money(1.50m));
    }

    // --- Stock Management ---

    [Fact]
    public void Successful_order_decrements_stock_for_each_purchased_item()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        sale.CalculateTotal("B,M");

        sale.Inventory.First(p => p.PurchaseCode == "B").Stock.Should().Be(47);
        sale.Inventory.First(p => p.PurchaseCode == "M").Stock.Should().Be(35);
    }

    [Fact]
    public void Out_of_stock_item_rejects_the_order_with_item_name()
    {
        var sale = new OrderBuilder()
            .WithProduct(new ProductBuilder().WithName("Water").WithPurchaseCode("W").WithPrice(1.50m).WithStock(0).Build())
            .Build();

        var act = () => sale.CalculateTotal("W");

        act.Should().Throw<OutOfStockException>()
            .WithMessage("Water is out of stock");
    }

    [Fact]
    public void Partially_stocked_order_where_second_item_is_out_of_stock_rejects_with_that_item_name()
    {
        var sale = new OrderBuilder()
            .WithProduct(new ProductBuilder().WithName("Brownie").WithPurchaseCode("B").WithStock(10).Build())
            .WithProduct(new ProductBuilder().WithName("Water").WithPurchaseCode("W").WithPrice(1.50m).WithStock(0).Build())
            .Build();

        var act = () => sale.CalculateTotal("B,W");

        act.Should().Throw<OutOfStockException>()
            .WithMessage("Water is out of stock");
    }

    [Fact]
    public void Order_does_not_decrement_stock_when_any_item_is_out_of_stock()
    {
        var sale = new OrderBuilder()
            .WithProduct(new ProductBuilder().WithName("Brownie").WithPurchaseCode("B").WithStock(5).Build())
            .WithProduct(new ProductBuilder().WithName("Water").WithPurchaseCode("W").WithPrice(1.50m).WithStock(0).Build())
            .Build();

        var act = () => sale.CalculateTotal("B,W");
        act.Should().Throw<OutOfStockException>();

        sale.Inventory.First(p => p.PurchaseCode == "B").Stock.Should().Be(5);
    }

    // --- Payment and Change ---

    [Fact]
    public void Exact_payment_returns_zero_change()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("B");
        var change = sale.CalculateChange(total, new Money(0.75m));

        change.Should().Be(Money.Zero);
    }

    [Fact]
    public void Overpayment_returns_correct_change()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("B,C,W");
        var change = sale.CalculateChange(total, new Money(4.00m));

        change.Should().Be(new Money(0.40m));
    }

    [Fact]
    public void Underpayment_is_rejected_with_not_enough_money()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var total = sale.CalculateTotal("C,M");
        var act = () => sale.CalculateChange(total, new Money(2.00m));

        act.Should().Throw<InsufficientPaymentException>()
            .WithMessage("Not enough money.");
    }

    // --- Edge Cases ---

    [Fact]
    public void Unknown_purchase_code_is_rejected()
    {
        var sale = new OrderBuilder().WithDefaultInventory().Build();

        var act = () => sale.CalculateTotal("X");

        act.Should().Throw<UnknownPurchaseCodeException>()
            .WithMessage("Unknown purchase code: X");
    }

    [Fact]
    public void Multiple_items_with_one_out_of_stock_reports_the_out_of_stock_item()
    {
        var sale = new OrderBuilder()
            .WithProduct(new ProductBuilder().WithName("Brownie").WithPurchaseCode("B").WithStock(10).Build())
            .WithProduct(new ProductBuilder().WithName("Muffin").WithPurchaseCode("M").WithPrice(1.00m).WithStock(0).Build())
            .Build();

        var act = () => sale.CalculateTotal("B,M");

        act.Should().Throw<OutOfStockException>()
            .WithMessage("Muffin is out of stock");
    }

    [Fact]
    public void Buying_all_remaining_stock_of_an_item_succeeds()
    {
        var sale = new OrderBuilder()
            .WithProduct(new ProductBuilder().WithName("Brownie").WithPurchaseCode("B").WithStock(1).Build())
            .Build();

        var total = sale.CalculateTotal("B");

        total.Should().Be(new Money(0.75m));
        sale.Inventory.First(p => p.PurchaseCode == "B").Stock.Should().Be(0);
    }

    [Fact]
    public void Buying_one_more_after_stock_is_depleted_fails()
    {
        var sale = new OrderBuilder()
            .WithProduct(new ProductBuilder().WithName("Brownie").WithPurchaseCode("B").WithStock(1).Build())
            .Build();

        sale.CalculateTotal("B");
        var act = () => sale.CalculateTotal("B");

        act.Should().Throw<OutOfStockException>()
            .WithMessage("Brownie is out of stock");
    }
}
