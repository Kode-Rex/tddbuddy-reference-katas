import pytest

from robot_factory import (
    Factory,
    Money,
    OrderIncompleteError,
    PartNotAvailableError,
    PartOption,
    PartType,
    PurchasedPart,
)

from .fake_part_supplier import FakePartSupplier
from .robot_order_builder import RobotOrderBuilder
from .supplier_builder import SupplierBuilder


def _all_parts_supplier(name: str, price: float) -> FakePartSupplier:
    return (
        FakePartSupplier(name)
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, Money(price))
        .with_part(PartType.BODY, PartOption.SQUARE, Money(price))
        .with_part(PartType.ARMS, PartOption.HANDS, Money(price))
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, Money(price))
        .with_part(PartType.POWER, PartOption.SOLAR, Money(price))
    )


# --- Order Validation ---


def test_order_missing_a_part_type_is_rejected_as_incomplete():
    order = RobotOrderBuilder().without(PartType.POWER).build()
    supplier = SupplierBuilder().named("AlphaParts").build()
    factory = Factory([supplier])

    with pytest.raises(OrderIncompleteError, match="Order is missing part types: Power"):
        factory.cost_robot(order)


def test_order_with_all_five_part_types_is_accepted():
    order = RobotOrderBuilder().build()
    supplier = _all_parts_supplier("AlphaParts", 10)
    factory = Factory([supplier])

    factory.cost_robot(order)  # should not raise


# --- Costing — Single Supplier ---


def test_cost_returns_the_suppliers_price_for_each_part():
    order = RobotOrderBuilder().build()
    supplier = _all_parts_supplier("AlphaParts", 25)
    factory = Factory([supplier])

    breakdown = factory.cost_robot(order)

    assert len(breakdown.parts) == 5
    assert all(q.price == Money(25) for q in breakdown.parts)


def test_cost_total_is_the_sum_of_all_part_prices():
    order = RobotOrderBuilder().build()
    supplier = _all_parts_supplier("AlphaParts", 20)
    factory = Factory([supplier])

    breakdown = factory.cost_robot(order)

    assert breakdown.total == Money(100)


# --- Costing — Multiple Suppliers ---


def test_cost_selects_the_cheapest_quote_when_two_suppliers_carry_the_same_part():
    order = RobotOrderBuilder().build()
    expensive = _all_parts_supplier("Expensive", 50)
    cheap = _all_parts_supplier("Cheap", 10)
    medium = _all_parts_supplier("Medium", 30)
    factory = Factory([expensive, cheap, medium])

    breakdown = factory.cost_robot(order)

    assert all(q.supplier_name == "Cheap" for q in breakdown.parts)


def test_cost_selects_parts_from_different_suppliers_when_each_is_cheapest_for_different_parts():
    order = (
        RobotOrderBuilder()
        .with_head(PartOption.STANDARD_VISION)
        .with_body(PartOption.SQUARE)
        .build()
    )

    alpha = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 10)
        .with_part(PartType.BODY, PartOption.SQUARE, 50)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    beta = (
        SupplierBuilder()
        .named("Beta")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 50)
        .with_part(PartType.BODY, PartOption.SQUARE, 10)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    gamma = (
        SupplierBuilder()
        .named("Gamma")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 30)
        .with_part(PartType.BODY, PartOption.SQUARE, 30)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    factory = Factory([alpha, beta, gamma])

    breakdown = factory.cost_robot(order)

    head = next(p for p in breakdown.parts if p.type == PartType.HEAD)
    body = next(p for p in breakdown.parts if p.type == PartType.BODY)
    assert head.supplier_name == "Alpha"
    assert body.supplier_name == "Beta"


def test_cost_breakdown_shows_the_winning_supplier_for_each_part():
    order = RobotOrderBuilder().build()
    alpha = _all_parts_supplier("Alpha", 15)
    beta = _all_parts_supplier("Beta", 25)
    gamma = _all_parts_supplier("Gamma", 35)
    factory = Factory([alpha, beta, gamma])

    breakdown = factory.cost_robot(order)

    assert all(q.supplier_name == "Alpha" for q in breakdown.parts)


