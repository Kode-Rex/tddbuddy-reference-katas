from datetime import datetime, timedelta

from laundry_reservation import MachineApi

from .recording_machine_device import RecordingMachineDevice

SLOT = datetime(2026, 4, 15, 10, 0, 0)


def test_locking_a_machine_delegates_to_the_device_with_reservation_id_date_time_and_pin():
    device = RecordingMachineDevice()
    api = MachineApi()
    api.register_device(7, device)

    api.lock("res-1", 7, SLOT, 12345)

    assert len(device.lock_calls) == 1
    call = device.lock_calls[0]
    assert call.reservation_id == "res-1"
    assert call.reservation_date_time == SLOT
    assert call.pin == 12345


def test_locking_a_machine_returns_true_when_the_device_accepts_the_lock():
    device = RecordingMachineDevice()
    device.should_accept_lock = True
    api = MachineApi()
    api.register_device(7, device)

    result = api.lock("res-1", 7, SLOT, 12345)

    assert result is True


def test_locking_a_machine_returns_false_when_the_device_rejects_the_lock():
    device = RecordingMachineDevice()
    device.should_accept_lock = False
    api = MachineApi()
    api.register_device(7, device)

    result = api.lock("res-1", 7, SLOT, 12345)

    assert result is False


def test_locking_a_machine_with_an_existing_reservation_id_updates_the_pin_and_date_time():
    device = RecordingMachineDevice()
    api = MachineApi()
    api.register_device(7, device)
    api.lock("res-1", 7, SLOT, 12345)

    new_slot = SLOT + timedelta(hours=1)
    result = api.lock("res-1", 7, new_slot, 67890)

    assert result is True
    assert len(device.lock_calls) == 2
    update_call = device.lock_calls[1]
    assert update_call.reservation_id == "res-1"
    assert update_call.reservation_date_time == new_slot
    assert update_call.pin == 67890


def test_unlocking_a_machine_delegates_to_the_device_with_the_reservation_id():
    device = RecordingMachineDevice()
    api = MachineApi()
    api.register_device(7, device)
    api.lock("res-1", 7, SLOT, 12345)

    api.unlock(7, "res-1")

    assert len(device.unlock_calls) == 1
    assert device.unlock_calls[0] == "res-1"
