using FluentAssertions;
using Xunit;

namespace RobotFactory.Tests;

public class RobotFactoryTests
{
    // --- Order Validation ---

    [Fact]
    public void Order_missing_a_part_type_is_rejected_as_incomplete()
    {
        var order = new RobotOrderBuilder().Without(PartType.Power).Build();
        var supplier = new SupplierBuilder().Named("AlphaParts").Build();
        var factory = new Factory(new[] { supplier });

        var act = () => factory.CostRobot(order);

        act.Should().Throw<OrderIncompleteException>()
            .WithMessage("Order is missing part types: Power");
    }

    [Fact]
    public void Order_with_all_five_part_types_is_accepted()
    {
        var order = new RobotOrderBuilder().Build();
        var supplier = AllPartsSupplier("AlphaParts", 10m);
        var factory = new Factory(new[] { supplier });

        var act = () => factory.CostRobot(order);

        act.Should().NotThrow();
    }

    // --- Costing — Single Supplier ---

    [Fact]
    public void Cost_returns_the_suppliers_price_for_each_part()
    {
        var order = new RobotOrderBuilder().Build();
        var supplier = AllPartsSupplier("AlphaParts", 25m);
        var factory = new Factory(new[] { supplier });

        var breakdown = factory.CostRobot(order);

        breakdown.Parts.Should().HaveCount(5);
        breakdown.Parts.Should().OnlyContain(q => q.Price == new Money(25m));
    }

    [Fact]
    public void Cost_total_is_the_sum_of_all_part_prices()
    {
        var order = new RobotOrderBuilder().Build();
        var supplier = AllPartsSupplier("AlphaParts", 20m);
        var factory = new Factory(new[] { supplier });

        var breakdown = factory.CostRobot(order);

        breakdown.Total.Should().Be(new Money(100m));
    }

    // --- Costing — Multiple Suppliers ---

    [Fact]
    public void Cost_selects_the_cheapest_quote_when_two_suppliers_carry_the_same_part()
    {
        var order = new RobotOrderBuilder().Build();
        var expensive = AllPartsSupplier("Expensive", 50m);
        var cheap = AllPartsSupplier("Cheap", 10m);
        var medium = AllPartsSupplier("Medium", 30m);
        var factory = new Factory(new[] { expensive, cheap, medium });

        var breakdown = factory.CostRobot(order);

        breakdown.Parts.Should().OnlyContain(q => q.SupplierName == "Cheap");
    }

