using FluentAssertions;
using Xunit;

namespace SupermarketPricing.Tests;

public class MultiBuyTests
{
    [Fact]
    public void Three_As_at_three_for_130_costs_130()
    {
        var a = new ProductBuilder().WithSku("A").Named("A").WithMultiBuy(3, 130, 50).Build();
        var checkout = new CheckoutBuilder().WithScanned(a, 3).Build();

        checkout.Total().Should().Be(new Money(130));
    }

    [Fact]
    public void Four_As_at_three_for_130_costs_180()
    {
        var a = new ProductBuilder().WithSku("A").Named("A").WithMultiBuy(3, 130, 50).Build();
        var checkout = new CheckoutBuilder().WithScanned(a, 4).Build();

        checkout.Total().Should().Be(new Money(180));
    }

    [Fact]
    public void Two_Bs_at_two_for_45_costs_45()
    {
        var b = new ProductBuilder().WithSku("B").Named("B").WithMultiBuy(2, 45, 30).Build();
        var checkout = new CheckoutBuilder().WithScanned(b, 2).Build();

        checkout.Total().Should().Be(new Money(45));
    }

    [Fact]
    public void Three_Bs_at_two_for_45_costs_75()
    {
        var b = new ProductBuilder().WithSku("B").Named("B").WithMultiBuy(2, 45, 30).Build();
        var checkout = new CheckoutBuilder().WithScanned(b, 3).Build();

        checkout.Total().Should().Be(new Money(75));
    }

    [Fact]
    public void Mixed_basket_with_multi_buy_discounts_totals_correctly()
    {
        var a = new ProductBuilder().WithSku("A").Named("A").WithMultiBuy(3, 130, 50).Build();
        var b = new ProductBuilder().WithSku("B").Named("B").WithMultiBuy(2, 45, 30).Build();
        var checkout = new CheckoutBuilder()
            .WithScanned(a, 3)
            .WithScanned(b, 2)
            .Build();

        checkout.Total().Should().Be(new Money(175));
    }

    [Fact]
    public void Scanning_order_does_not_affect_multi_buy_total()
    {
        var a = new ProductBuilder().WithSku("A").Named("A").WithMultiBuy(3, 130, 50).Build();
        var b = new ProductBuilder().WithSku("B").Named("B").WithMultiBuy(2, 45, 30).Build();

        // Scan in interleaved order: A, B, A, B, A
        var checkout = new Checkout();
        checkout.Scan(a);
        checkout.Scan(b);
        checkout.Scan(a);
        checkout.Scan(b);
        checkout.Scan(a);

        checkout.Total().Should().Be(new Money(175));
    }
}
