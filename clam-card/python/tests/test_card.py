from datetime import date
from decimal import Decimal

import pytest

from clam_card import UnknownStationError, Zone

from tests.card_builder import CardBuilder, RideBuilder


def a_michael_card() -> CardBuilder:
    return (
        CardBuilder()
        .on_day(date(2024, 1, 1))
        .with_zone(Zone.A, "Asterisk", "Amersham", "Aldgate", "Angel", "Anerley", "Antelope")
        .with_zone(Zone.B, "Bison", "Bugel", "Balham", "Bullhead", "Barbican")
    )


def test_one_way_zone_a_journey_is_charged_the_zone_a_single_fare():
    card = a_michael_card().build()

    ride = card.travel_from("Asterisk").to("Aldgate")

    assert ride.fare == Decimal("2.50")
    assert ride.zone == Zone.A


def test_one_way_zone_a_to_b_journey_is_charged_the_zone_b_single_fare():
    card = a_michael_card().build()

    ride = card.travel_from("Asterisk").to("Barbican")

    assert ride.fare == Decimal("3.00")
    assert ride.zone == Zone.B


def test_one_way_zone_b_to_a_journey_is_charged_the_zone_b_single_fare():
    card = a_michael_card().build()

    ride = card.travel_from("Bison").to("Asterisk")

    assert ride.fare == Decimal("3.00")
    assert ride.zone == Zone.B


def test_one_way_zone_b_journey_is_charged_the_zone_b_single_fare():
    card = a_michael_card().build()

    ride = card.travel_from("Bison").to("Barbican")

    assert ride.fare == Decimal("3.00")
    assert ride.zone == Zone.B


def test_two_single_journeys_accumulate_on_total_charged():
    card = a_michael_card().build()

    card.travel_from("Asterisk").to("Aldgate")
    card.travel_from("Asterisk").to("Balham")

    assert card.total_charged() == Decimal("5.50")
    rides = card.rides()
    assert len(rides) == 2
    assert rides[0].fare == Decimal("2.50")
    assert rides[1].fare == Decimal("3.00")


def test_zone_a_daily_cap_is_7_dollars():
    card = a_michael_card().build()

    r1 = card.travel_from("Asterisk").to("Aldgate")
    r2 = card.travel_from("Aldgate").to("Angel")
    r3 = card.travel_from("Angel").to("Antelope")
    r4 = card.travel_from("Antelope").to("Asterisk")

    assert r1.fare == Decimal("2.50")
    assert r2.fare == Decimal("2.50")
    assert r3.fare == Decimal("2.00")
    assert r4.fare == Decimal("0.00")
    assert card.total_charged() == Decimal("7.00")


def test_zone_b_daily_cap_is_8_dollars():
    card = a_michael_card().build()

    r1 = card.travel_from("Asterisk").to("Barbican")
    r2 = card.travel_from("Barbican").to("Balham")
    r3 = card.travel_from("Balham").to("Bison")
    r4 = card.travel_from("Bison").to("Asterisk")

    assert r1.fare == Decimal("3.00")
    assert r2.fare == Decimal("3.00")
    assert r3.fare == Decimal("2.00")
    assert r4.fare == Decimal("0.00")
    assert card.total_charged() == Decimal("8.00")


def test_reaching_the_zone_a_cap_does_not_affect_zone_b_fares():
    card = a_michael_card().build()

    card.travel_from("Asterisk").to("Aldgate")
    card.travel_from("Aldgate").to("Angel")
    card.travel_from("Angel").to("Antelope")
    card.travel_from("Antelope").to("Asterisk")

    next_b = card.travel_from("Asterisk").to("Barbican")

    assert next_b.fare == Decimal("3.00")


def test_reaching_the_zone_b_cap_does_not_affect_zone_a_fares():
    card = a_michael_card().build()

    card.travel_from("Asterisk").to("Barbican")
    card.travel_from("Barbican").to("Balham")
    card.travel_from("Balham").to("Bison")
    card.travel_from("Bison").to("Asterisk")

    next_a = card.travel_from("Asterisk").to("Aldgate")

    assert next_a.fare == Decimal("2.50")


def test_travelling_from_an_unknown_station_raises():
    card = a_michael_card().build()

    with pytest.raises(UnknownStationError) as info:
        card.travel_from("Moonbase")
    assert str(info.value) == "station is not on this card's network"


def test_travelling_to_an_unknown_station_raises():
    card = a_michael_card().build()

    with pytest.raises(UnknownStationError) as info:
        card.travel_from("Asterisk").to("Moonbase")
    assert str(info.value) == "station is not on this card's network"


def test_each_ride_records_its_zone_and_fare():
    card = a_michael_card().build()

    card.travel_from("Asterisk").to("Barbican")
    card.travel_from("Asterisk").to("Aldgate")

    expected_b = (
        RideBuilder()
        .from_station("Asterisk").to("Barbican")
        .charged_at(Zone.B).with_fare(Decimal("3.00"))
        .build()
    )
    expected_a = (
        RideBuilder()
        .from_station("Asterisk").to("Aldgate")
        .charged_at(Zone.A).with_fare(Decimal("2.50"))
        .build()
    )

    assert card.rides() == [expected_b, expected_a]
