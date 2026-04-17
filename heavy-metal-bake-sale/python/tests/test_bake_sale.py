import pytest

from heavy_metal_bake_sale import (
    BakeSale,
    Money,
    OutOfStockException,
    InsufficientPaymentException,
    UnknownPurchaseCodeException,
)
from tests.product_builder import ProductBuilder
from tests.order_builder import OrderBuilder


# --- Product Setup ---


def test_a_product_has_a_name_price_purchase_code_and_stock_quantity():
    product = (
        ProductBuilder()
        .with_name("Brownie")
        .with_price(0.75)
        .with_purchase_code("B")
        .with_stock(48)
        .build()
    )

    assert product.name == "Brownie"
    assert product.price == Money(0.75)
    assert product.purchase_code == "B"
    assert product.stock == 48


def test_default_inventory_contains_brownie_muffin_cake_pop_and_water():
    sale = BakeSale.create_default()

    assert len(sale.inventory) == 4
    assert [p.name for p in sale.inventory] == [
        "Brownie", "Muffin", "Cake Pop", "Water"
    ]


# --- Order Totals ---


def test_single_brownie_order_totals_0_75():
    sale = OrderBuilder().with_default_inventory().build()
    assert sale.calculate_total("B") == Money(0.75)


def test_single_muffin_order_totals_1_00():
    sale = OrderBuilder().with_default_inventory().build()
    assert sale.calculate_total("M") == Money(1.00)


def test_single_cake_pop_order_totals_1_35():
    sale = OrderBuilder().with_default_inventory().build()
    assert sale.calculate_total("C") == Money(1.35)


def test_single_water_order_totals_1_50():
    sale = OrderBuilder().with_default_inventory().build()
    assert sale.calculate_total("W") == Money(1.50)


def test_multiple_different_items_total_to_the_sum_of_their_prices():
    sale = OrderBuilder().with_default_inventory().build()
    assert sale.calculate_total("B,C,W") == Money(3.60)


def test_duplicate_items_in_an_order_are_each_counted_separately():
    sale = OrderBuilder().with_default_inventory().build()
    assert sale.calculate_total("B,B") == Money(1.50)


# --- Stock Management ---


def test_successful_order_decrements_stock_for_each_purchased_item():
    sale = OrderBuilder().with_default_inventory().build()
    sale.calculate_total("B,M")

    brownie = next(p for p in sale.inventory if p.purchase_code == "B")
    muffin = next(p for p in sale.inventory if p.purchase_code == "M")
    assert brownie.stock == 47
    assert muffin.stock == 35


def test_out_of_stock_item_rejects_the_order_with_item_name():
    sale = (
        OrderBuilder()
        .with_product(
            ProductBuilder().with_name("Water").with_purchase_code("W").with_price(1.50).with_stock(0).build()
        )
        .build()
    )

    with pytest.raises(OutOfStockException, match="Water is out of stock"):
        sale.calculate_total("W")


def test_partially_stocked_order_where_second_item_is_out_of_stock_rejects_with_that_item_name():
    sale = (
        OrderBuilder()
        .with_product(
            ProductBuilder().with_name("Brownie").with_purchase_code("B").with_stock(10).build()
        )
        .with_product(
            ProductBuilder().with_name("Water").with_purchase_code("W").with_price(1.50).with_stock(0).build()
        )
        .build()
    )

    with pytest.raises(OutOfStockException, match="Water is out of stock"):
        sale.calculate_total("B,W")


def test_order_does_not_decrement_stock_when_any_item_is_out_of_stock():
    sale = (
        OrderBuilder()
        .with_product(
            ProductBuilder().with_name("Brownie").with_purchase_code("B").with_stock(5).build()
        )
        .with_product(
            ProductBuilder().with_name("Water").with_purchase_code("W").with_price(1.50).with_stock(0).build()
        )
        .build()
    )

    with pytest.raises(OutOfStockException):
        sale.calculate_total("B,W")

    brownie = next(p for p in sale.inventory if p.purchase_code == "B")
    assert brownie.stock == 5


# --- Payment and Change ---


def test_exact_payment_returns_zero_change():
    sale = OrderBuilder().with_default_inventory().build()
    total = sale.calculate_total("B")
    assert sale.calculate_change(total, Money(0.75)) == Money.zero()


def test_overpayment_returns_correct_change():
    sale = OrderBuilder().with_default_inventory().build()
    total = sale.calculate_total("B,C,W")
    assert sale.calculate_change(total, Money(4.00)) == Money(0.40)


def test_underpayment_is_rejected_with_not_enough_money():
    sale = OrderBuilder().with_default_inventory().build()
    total = sale.calculate_total("C,M")

    with pytest.raises(InsufficientPaymentException, match="Not enough money."):
        sale.calculate_change(total, Money(2.00))


# --- Edge Cases ---


def test_unknown_purchase_code_is_rejected():
    sale = OrderBuilder().with_default_inventory().build()

    with pytest.raises(UnknownPurchaseCodeException, match="Unknown purchase code: X"):
        sale.calculate_total("X")


def test_multiple_items_with_one_out_of_stock_reports_the_out_of_stock_item():
    sale = (
        OrderBuilder()
        .with_product(
            ProductBuilder().with_name("Brownie").with_purchase_code("B").with_stock(10).build()
        )
        .with_product(
            ProductBuilder().with_name("Muffin").with_purchase_code("M").with_price(1.00).with_stock(0).build()
        )
        .build()
    )

    with pytest.raises(OutOfStockException, match="Muffin is out of stock"):
        sale.calculate_total("B,M")


def test_buying_all_remaining_stock_of_an_item_succeeds():
    sale = (
        OrderBuilder()
        .with_product(
            ProductBuilder().with_name("Brownie").with_purchase_code("B").with_stock(1).build()
        )
        .build()
    )

    assert sale.calculate_total("B") == Money(0.75)
    brownie = next(p for p in sale.inventory if p.purchase_code == "B")
    assert brownie.stock == 0


def test_buying_one_more_after_stock_is_depleted_fails():
    sale = (
        OrderBuilder()
        .with_product(
            ProductBuilder().with_name("Brownie").with_purchase_code("B").with_stock(1).build()
        )
        .build()
    )

    sale.calculate_total("B")

    with pytest.raises(OutOfStockException, match="Brownie is out of stock"):
        sale.calculate_total("B")
