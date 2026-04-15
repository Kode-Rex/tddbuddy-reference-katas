using FluentAssertions;
using Xunit;

namespace ShoppingCart.Tests;

public class PercentDiscountTests
{
    [Fact]
    public void Ten_percent_off_reduces_the_line_subtotal_by_ten_percent()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(10.00m)
            .WithDiscount(new PercentOff(10))
            .Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(3)).Build();

        cart.Lines[0].Subtotal().Should().Be(new Money(27.00m));
    }

    [Fact]
    public void Percent_discount_applies_before_cart_total_is_summed()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(10.00m)
            .WithDiscount(new PercentOff(10))
            .Build();
        var bread = new ProductBuilder().WithSku("BREAD").PricedAt(5.00m).Build();
        var cart = new CartBuilder()
            .WithProduct(apple, new Quantity(3))
            .WithProduct(bread, new Quantity(2))
            .Build();

        cart.Total().Should().Be(new Money(37.00m));
    }
}