# --- Costing — Partial Catalogs ---


def test_cost_succeeds_when_a_part_is_available_from_only_one_of_three_suppliers():
    order = RobotOrderBuilder().with_head(PartOption.NIGHT_VISION).build()

    alpha = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.HEAD, PartOption.NIGHT_VISION, 100)
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    beta = (
        SupplierBuilder()
        .named("Beta")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    gamma = (
        SupplierBuilder()
        .named("Gamma")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    factory = Factory([alpha, beta, gamma])

    breakdown = factory.cost_robot(order)

    head = next(p for p in breakdown.parts if p.type == PartType.HEAD)
    assert head.supplier_name == "Alpha"


def test_cost_fails_with_part_not_available_when_no_supplier_carries_a_required_part():
    order = RobotOrderBuilder().with_head(PartOption.INFRARED_VISION).build()

    alpha = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    beta = (
        SupplierBuilder()
        .named("Beta")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    gamma = (
        SupplierBuilder()
        .named("Gamma")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    factory = Factory([alpha, beta, gamma])

    with pytest.raises(
        PartNotAvailableError, match="Part not available: InfraredVision"
    ):
        factory.cost_robot(order)


# --- Costing — Edge Cases ---


def test_cost_with_identical_prices_from_two_suppliers_picks_either():
    order = RobotOrderBuilder().build()
    alpha = _all_parts_supplier("Alpha", 20)
    beta = _all_parts_supplier("Beta", 20)
    gamma = _all_parts_supplier("Gamma", 30)
    factory = Factory([alpha, beta, gamma])

    breakdown = factory.cost_robot(order)

    assert all(
        q.supplier_name in ("Alpha", "Beta") for q in breakdown.parts
    )
    assert breakdown.total == Money(100)


def test_cost_with_three_suppliers_each_cheapest_for_different_parts():
    order = RobotOrderBuilder().build()

    alpha = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 5)
        .with_part(PartType.BODY, PartOption.SQUARE, 50)
        .with_part(PartType.ARMS, PartOption.HANDS, 50)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 10)
        .with_part(PartType.POWER, PartOption.SOLAR, 50)
        .build()
    )

    beta = (
        SupplierBuilder()
        .named("Beta")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 50)
        .with_part(PartType.BODY, PartOption.SQUARE, 5)
        .with_part(PartType.ARMS, PartOption.HANDS, 50)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 50)
        .with_part(PartType.POWER, PartOption.SOLAR, 10)
        .build()
    )

    gamma = (
        SupplierBuilder()
        .named("Gamma")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 50)
        .with_part(PartType.BODY, PartOption.SQUARE, 50)
        .with_part(PartType.ARMS, PartOption.HANDS, 5)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 50)
        .with_part(PartType.POWER, PartOption.SOLAR, 50)
        .build()
    )

    factory = Factory([alpha, beta, gamma])

    breakdown = factory.cost_robot(order)

    by_type = {q.type: q for q in breakdown.parts}
    assert by_type[PartType.HEAD].supplier_name == "Alpha"
    assert by_type[PartType.BODY].supplier_name == "Beta"
    assert by_type[PartType.ARMS].supplier_name == "Gamma"
    assert by_type[PartType.MOVEMENT].supplier_name == "Alpha"
    assert by_type[PartType.POWER].supplier_name == "Beta"
    assert breakdown.total == Money(35)


# --- Purchasing ---


def test_purchase_calls_the_winning_suppliers_purchase_method_for_each_part():
    order = RobotOrderBuilder().build()
    supplier = _all_parts_supplier("AlphaParts", 10)
    filler1 = _all_parts_supplier("Filler1", 50)
    filler2 = _all_parts_supplier("Filler2", 50)
    factory = Factory([supplier, filler1, filler2])

    factory.purchase_robot(order)

    assert len(supplier.purchase_log) == 5


