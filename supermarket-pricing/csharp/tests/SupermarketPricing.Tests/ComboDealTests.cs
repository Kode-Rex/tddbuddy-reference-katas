using FluentAssertions;
using Xunit;

namespace SupermarketPricing.Tests;

public class ComboDealTests
{
    [Fact]
    public void D_plus_C_together_at_combo_price_costs_25()
    {
        var d = new ProductBuilder().WithSku("D").Named("D").WithUnitPrice(15).Build();
        var c = new ProductBuilder().WithSku("C").Named("C").WithUnitPrice(20).Build();
        var checkout = new CheckoutBuilder()
            .WithComboDeal("D", "C", 25)
            .WithScanned(d)
            .WithScanned(c)
            .Build();

        checkout.Total().Should().Be(new Money(25));
    }

    [Fact]
    public void D_plus_C_plus_D_uses_combo_once_remaining_D_at_unit_price()
    {
        var d = new ProductBuilder().WithSku("D").Named("D").WithUnitPrice(15).Build();
        var c = new ProductBuilder().WithSku("C").Named("C").WithUnitPrice(20).Build();
        var checkout = new CheckoutBuilder()
            .WithComboDeal("D", "C", 25)
            .WithScanned(d, 2)
            .WithScanned(c)
            .Build();

        checkout.Total().Should().Be(new Money(40));
    }

    [Fact]
    public void D_alone_with_a_combo_deal_configured_still_costs_unit_price()
    {
        var d = new ProductBuilder().WithSku("D").Named("D").WithUnitPrice(15).Build();
        var checkout = new CheckoutBuilder()
            .WithComboDeal("D", "C", 25)
            .WithScanned(d)
            .Build();

        checkout.Total().Should().Be(new Money(15));
    }

    [Fact]
    public void C_alone_with_a_combo_deal_configured_still_costs_unit_price()
    {
        var c = new ProductBuilder().WithSku("C").Named("C").WithUnitPrice(20).Build();
        var checkout = new CheckoutBuilder()
            .WithComboDeal("D", "C", 25)
            .WithScanned(c)
            .Build();

        checkout.Total().Should().Be(new Money(20));
    }
}
