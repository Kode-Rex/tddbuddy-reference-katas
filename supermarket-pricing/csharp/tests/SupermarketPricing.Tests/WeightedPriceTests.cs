using FluentAssertions;
using Xunit;

namespace SupermarketPricing.Tests;

public class WeightedPriceTests
{
    [Fact]
    public void Bananas_at_199_cents_per_kg_for_half_kg_costs_100()
    {
        var bananas = new ProductBuilder().WithSku("Bananas").Named("Bananas").WithWeightedPrice(199).Build();
        var checkout = new CheckoutBuilder().WithWeighed(bananas, 0.5m).Build();

        checkout.Total().Should().Be(new Money(100));
    }

    [Fact]
    public void Apples_at_349_cents_per_kg_for_one_kg_costs_349()
    {
        var apples = new ProductBuilder().WithSku("Apples").Named("Apples").WithWeightedPrice(349).Build();
        var checkout = new CheckoutBuilder().WithWeighed(apples, 1.0m).Build();

        checkout.Total().Should().Be(new Money(349));
    }

    [Fact]
    public void Weighted_item_with_zero_weight_costs_zero()
    {
        var bananas = new ProductBuilder().WithSku("Bananas").Named("Bananas").WithWeightedPrice(199).Build();
        var checkout = new CheckoutBuilder().WithWeighed(bananas, 0m).Build();

        checkout.Total().Should().Be(Money.Zero);
    }
}
