using FluentAssertions;
using Xunit;

namespace ShoppingCart.Tests;

public class FixedDiscountTests
{
    [Fact]
    public void Fixed_discount_subtracts_a_flat_amount_from_the_line_subtotal()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(10.00m)
            .WithDiscount(new FixedOff(new Money(2.50m)))
            .Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(3)).Build();

        cart.Lines[0].Subtotal().Should().Be(new Money(27.50m));
    }

    [Fact]
    public void Fixed_discount_cannot_take_a_line_subtotal_below_zero()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(2.00m)
            .WithDiscount(new FixedOff(new Money(10.00m)))
            .Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(1)).Build();

        cart.Lines[0].Subtotal().Should().Be(Money.Zero);
    }
}
