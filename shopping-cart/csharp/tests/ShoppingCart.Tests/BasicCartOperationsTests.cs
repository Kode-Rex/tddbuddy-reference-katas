using FluentAssertions;
using Xunit;

namespace ShoppingCart.Tests;

public class BasicCartOperationsTests
{
    [Fact]
    public void New_cart_is_empty()
    {
        var cart = new CartBuilder().Build();

        cart.IsEmpty.Should().BeTrue();
        cart.Lines.Should().BeEmpty();
    }

    [Fact]
    public void Adding_a_product_adds_one_line_item_with_quantity_one()
    {
        var apple = new ProductBuilder().WithSku("APPLE").Named("Apple").PricedAt(1.00m).Build();
        var cart = new CartBuilder().Build();

        cart.Add(apple);

        cart.Lines.Should().ContainSingle();
        cart.Lines[0].Product.Should().BeSameAs(apple);
        cart.Lines[0].Quantity.Value.Should().Be(1);
    }

    [Fact]
    public void Adding_the_same_product_twice_increments_the_existing_lines_quantity()
    {
        var apple = new ProductBuilder().WithSku("APPLE").Build();
        var cart = new CartBuilder().WithProduct(apple).Build();

        cart.Add(apple);

        cart.Lines.Should().ContainSingle();
        cart.Lines[0].Quantity.Value.Should().Be(2);
    }

    [Fact]
    public void Removing_a_product_removes_its_line_item()
    {
        var apple = new ProductBuilder().WithSku("APPLE").Build();
        var bread = new ProductBuilder().WithSku("BREAD").Build();
        var cart = new CartBuilder().WithProduct(apple).WithProduct(bread).Build();

        cart.Remove("APPLE");

        cart.Lines.Should().ContainSingle();
        cart.Lines[0].Product.Sku.Should().Be("BREAD");
    }

    [Fact]
    public void Updating_quantity_to_a_positive_number_replaces_the_lines_quantity()
    {
        var apple = new ProductBuilder().WithSku("APPLE").Build();
        var cart = new CartBuilder().WithProduct(apple).Build();

        cart.UpdateQuantity("APPLE", 5);

        cart.Lines[0].Quantity.Value.Should().Be(5);
    }

    [Fact]
    public void Updating_quantity_to_zero_is_rejected()
    {
        var apple = new ProductBuilder().WithSku("APPLE").Build();
        var cart = new CartBuilder().WithProduct(apple).Build();

        var act = () => cart.UpdateQuantity("APPLE", 0);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Updating_quantity_to_a_negative_number_is_rejected()
    {
        var apple = new ProductBuilder().WithSku("APPLE").Build();
        var cart = new CartBuilder().WithProduct(apple).Build();

        var act = () => cart.UpdateQuantity("APPLE", -1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