    [Fact]
    public void Cost_selects_parts_from_different_suppliers_when_each_is_cheapest_for_different_parts()
    {
        var order = new RobotOrderBuilder()
            .WithHead(PartOption.StandardVision)
            .WithBody(PartOption.Square)
            .Build();

        var alpha = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Head, PartOption.StandardVision, 10m)
            .WithPart(PartType.Body, PartOption.Square, 50m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var beta = new SupplierBuilder().Named("Beta")
            .WithPart(PartType.Head, PartOption.StandardVision, 50m)
            .WithPart(PartType.Body, PartOption.Square, 10m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var gamma = new SupplierBuilder().Named("Gamma")
            .WithPart(PartType.Head, PartOption.StandardVision, 30m)
            .WithPart(PartType.Body, PartOption.Square, 30m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var factory = new Factory(new[] { alpha, beta, gamma });

        var breakdown = factory.CostRobot(order);

        breakdown.Parts.First(p => p.Type == PartType.Head).SupplierName.Should().Be("Alpha");
        breakdown.Parts.First(p => p.Type == PartType.Body).SupplierName.Should().Be("Beta");
    }

    [Fact]
    public void Cost_breakdown_shows_the_winning_supplier_for_each_part()
    {
        var order = new RobotOrderBuilder().Build();
        var alpha = AllPartsSupplier("Alpha", 15m);
        var beta = AllPartsSupplier("Beta", 25m);
        var gamma = AllPartsSupplier("Gamma", 35m);
        var factory = new Factory(new[] { alpha, beta, gamma });

        var breakdown = factory.CostRobot(order);

        breakdown.Parts.Should().OnlyContain(q => q.SupplierName == "Alpha");
    }

    // --- Costing — Partial Catalogs ---

    [Fact]
    public void Cost_succeeds_when_a_part_is_available_from_only_one_of_three_suppliers()
    {
        var order = new RobotOrderBuilder()
            .WithHead(PartOption.NightVision)
            .Build();

        var alpha = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Head, PartOption.NightVision, 100m)
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var beta = new SupplierBuilder().Named("Beta")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var gamma = new SupplierBuilder().Named("Gamma")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var factory = new Factory(new[] { alpha, beta, gamma });

        var breakdown = factory.CostRobot(order);

        breakdown.Parts.First(p => p.Type == PartType.Head).SupplierName.Should().Be("Alpha");
    }

    [Fact]
    public void Cost_fails_with_part_not_available_when_no_supplier_carries_a_required_part()
    {
        var order = new RobotOrderBuilder()
            .WithHead(PartOption.InfraredVision)
            .Build();

        var alpha = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var beta = new SupplierBuilder().Named("Beta")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var gamma = new SupplierBuilder().Named("Gamma")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var factory = new Factory(new[] { alpha, beta, gamma });

        var act = () => factory.CostRobot(order);

        act.Should().Throw<PartNotAvailableException>()
            .WithMessage("Part not available: InfraredVision");
    }

    // --- Costing — Edge Cases ---

    [Fact]
    public void Cost_with_identical_prices_from_two_suppliers_picks_either()
    {
        var order = new RobotOrderBuilder().Build();
        var alpha = AllPartsSupplier("Alpha", 20m);
        var beta = AllPartsSupplier("Beta", 20m);
        var gamma = AllPartsSupplier("Gamma", 30m);
        var factory = new Factory(new[] { alpha, beta, gamma });

        var breakdown = factory.CostRobot(order);

        // Both Alpha and Beta are equally cheap; implementation picks the first seen
        breakdown.Parts.Should().OnlyContain(q =>
            q.SupplierName == "Alpha" || q.SupplierName == "Beta");
        breakdown.Total.Should().Be(new Money(100m));
    }

    [Fact]
    public void Cost_with_three_suppliers_each_cheapest_for_different_parts()
    {
        var order = new RobotOrderBuilder().Build();

        var alpha = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Head, PartOption.StandardVision, 5m)
            .WithPart(PartType.Body, PartOption.Square, 50m)
            .WithPart(PartType.Arms, PartOption.Hands, 50m)
            .WithPart(PartType.Movement, PartOption.Wheels, 10m)
            .WithPart(PartType.Power, PartOption.Solar, 50m)
            .Build();

        var beta = new SupplierBuilder().Named("Beta")
            .WithPart(PartType.Head, PartOption.StandardVision, 50m)
            .WithPart(PartType.Body, PartOption.Square, 5m)
            .WithPart(PartType.Arms, PartOption.Hands, 50m)
            .WithPart(PartType.Movement, PartOption.Wheels, 50m)
            .WithPart(PartType.Power, PartOption.Solar, 10m)
            .Build();

        var gamma = new SupplierBuilder().Named("Gamma")
            .WithPart(PartType.Head, PartOption.StandardVision, 50m)
            .WithPart(PartType.Body, PartOption.Square, 50m)
            .WithPart(PartType.Arms, PartOption.Hands, 5m)
            .WithPart(PartType.Movement, PartOption.Wheels, 50m)
            .WithPart(PartType.Power, PartOption.Solar, 50m)
            .Build();

        var factory = new Factory(new[] { alpha, beta, gamma });

        var breakdown = factory.CostRobot(order);

        breakdown.Parts.First(p => p.Type == PartType.Head).SupplierName.Should().Be("Alpha");
        breakdown.Parts.First(p => p.Type == PartType.Body).SupplierName.Should().Be("Beta");
        breakdown.Parts.First(p => p.Type == PartType.Arms).SupplierName.Should().Be("Gamma");
        breakdown.Parts.First(p => p.Type == PartType.Movement).SupplierName.Should().Be("Alpha");
        breakdown.Parts.First(p => p.Type == PartType.Power).SupplierName.Should().Be("Beta");
        breakdown.Total.Should().Be(new Money(35m));
    }

    // --- Purchasing ---

    [Fact]
    public void Purchase_calls_the_winning_suppliers_purchase_method_for_each_part()
    {
        var order = new RobotOrderBuilder().Build();
        var supplier = AllPartsSupplier("AlphaParts", 10m);
        var filler1 = AllPartsSupplier("Filler1", 50m);
        var filler2 = AllPartsSupplier("Filler2", 50m);
        var factory = new Factory(new[] { supplier, filler1, filler2 });

        factory.PurchaseRobot(order);

        supplier.PurchaseLog.Should().HaveCount(5);
    }

