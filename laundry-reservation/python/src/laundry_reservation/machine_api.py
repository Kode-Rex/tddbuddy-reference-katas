from __future__ import annotations

from datetime import datetime

from .machine_device import MachineDevice


class MachineApi:
    def __init__(self) -> None:
        self._devices: dict[int, MachineDevice] = {}
        self._reservation_to_machine: dict[str, int] = {}

    def register_device(self, machine_number: int, device: MachineDevice) -> None:
        self._devices[machine_number] = device

    def lock(
        self,
        reservation_id: str,
        machine_number: int,
        reservation_date_time: datetime,
        pin: int,
    ) -> bool:
        device = self._devices.get(machine_number)
        if device is None:
            return False

        if reservation_id in self._reservation_to_machine:
            device.lock(reservation_id, reservation_date_time, pin)
            return True

        locked = device.lock(reservation_id, reservation_date_time, pin)
        if locked:
            self._reservation_to_machine[reservation_id] = machine_number
        return locked

    def unlock(self, machine_number: int, reservation_id: str) -> None:
        device = self._devices.get(machine_number)
        if device is not None:
            device.unlock(reservation_id)
            self._reservation_to_machine.pop(reservation_id, None)
