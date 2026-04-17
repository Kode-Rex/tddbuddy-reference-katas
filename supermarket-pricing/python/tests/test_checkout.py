from supermarket_pricing import Checkout, Money

from .checkout_builder import CheckoutBuilder
from .product_builder import ProductBuilder


# --- Simple Pricing ---


def test_empty_checkout_has_a_zero_total():
    checkout = CheckoutBuilder().build()
    assert checkout.total() == Money.zero()


def test_scanning_a_single_item_returns_its_unit_price():
    a = ProductBuilder().with_sku("A").named("A").with_unit_price(50).build()
    checkout = CheckoutBuilder().with_scanned(a).build()
    assert checkout.total() == Money(50)


def test_scanning_two_different_items_returns_the_sum_of_their_unit_prices():
    a = ProductBuilder().with_sku("A").named("A").with_unit_price(50).build()
    b = ProductBuilder().with_sku("B").named("B").with_unit_price(30).build()
    checkout = CheckoutBuilder().with_scanned(a).with_scanned(b).build()
    assert checkout.total() == Money(80)


def test_scanning_the_same_item_twice_returns_double_its_unit_price():
    a = ProductBuilder().with_sku("A").named("A").with_unit_price(50).build()
    checkout = CheckoutBuilder().with_scanned(a, 2).build()
    assert checkout.total() == Money(100)


# --- Multi-Buy Discounts ---


def test_three_as_at_three_for_130_costs_130():
    a = ProductBuilder().with_sku("A").named("A").with_multi_buy(3, 130, 50).build()
    checkout = CheckoutBuilder().with_scanned(a, 3).build()
    assert checkout.total() == Money(130)


def test_four_as_at_three_for_130_costs_180():
    a = ProductBuilder().with_sku("A").named("A").with_multi_buy(3, 130, 50).build()
    checkout = CheckoutBuilder().with_scanned(a, 4).build()
    assert checkout.total() == Money(180)


def test_two_bs_at_two_for_45_costs_45():
    b = ProductBuilder().with_sku("B").named("B").with_multi_buy(2, 45, 30).build()
    checkout = CheckoutBuilder().with_scanned(b, 2).build()
    assert checkout.total() == Money(45)


def test_three_bs_at_two_for_45_costs_75():
    b = ProductBuilder().with_sku("B").named("B").with_multi_buy(2, 45, 30).build()
    checkout = CheckoutBuilder().with_scanned(b, 3).build()
    assert checkout.total() == Money(75)


def test_mixed_basket_with_multi_buy_discounts_totals_correctly():
    a = ProductBuilder().with_sku("A").named("A").with_multi_buy(3, 130, 50).build()
    b = ProductBuilder().with_sku("B").named("B").with_multi_buy(2, 45, 30).build()
    checkout = (
        CheckoutBuilder()
        .with_scanned(a, 3)
        .with_scanned(b, 2)
        .build()
    )
    assert checkout.total() == Money(175)


def test_scanning_order_does_not_affect_multi_buy_total():
    a = ProductBuilder().with_sku("A").named("A").with_multi_buy(3, 130, 50).build()
    b = ProductBuilder().with_sku("B").named("B").with_multi_buy(2, 45, 30).build()
    checkout = Checkout()
    checkout.scan(a)
    checkout.scan(b)
    checkout.scan(a)
    checkout.scan(b)
    checkout.scan(a)
    assert checkout.total() == Money(175)


# --- Buy One Get One Free ---


def test_two_cs_with_bogof_costs_20():
    c = ProductBuilder().with_sku("C").named("C").with_buy_one_get_one_free(20).build()
    checkout = CheckoutBuilder().with_scanned(c, 2).build()
    assert checkout.total() == Money(20)


def test_three_cs_with_bogof_costs_40():
    c = ProductBuilder().with_sku("C").named("C").with_buy_one_get_one_free(20).build()
    checkout = CheckoutBuilder().with_scanned(c, 3).build()
    assert checkout.total() == Money(40)


def test_single_c_with_bogof_costs_20():
    c = ProductBuilder().with_sku("C").named("C").with_buy_one_get_one_free(20).build()
    checkout = CheckoutBuilder().with_scanned(c, 1).build()
    assert checkout.total() == Money(20)


# --- Weighted Items ---


def test_bananas_at_199_cents_per_kg_for_half_kg_costs_100():
    bananas = ProductBuilder().with_sku("Bananas").named("Bananas").with_weighted_price(199).build()
    checkout = CheckoutBuilder().with_weighed(bananas, "0.5").build()
    assert checkout.total() == Money(100)


def test_apples_at_349_cents_per_kg_for_one_kg_costs_349():
    apples = ProductBuilder().with_sku("Apples").named("Apples").with_weighted_price(349).build()
    checkout = CheckoutBuilder().with_weighed(apples, "1.0").build()
    assert checkout.total() == Money(349)


def test_weighted_item_with_zero_weight_costs_zero():
    bananas = ProductBuilder().with_sku("Bananas").named("Bananas").with_weighted_price(199).build()
    checkout = CheckoutBuilder().with_weighed(bananas, "0").build()
    assert checkout.total() == Money.zero()


# --- Combo Deals ---


def test_d_plus_c_together_at_combo_price_costs_25():
    d = ProductBuilder().with_sku("D").named("D").with_unit_price(15).build()
    c = ProductBuilder().with_sku("C").named("C").with_unit_price(20).build()
    checkout = (
        CheckoutBuilder()
        .with_combo_deal("D", "C", 25)
        .with_scanned(d)
        .with_scanned(c)
        .build()
    )
    assert checkout.total() == Money(25)


def test_d_plus_c_plus_d_uses_combo_once_remaining_d_at_unit_price():
    d = ProductBuilder().with_sku("D").named("D").with_unit_price(15).build()
    c = ProductBuilder().with_sku("C").named("C").with_unit_price(20).build()
    checkout = (
        CheckoutBuilder()
        .with_combo_deal("D", "C", 25)
        .with_scanned(d, 2)
        .with_scanned(c)
        .build()
    )
    assert checkout.total() == Money(40)


def test_d_alone_with_a_combo_deal_configured_still_costs_unit_price():
    d = ProductBuilder().with_sku("D").named("D").with_unit_price(15).build()
    checkout = (
        CheckoutBuilder()
        .with_combo_deal("D", "C", 25)
        .with_scanned(d)
        .build()
    )
    assert checkout.total() == Money(15)


def test_c_alone_with_a_combo_deal_configured_still_costs_unit_price():
    c = ProductBuilder().with_sku("C").named("C").with_unit_price(20).build()
    checkout = (
        CheckoutBuilder()
        .with_combo_deal("D", "C", 25)
        .with_scanned(c)
        .build()
    )
    assert checkout.total() == Money(20)
