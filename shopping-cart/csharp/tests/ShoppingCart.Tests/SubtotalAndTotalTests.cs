using FluentAssertions;
using Xunit;

namespace ShoppingCart.Tests;

public class SubtotalAndTotalTests
{
    [Fact]
    public void Line_subtotal_is_unit_price_multiplied_by_quantity()
    {
        var apple = new ProductBuilder().WithSku("APPLE").PricedAt(1.25m).Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(4)).Build();

        cart.Lines[0].Subtotal().Should().Be(new Money(5.00m));
    }

    [Fact]
    public void Cart_total_is_the_sum_of_line_subtotals()
    {
        var apple = new ProductBuilder().WithSku("APPLE").PricedAt(1.25m).Build();
        var bread = new ProductBuilder().WithSku("BREAD").PricedAt(3.00m).Build();
        var cart = new CartBuilder()
            .WithProduct(apple, new Quantity(4))
            .WithProduct(bread, new Quantity(2))
            .Build();

        cart.Total().Should().Be(new Money(11.00m));
    }

    [Fact]
    public void Cart_total_of_an_empty_cart_is_zero()
    {
        var cart = new CartBuilder().Build();

        cart.Total().Should().Be(Money.Zero);
    }
}
