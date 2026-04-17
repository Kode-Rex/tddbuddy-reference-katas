from datetime import datetime

import pytest

from laundry_reservation import Customer, DuplicateReservationError

from .reservation_service_builder import ReservationServiceBuilder

SLOT = datetime(2026, 4, 15, 10, 0, 0)
CUSTOMER = Customer("alice@example.com", "+27821234567")


def test_creating_a_reservation_assigns_a_unique_reservation_id():
    builder = ReservationServiceBuilder()
    service, _ = builder.build()

    reservation = service.create_reservation(SLOT, CUSTOMER)

    assert reservation.id


def test_creating_a_reservation_assigns_a_machine_number_from_the_selector():
    builder = ReservationServiceBuilder().with_machine_number(14)
    service, _ = builder.build()

    reservation = service.create_reservation(SLOT, CUSTOMER)

    assert reservation.machine_number == 14


def test_creating_a_reservation_assigns_a_five_digit_pin_from_the_generator():
    builder = ReservationServiceBuilder().with_pins(98765)
    service, _ = builder.build()

    reservation = service.create_reservation(SLOT, CUSTOMER)

    assert reservation.pin == 98765


def test_creating_a_reservation_sends_a_confirmation_email_with_machine_number_reservation_id_and_pin():
    builder = ReservationServiceBuilder().with_machine_number(7).with_pins(12345)
    service, _ = builder.build()

    reservation = service.create_reservation(SLOT, CUSTOMER)

    assert len(builder.email_notifier.sent) == 1
    email = builder.email_notifier.sent[0]
    assert email.to == "alice@example.com"
    assert "Machine 7" in email.body
    assert reservation.id in email.body
    assert "12345" in email.body


def test_creating_a_reservation_saves_the_reservation_to_the_repository():
    builder = ReservationServiceBuilder()
    service, _ = builder.build()

    reservation = service.create_reservation(SLOT, CUSTOMER)

    assert len(builder.repository.all) == 1
    assert builder.repository.all[0].id == reservation.id


def test_creating_a_reservation_locks_the_machine_via_the_machine_api():
    builder = ReservationServiceBuilder().with_machine_number(7).with_pins(12345)
    service, _ = builder.build()

    reservation = service.create_reservation(SLOT, CUSTOMER)

    assert len(builder.device.lock_calls) == 1
    lock_call = builder.device.lock_calls[0]
    assert lock_call.reservation_id == reservation.id
    assert lock_call.reservation_date_time == SLOT
    assert lock_call.pin == 12345


def test_creating_a_second_reservation_for_the_same_customer_is_rejected():
    builder = ReservationServiceBuilder().with_pins(12345, 67890)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    with pytest.raises(
        DuplicateReservationError,
        match="Customer 'alice@example.com' already has an active reservation.",
    ):
        service.create_reservation(datetime(2026, 4, 15, 12, 0, 0), CUSTOMER)