def test_purchase_returns_the_list_of_purchased_parts_with_their_suppliers():
    order = RobotOrderBuilder().build()
    supplier = _all_parts_supplier("AlphaParts", 10)
    filler1 = _all_parts_supplier("Filler1", 50)
    filler2 = _all_parts_supplier("Filler2", 50)
    factory = Factory([supplier, filler1, filler2])

    parts = factory.purchase_robot(order)

    assert len(parts) == 5
    assert all(p.supplier_name == "AlphaParts" for p in parts)


def test_purchase_is_rejected_when_the_order_is_incomplete():
    order = RobotOrderBuilder().without(PartType.ARMS).build()
    supplier = _all_parts_supplier("AlphaParts", 10)
    filler1 = _all_parts_supplier("Filler1", 50)
    filler2 = _all_parts_supplier("Filler2", 50)
    factory = Factory([supplier, filler1, filler2])

    with pytest.raises(OrderIncompleteError):
        factory.purchase_robot(order)


def test_purchase_fails_when_a_part_is_not_available_from_any_supplier():
    order = RobotOrderBuilder().with_head(PartOption.NIGHT_VISION).build()

    alpha = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    beta = (
        SupplierBuilder()
        .named("Beta")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    gamma = (
        SupplierBuilder()
        .named("Gamma")
        .with_part(PartType.BODY, PartOption.SQUARE, 20)
        .with_part(PartType.ARMS, PartOption.HANDS, 20)
        .with_part(PartType.MOVEMENT, PartOption.WHEELS, 20)
        .with_part(PartType.POWER, PartOption.SOLAR, 20)
        .build()
    )

    factory = Factory([alpha, beta, gamma])

    with pytest.raises(
        PartNotAvailableError, match="Part not available: NightVision"
    ):
        factory.purchase_robot(order)


# --- Supplier Behavior ---


def test_supplier_that_does_not_carry_a_part_returns_no_quote():
    supplier = SupplierBuilder().named("Empty").build()

    assert supplier.get_quote(PartType.HEAD, PartOption.STANDARD_VISION) is None


def test_supplier_returns_a_quote_for_a_part_it_carries():
    supplier = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 42)
        .build()
    )

    quote = supplier.get_quote(PartType.HEAD, PartOption.STANDARD_VISION)

    assert quote is not None
    assert quote.price == Money(42)
    assert quote.supplier_name == "Alpha"


def test_supplier_purchase_records_the_transaction():
    supplier = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.HEAD, PartOption.STANDARD_VISION, 42)
        .build()
    )

    supplier.purchase(PartType.HEAD, PartOption.STANDARD_VISION)

    assert len(supplier.purchase_log) == 1
    assert supplier.purchase_log[0] == PurchasedPart(
        type=PartType.HEAD,
        option=PartOption.STANDARD_VISION,
        price=Money(42),
        supplier_name="Alpha",
    )


# --- Full Assembly ---


