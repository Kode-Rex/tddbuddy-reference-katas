using FluentAssertions;
using Xunit;

namespace SupermarketPricing.Tests;

public class BuyOneGetOneFreeTests
{
    [Fact]
    public void Two_Cs_with_BOGOF_costs_20()
    {
        var c = new ProductBuilder().WithSku("C").Named("C").WithBuyOneGetOneFree(20).Build();
        var checkout = new CheckoutBuilder().WithScanned(c, 2).Build();

        checkout.Total().Should().Be(new Money(20));
    }

    [Fact]
    public void Three_Cs_with_BOGOF_costs_40()
    {
        var c = new ProductBuilder().WithSku("C").Named("C").WithBuyOneGetOneFree(20).Build();
        var checkout = new CheckoutBuilder().WithScanned(c, 3).Build();

        checkout.Total().Should().Be(new Money(40));
    }

    [Fact]
    public void Single_C_with_BOGOF_costs_20()
    {
        var c = new ProductBuilder().WithSku("C").Named("C").WithBuyOneGetOneFree(20).Build();
        var checkout = new CheckoutBuilder().WithScanned(c, 1).Build();

        checkout.Total().Should().Be(new Money(20));
    }
}
