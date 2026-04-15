using FluentAssertions;
using Xunit;

namespace ShoppingCart.Tests;

public class BulkPricingTests
{
    [Fact]
    public void Bulk_pricing_applies_the_lower_unit_price_once_the_threshold_is_reached()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(1.00m)
            .WithDiscount(new BulkPricing(threshold: new Quantity(3), bulkUnitPrice: new Money(0.75m)))
            .Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(4)).Build();

        cart.Lines[0].Subtotal().Should().Be(new Money(3.00m));
    }

    [Fact]
    public void Bulk_pricing_does_not_apply_below_the_threshold()
    {
        var apple = new ProductBuilder()
            .WithSku("APPLE")
            .PricedAt(1.00m)
            .WithDiscount(new BulkPricing(threshold: new Quantity(3), bulkUnitPrice: new Money(0.75m)))
            .Build();
        var cart = new CartBuilder().WithProduct(apple, new Quantity(2)).Build();

        cart.Lines[0].Subtotal().Should().Be(new Money(2.00m));
    }
}
