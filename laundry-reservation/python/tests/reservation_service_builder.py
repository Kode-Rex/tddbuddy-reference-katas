from __future__ import annotations

from laundry_reservation import MachineApi, ReservationService

from .fixed_machine_selector import FixedMachineSelector
from .fixed_pin_generator import FixedPinGenerator
from .in_memory_reservation_repository import InMemoryReservationRepository
from .recording_email_notifier import RecordingEmailNotifier
from .recording_machine_device import RecordingMachineDevice
from .recording_sms_notifier import RecordingSmsNotifier


class ReservationServiceBuilder:
    def __init__(self) -> None:
        self._machine_number = 7
        self._pins: list[int] = [12345]
        self.device = RecordingMachineDevice()
        self.email_notifier = RecordingEmailNotifier()
        self.sms_notifier = RecordingSmsNotifier()
        self.repository = InMemoryReservationRepository()

    def with_machine_number(self, n: int) -> ReservationServiceBuilder:
        self._machine_number = n
        return self

    def with_pins(self, *pins: int) -> ReservationServiceBuilder:
        self._pins = list(pins)
        return self

    def with_device_rejecting_lock(self) -> ReservationServiceBuilder:
        self.device.should_accept_lock = False
        return self

    def build(self) -> tuple[ReservationService, MachineApi]:
        machine_api = MachineApi()
        machine_api.register_device(self._machine_number, self.device)

        service = ReservationService(
            repository=self.repository,
            email_notifier=self.email_notifier,
            sms_notifier=self.sms_notifier,
            machine_api=machine_api,
            pin_generator=FixedPinGenerator(*self._pins),
            machine_selector=FixedMachineSelector(self._machine_number),
        )

        return service, machine_api
