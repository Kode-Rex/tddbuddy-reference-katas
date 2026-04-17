using FluentAssertions;
using Xunit;

namespace SupermarketPricing.Tests;

public class SimplePricingTests
{
    [Fact]
    public void Empty_checkout_has_a_zero_total()
    {
        var checkout = new CheckoutBuilder().Build();

        checkout.Total().Should().Be(Money.Zero);
    }

    [Fact]
    public void Scanning_a_single_item_returns_its_unit_price()
    {
        var a = new ProductBuilder().WithSku("A").Named("A").WithUnitPrice(50).Build();
        var checkout = new CheckoutBuilder().WithScanned(a).Build();

        checkout.Total().Should().Be(new Money(50));
    }

    [Fact]
    public void Scanning_two_different_items_returns_the_sum_of_their_unit_prices()
    {
        var a = new ProductBuilder().WithSku("A").Named("A").WithUnitPrice(50).Build();
        var b = new ProductBuilder().WithSku("B").Named("B").WithUnitPrice(30).Build();
        var checkout = new CheckoutBuilder().WithScanned(a).WithScanned(b).Build();

        checkout.Total().Should().Be(new Money(80));
    }

    [Fact]
    public void Scanning_the_same_item_twice_returns_double_its_unit_price()
    {
        var a = new ProductBuilder().WithSku("A").Named("A").WithUnitPrice(50).Build();
        var checkout = new CheckoutBuilder().WithScanned(a, 2).Build();

        checkout.Total().Should().Be(new Money(100));
    }
}
