from datetime import datetime

from laundry_reservation import Customer

from .reservation_service_builder import ReservationServiceBuilder

SLOT = datetime(2026, 4, 15, 10, 0, 0)
CUSTOMER = Customer("alice@example.com", "+27821234567")


def test_claiming_with_the_correct_pin_marks_the_reservation_as_used():
    builder = ReservationServiceBuilder().with_pins(12345)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    service.claim_reservation(7, 12345)

    assert builder.repository.all[0].is_used is True


def test_claiming_with_the_correct_pin_unlocks_the_machine():
    builder = ReservationServiceBuilder().with_pins(12345)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    service.claim_reservation(7, 12345)

    assert len(builder.device.unlock_calls) == 1


def test_claiming_with_an_incorrect_pin_does_not_unlock_the_machine():
    builder = ReservationServiceBuilder().with_pins(12345)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    service.claim_reservation(7, 99999)

    assert len(builder.device.unlock_calls) == 0


def test_claiming_with_an_incorrect_pin_does_not_mark_the_reservation_as_used():
    builder = ReservationServiceBuilder().with_pins(12345)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    service.claim_reservation(7, 99999)

    assert builder.repository.all[0].is_used is False


def test_five_consecutive_incorrect_pins_sends_an_sms_with_a_new_pin():
    builder = ReservationServiceBuilder().with_pins(12345, 67890)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    for _ in range(5):
        service.claim_reservation(7, 99999)

    assert len(builder.sms_notifier.sent) == 1
    sms = builder.sms_notifier.sent[0]
    assert sms.to == "+27821234567"
    assert sms.message == "Your new Wunda Wash PIN is 67890."


def test_five_consecutive_incorrect_pins_updates_the_reservation_with_the_new_pin():
    builder = ReservationServiceBuilder().with_pins(12345, 67890)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    for _ in range(5):
        service.claim_reservation(7, 99999)

    assert builder.repository.all[0].pin == 67890


def test_five_consecutive_incorrect_pins_re_locks_the_machine_with_the_new_pin():
    builder = ReservationServiceBuilder().with_pins(12345, 67890)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    for _ in range(5):
        service.claim_reservation(7, 99999)

    # First lock call is from create_reservation, second is from the re-lock after 5 failures
    assert len(builder.device.lock_calls) == 2
    re_lock = builder.device.lock_calls[1]
    assert re_lock.pin == 67890


def test_a_successful_claim_resets_the_failure_counter():
    builder = ReservationServiceBuilder().with_pins(12345)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    for _ in range(4):
        service.claim_reservation(7, 99999)
    service.claim_reservation(7, 12345)

    assert len(builder.sms_notifier.sent) == 0


def test_the_failure_counter_resets_after_a_new_pin_is_issued_allowing_five_more_attempts():
    builder = ReservationServiceBuilder().with_pins(12345, 67890, 11111)
    service, _ = builder.build()
    service.create_reservation(SLOT, CUSTOMER)

    # First round: 5 bad PINs triggers new PIN (67890)
    for _ in range(5):
        service.claim_reservation(7, 99999)
    assert len(builder.sms_notifier.sent) == 1

    # Second round: 4 bad PINs should not trigger another SMS
    for _ in range(4):
        service.claim_reservation(7, 99999)
    assert len(builder.sms_notifier.sent) == 1

    # Fifth bad PIN of second round triggers second SMS with PIN 11111
    service.claim_reservation(7, 99999)
    assert len(builder.sms_notifier.sent) == 2
