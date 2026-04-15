import pytest

from shopping_cart import BulkPricing, BuyXGetY, FixedOff, Money, PercentOff, Quantity

from .cart_builder import CartBuilder
from .product_builder import ProductBuilder


# --- Basic Cart Operations ---

def test_new_cart_is_empty():
    cart = CartBuilder().build()
    assert cart.is_empty
    assert cart.lines == []


def test_adding_a_product_adds_one_line_item_with_quantity_one():
    apple = ProductBuilder().with_sku("APPLE").named("Apple").priced_at("1.00").build()
    cart = CartBuilder().build()
    cart.add(apple)
    assert len(cart.lines) == 1
    assert cart.lines[0].product is apple
    assert cart.lines[0].quantity.value == 1


def test_adding_the_same_product_twice_increments_the_existing_lines_quantity():
    apple = ProductBuilder().with_sku("APPLE").build()
    cart = CartBuilder().with_product(apple).build()
    cart.add(apple)
    assert len(cart.lines) == 1
    assert cart.lines[0].quantity.value == 2


def test_removing_a_product_removes_its_line_item():
    apple = ProductBuilder().with_sku("APPLE").build()
    bread = ProductBuilder().with_sku("BREAD").build()
    cart = CartBuilder().with_product(apple).with_product(bread).build()
    cart.remove("APPLE")
    assert len(cart.lines) == 1
    assert cart.lines[0].product.sku == "BREAD"


def test_updating_quantity_to_a_positive_number_replaces_the_lines_quantity():
    apple = ProductBuilder().with_sku("APPLE").build()
    cart = CartBuilder().with_product(apple).build()
    cart.update_quantity("APPLE", 5)
    assert cart.lines[0].quantity.value == 5


def test_updating_quantity_to_zero_is_rejected():
    apple = ProductBuilder().with_sku("APPLE").build()
    cart = CartBuilder().with_product(apple).build()
    with pytest.raises(ValueError):
        cart.update_quantity("APPLE", 0)


def test_updating_quantity_to_a_negative_number_is_rejected():
    apple = ProductBuilder().with_sku("APPLE").build()
    cart = CartBuilder().with_product(apple).build()
    with pytest.raises(ValueError):
        cart.update_quantity("APPLE", -1)


# --- Subtotals and Totals ---

def test_line_subtotal_is_unit_price_multiplied_by_quantity():
    apple = ProductBuilder().with_sku("APPLE").priced_at("1.25").build()
    cart = CartBuilder().with_product(apple, Quantity(4)).build()
    assert cart.lines[0].subtotal() == Money("5.00")


def test_cart_total_is_the_sum_of_line_subtotals():
    apple = ProductBuilder().with_sku("APPLE").priced_at("1.25").build()
    bread = ProductBuilder().with_sku("BREAD").priced_at("3.00").build()
    cart = (
        CartBuilder()
        .with_product(apple, Quantity(4))
        .with_product(bread, Quantity(2))
        .build()
    )
    assert cart.total() == Money("11.00")


def test_cart_total_of_an_empty_cart_is_zero():
    cart = CartBuilder().build()
    assert cart.total() == Money.zero()


# --- Percent Discount ---

def test_ten_percent_off_reduces_the_line_subtotal_by_ten_percent():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("10.00")
        .with_discount(PercentOff(10))
        .build()
    )
    cart = CartBuilder().with_product(apple, Quantity(3)).build()
    assert cart.lines[0].subtotal() == Money("27.00")


def test_percent_discount_applies_before_cart_total_is_summed():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("10.00")
        .with_discount(PercentOff(10))
        .build()
    )
    bread = ProductBuilder().with_sku("BREAD").priced_at("5.00").build()
    cart = (
        CartBuilder()
        .with_product(apple, Quantity(3))
        .with_product(bread, Quantity(2))
        .build()
    )
    assert cart.total() == Money("37.00")


# --- Fixed Discount ---

def test_fixed_discount_subtracts_a_flat_amount_from_the_line_subtotal():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("10.00")
        .with_discount(FixedOff(Money("2.50")))
        .build()
    )
    cart = CartBuilder().with_product(apple, Quantity(3)).build()
    assert cart.lines[0].subtotal() == Money("27.50")


def test_fixed_discount_cannot_take_a_line_subtotal_below_zero():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("2.00")
        .with_discount(FixedOff(Money("10.00")))
        .build()
    )
    cart = CartBuilder().with_product(apple, Quantity(1)).build()
    assert cart.lines[0].subtotal() == Money.zero()


# --- Buy X Get Y Free ---

def test_buy_two_get_one_free_charges_only_for_two_when_three_are_bought():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("3.00")
        .with_discount(BuyXGetY(buy_count=2, free_count=1))
        .build()
    )
    cart = CartBuilder().with_product(apple, Quantity(3)).build()
    assert cart.lines[0].subtotal() == Money("6.00")


def test_buy_two_get_one_free_charges_for_four_when_five_are_bought():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("3.00")
        .with_discount(BuyXGetY(buy_count=2, free_count=1))
        .build()
    )
    cart = CartBuilder().with_product(apple, Quantity(5)).build()
    assert cart.lines[0].subtotal() == Money("12.00")


# --- Bulk Pricing ---

def test_bulk_pricing_applies_the_lower_unit_price_once_the_threshold_is_reached():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("1.00")
        .with_discount(BulkPricing(threshold=Quantity(3), bulk_unit_price=Money("0.75")))
        .build()
    )
    cart = CartBuilder().with_product(apple, Quantity(4)).build()
    assert cart.lines[0].subtotal() == Money("3.00")


def test_bulk_pricing_does_not_apply_below_the_threshold():
    apple = (
        ProductBuilder()
        .with_sku("APPLE")
        .priced_at("1.00")
        .with_discount(BulkPricing(threshold=Quantity(3), bulk_unit_price=Money("0.75")))
        .build()
    )
    cart = CartBuilder().with_product(apple, Quantity(2)).build()
    assert cart.lines[0].subtotal() == Money("2.00")