    [Fact]
    public void Purchase_returns_the_list_of_purchased_parts_with_their_suppliers()
    {
        var order = new RobotOrderBuilder().Build();
        var supplier = AllPartsSupplier("AlphaParts", 10m);
        var filler1 = AllPartsSupplier("Filler1", 50m);
        var filler2 = AllPartsSupplier("Filler2", 50m);
        var factory = new Factory(new[] { supplier, filler1, filler2 });

        var parts = factory.PurchaseRobot(order);

        parts.Should().HaveCount(5);
        parts.Should().OnlyContain(p => p.SupplierName == "AlphaParts");
    }

    [Fact]
    public void Purchase_is_rejected_when_the_order_is_incomplete()
    {
        var order = new RobotOrderBuilder().Without(PartType.Arms).Build();
        var supplier = AllPartsSupplier("AlphaParts", 10m);
        var filler1 = AllPartsSupplier("Filler1", 50m);
        var filler2 = AllPartsSupplier("Filler2", 50m);
        var factory = new Factory(new[] { supplier, filler1, filler2 });

        var act = () => factory.PurchaseRobot(order);

        act.Should().Throw<OrderIncompleteException>();
    }

    [Fact]
    public void Purchase_fails_when_a_part_is_not_available_from_any_supplier()
    {
        var order = new RobotOrderBuilder()
            .WithHead(PartOption.NightVision)
            .Build();

        var alpha = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var beta = new SupplierBuilder().Named("Beta")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var gamma = new SupplierBuilder().Named("Gamma")
            .WithPart(PartType.Body, PartOption.Square, 20m)
            .WithPart(PartType.Arms, PartOption.Hands, 20m)
            .WithPart(PartType.Movement, PartOption.Wheels, 20m)
            .WithPart(PartType.Power, PartOption.Solar, 20m)
            .Build();

        var factory = new Factory(new[] { alpha, beta, gamma });

        var act = () => factory.PurchaseRobot(order);

        act.Should().Throw<PartNotAvailableException>()
            .WithMessage("Part not available: NightVision");
    }

    // --- Supplier Behavior ---

    [Fact]
    public void Supplier_that_does_not_carry_a_part_returns_no_quote()
    {
        var supplier = new SupplierBuilder().Named("Empty").Build();

        supplier.GetQuote(PartType.Head, PartOption.StandardVision).Should().BeNull();
    }

    [Fact]
    public void Supplier_returns_a_quote_for_a_part_it_carries()
    {
        var supplier = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Head, PartOption.StandardVision, 42m)
            .Build();

        var quote = supplier.GetQuote(PartType.Head, PartOption.StandardVision);

