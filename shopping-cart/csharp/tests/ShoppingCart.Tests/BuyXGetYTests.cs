using FluentAssertions;
using Xunit;

namespace ShoppingCart.Tests;

public class BuyXGetYTests
{
    [Fact]
    public void Buy_two_get_one_free_charges_only_for_two_when_three_are_bought()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(3.00m)
            .WithDiscount(new BuyXGetY(buyCount: 2, freeCount: 1))
            .Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(3)).Build();

        cart.Lines[0].Subtotal().Should().Be(new Money(6.00m));
    }

    [Fact]
    public void Buy_two_get_one_free_charges_for_four_when_five_are_bought()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(3.00m)
            .WithDiscount(new BuyXGetY(buyCount: 2, freeCount: 1))
            .Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(5)).Build();

        cart.Lines[0].Subtotal().Should().Be(new Money(12.00m));
    }
}