def test_full_robot_with_three_suppliers_each_cheapest_for_some_parts_costs_correctly():
    order = (
        RobotOrderBuilder()
        .with_head(PartOption.INFRARED_VISION)
        .with_body(PartOption.ROUND)
        .with_arms(PartOption.BOXING_GLOVES)
        .with_movement(PartOption.TRACKS)
        .with_power(PartOption.BIOMASS)
        .build()
    )

    alpha = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.HEAD, PartOption.INFRARED_VISION, 100)
        .with_part(PartType.BODY, PartOption.ROUND, 200)
        .with_part(PartType.ARMS, PartOption.BOXING_GLOVES, 50)
        .with_part(PartType.MOVEMENT, PartOption.TRACKS, 300)
        .with_part(PartType.POWER, PartOption.BIOMASS, 150)
        .build()
    )

    beta = (
        SupplierBuilder()
        .named("Beta")
        .with_part(PartType.HEAD, PartOption.INFRARED_VISION, 80)
        .with_part(PartType.BODY, PartOption.ROUND, 250)
        .with_part(PartType.ARMS, PartOption.BOXING_GLOVES, 75)
        .with_part(PartType.MOVEMENT, PartOption.TRACKS, 200)
        .with_part(PartType.POWER, PartOption.BIOMASS, 175)
        .build()
    )

    gamma = (
        SupplierBuilder()
        .named("Gamma")
        .with_part(PartType.HEAD, PartOption.INFRARED_VISION, 120)
        .with_part(PartType.BODY, PartOption.ROUND, 150)
        .with_part(PartType.ARMS, PartOption.BOXING_GLOVES, 60)
        .with_part(PartType.MOVEMENT, PartOption.TRACKS, 350)
        .with_part(PartType.POWER, PartOption.BIOMASS, 100)
        .build()
    )

    factory = Factory([alpha, beta, gamma])

    breakdown = factory.cost_robot(order)

    by_type = {q.type: q for q in breakdown.parts}
    assert by_type[PartType.HEAD].supplier_name == "Beta"
    assert by_type[PartType.BODY].supplier_name == "Gamma"
    assert by_type[PartType.ARMS].supplier_name == "Alpha"
    assert by_type[PartType.MOVEMENT].supplier_name == "Beta"
    assert by_type[PartType.POWER].supplier_name == "Gamma"
    assert breakdown.total == Money(580)


def test_full_robot_purchased_from_mixed_suppliers_each_part_from_its_cheapest_source():
    order = (
        RobotOrderBuilder()
        .with_head(PartOption.NIGHT_VISION)
        .with_body(PartOption.RECTANGULAR)
        .with_arms(PartOption.PINCHERS)
        .with_movement(PartOption.LEGS)
        .with_power(PartOption.RECHARGEABLE_BATTERY)
        .build()
    )

    alpha = (
        SupplierBuilder()
        .named("Alpha")
        .with_part(PartType.HEAD, PartOption.NIGHT_VISION, 90)
        .with_part(PartType.BODY, PartOption.RECTANGULAR, 110)
        .with_part(PartType.ARMS, PartOption.PINCHERS, 40)
        .with_part(PartType.MOVEMENT, PartOption.LEGS, 200)
        .with_part(PartType.POWER, PartOption.RECHARGEABLE_BATTERY, 130)
        .build()
    )

    beta = (
        SupplierBuilder()
        .named("Beta")
        .with_part(PartType.HEAD, PartOption.NIGHT_VISION, 70)
        .with_part(PartType.BODY, PartOption.RECTANGULAR, 130)
        .with_part(PartType.ARMS, PartOption.PINCHERS, 55)
        .with_part(PartType.MOVEMENT, PartOption.LEGS, 160)
        .with_part(PartType.POWER, PartOption.RECHARGEABLE_BATTERY, 140)
        .build()
    )

    gamma = (
        SupplierBuilder()
        .named("Gamma")
        .with_part(PartType.HEAD, PartOption.NIGHT_VISION, 85)
        .with_part(PartType.BODY, PartOption.RECTANGULAR, 95)
        .with_part(PartType.ARMS, PartOption.PINCHERS, 60)
        .with_part(PartType.MOVEMENT, PartOption.LEGS, 180)
        .with_part(PartType.POWER, PartOption.RECHARGEABLE_BATTERY, 120)
        .build()
    )

    factory = Factory([alpha, beta, gamma])

    parts = factory.purchase_robot(order)

    by_type = {p.type: p for p in parts}
    assert by_type[PartType.HEAD].supplier_name == "Beta"
    assert by_type[PartType.BODY].supplier_name == "Gamma"
    assert by_type[PartType.ARMS].supplier_name == "Alpha"
    assert by_type[PartType.MOVEMENT].supplier_name == "Beta"
    assert by_type[PartType.POWER].supplier_name == "Gamma"

    assert len([p for p in alpha.purchase_log if p.type == PartType.ARMS]) == 1
    assert len(beta.purchase_log) == 2
    assert len(gamma.purchase_log) == 2