        quote.Should().NotBeNull();
        quote!.Price.Should().Be(new Money(42m));
        quote.SupplierName.Should().Be("Alpha");
    }

    [Fact]
    public void Supplier_purchase_records_the_transaction()
    {
        var supplier = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Head, PartOption.StandardVision, 42m)
            .Build();

        supplier.Purchase(PartType.Head, PartOption.StandardVision);

        supplier.PurchaseLog.Should().ContainSingle()
            .Which.Should().Be(new PurchasedPart(
                PartType.Head, PartOption.StandardVision, new Money(42m), "Alpha"));
    }

    // --- Full Assembly ---

    [Fact]
    public void Full_robot_with_three_suppliers_each_cheapest_for_some_parts_costs_correctly()
    {
        var order = new RobotOrderBuilder()
            .WithHead(PartOption.InfraredVision)
            .WithBody(PartOption.Round)
            .WithArms(PartOption.BoxingGloves)
            .WithMovement(PartOption.Tracks)
            .WithPower(PartOption.Biomass)
            .Build();

        var alpha = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Head, PartOption.InfraredVision, 100m)
            .WithPart(PartType.Body, PartOption.Round, 200m)
            .WithPart(PartType.Arms, PartOption.BoxingGloves, 50m)
            .WithPart(PartType.Movement, PartOption.Tracks, 300m)
            .WithPart(PartType.Power, PartOption.Biomass, 150m)
            .Build();

        var beta = new SupplierBuilder().Named("Beta")
            .WithPart(PartType.Head, PartOption.InfraredVision, 80m)
            .WithPart(PartType.Body, PartOption.Round, 250m)
            .WithPart(PartType.Arms, PartOption.BoxingGloves, 75m)
            .WithPart(PartType.Movement, PartOption.Tracks, 200m)
            .WithPart(PartType.Power, PartOption.Biomass, 175m)
            .Build();

        var gamma = new SupplierBuilder().Named("Gamma")
            .WithPart(PartType.Head, PartOption.InfraredVision, 120m)
            .WithPart(PartType.Body, PartOption.Round, 150m)
            .WithPart(PartType.Arms, PartOption.BoxingGloves, 60m)
            .WithPart(PartType.Movement, PartOption.Tracks, 350m)
            .WithPart(PartType.Power, PartOption.Biomass, 100m)
            .Build();

        var factory = new Factory(new[] { alpha, beta, gamma });

        var breakdown = factory.CostRobot(order);

        // Head: Beta 80, Body: Gamma 150, Arms: Alpha 50, Movement: Beta 200, Power: Gamma 100
        breakdown.Parts.First(p => p.Type == PartType.Head).SupplierName.Should().Be("Beta");
        breakdown.Parts.First(p => p.Type == PartType.Body).SupplierName.Should().Be("Gamma");
        breakdown.Parts.First(p => p.Type == PartType.Arms).SupplierName.Should().Be("Alpha");
        breakdown.Parts.First(p => p.Type == PartType.Movement).SupplierName.Should().Be("Beta");
        breakdown.Parts.First(p => p.Type == PartType.Power).SupplierName.Should().Be("Gamma");
        breakdown.Total.Should().Be(new Money(580m));
    }

    [Fact]
    public void Full_robot_purchased_from_mixed_suppliers_each_part_from_its_cheapest_source()
    {
        var order = new RobotOrderBuilder()
            .WithHead(PartOption.NightVision)
            .WithBody(PartOption.Rectangular)
            .WithArms(PartOption.Pinchers)
            .WithMovement(PartOption.Legs)
            .WithPower(PartOption.RechargeableBattery)
            .Build();

        var alpha = new SupplierBuilder().Named("Alpha")
            .WithPart(PartType.Head, PartOption.NightVision, 90m)
            .WithPart(PartType.Body, PartOption.Rectangular, 110m)
            .WithPart(PartType.Arms, PartOption.Pinchers, 40m)
            .WithPart(PartType.Movement, PartOption.Legs, 200m)
            .WithPart(PartType.Power, PartOption.RechargeableBattery, 130m)
            .Build();

        var beta = new SupplierBuilder().Named("Beta")
            .WithPart(PartType.Head, PartOption.NightVision, 70m)
            .WithPart(PartType.Body, PartOption.Rectangular, 130m)
            .WithPart(PartType.Arms, PartOption.Pinchers, 55m)
            .WithPart(PartType.Movement, PartOption.Legs, 160m)
            .WithPart(PartType.Power, PartOption.RechargeableBattery, 140m)
            .Build();

        var gamma = new SupplierBuilder().Named("Gamma")
            .WithPart(PartType.Head, PartOption.NightVision, 85m)
            .WithPart(PartType.Body, PartOption.Rectangular, 95m)
            .WithPart(PartType.Arms, PartOption.Pinchers, 60m)
            .WithPart(PartType.Movement, PartOption.Legs, 180m)
            .WithPart(PartType.Power, PartOption.RechargeableBattery, 120m)
            .Build();

        var factory = new Factory(new[] { alpha, beta, gamma });

        var parts = factory.PurchaseRobot(order);

        // Head: Beta 70, Body: Gamma 95, Arms: Alpha 40, Movement: Beta 160, Power: Gamma 120
        parts.First(p => p.Type == PartType.Head).SupplierName.Should().Be("Beta");
        parts.First(p => p.Type == PartType.Body).SupplierName.Should().Be("Gamma");
        parts.First(p => p.Type == PartType.Arms).SupplierName.Should().Be("Alpha");
        parts.First(p => p.Type == PartType.Movement).SupplierName.Should().Be("Beta");
        parts.First(p => p.Type == PartType.Power).SupplierName.Should().Be("Gamma");

        alpha.PurchaseLog.Should().ContainSingle(p => p.Type == PartType.Arms);
        beta.PurchaseLog.Should().HaveCount(2);
        gamma.PurchaseLog.Should().HaveCount(2);
    }

    // --- Helper ---

    private static FakePartSupplier AllPartsSupplier(string name, decimal price)
    {
        return new FakePartSupplier(name)
            .WithPart(PartType.Head, PartOption.StandardVision, new Money(price))
            .WithPart(PartType.Body, PartOption.Square, new Money(price))
            .WithPart(PartType.Arms, PartOption.Hands, new Money(price))
            .WithPart(PartType.Movement, PartOption.Wheels, new Money(price))
            .WithPart(PartType.Power, PartOption.Solar, new Money(price));
    }
}
