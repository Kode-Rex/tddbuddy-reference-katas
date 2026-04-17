from __future__ import annotations

import uuid
from datetime import datetime

from .customer import Customer
from .email_notifier import EmailNotifier
from .exceptions import DuplicateReservationError
from .machine_api import MachineApi
from .machine_selector import MachineSelector
from .pin_generator import PinGenerator
from .reservation import Reservation
from .reservation_repository import ReservationRepository
from .sms_notifier import SmsNotifier

MAX_FAILED_ATTEMPTS = 5


class ReservationService:
    def __init__(
        self,
        repository: ReservationRepository,
        email_notifier: EmailNotifier,
        sms_notifier: SmsNotifier,
        machine_api: MachineApi,
        pin_generator: PinGenerator,
        machine_selector: MachineSelector,
    ) -> None:
        self._repository = repository
        self._email_notifier = email_notifier
        self._sms_notifier = sms_notifier
        self._machine_api = machine_api
        self._pin_generator = pin_generator
        self._machine_selector = machine_selector
        self._failure_counts: dict[int, int] = {}

    def create_reservation(self, slot: datetime, customer: Customer) -> Reservation:
        existing = self._repository.find_active_by_customer_email(customer.email)
        if existing is not None:
            raise DuplicateReservationError(
                f"Customer '{customer.email}' already has an active reservation."
            )

        reservation_id = str(uuid.uuid4())
        machine_number = self._machine_selector.select_available()
        pin = self._pin_generator.generate()
        reservation = Reservation(reservation_id, slot, machine_number, customer, pin)

        self._repository.save(reservation)

        self._email_notifier.send(
            customer.email,
            "Wunda Wash Reservation Confirmation",
            f"Reservation {reservation_id}: Machine {machine_number}, PIN {pin:05d}",
        )

        self._machine_api.lock(reservation_id, machine_number, slot, pin)

        return reservation

    def claim_reservation(self, machine_number: int, pin: int) -> bool:
        reservation = self._repository.find_active_by_machine_number(machine_number)
        if reservation is None:
            return False

        if reservation.pin == pin:
            reservation.mark_used()
            self._repository.save(reservation)
            self._machine_api.unlock(machine_number, reservation.id)
            self._failure_counts.pop(machine_number, None)
            return True

        failures = self._failure_counts.get(machine_number, 0) + 1
        self._failure_counts[machine_number] = failures

        if failures >= MAX_FAILED_ATTEMPTS:
            new_pin = self._pin_generator.generate()
            reservation.update_pin(new_pin)
            self._repository.save(reservation)
            self._sms_notifier.send(
                reservation.customer.cell_phone,
                f"Your new Wunda Wash PIN is {new_pin:05d}.",
            )
            self._machine_api.lock(
                reservation.id,
                machine_number,
                reservation.slot,
                new_pin,
            )
            self._failure_counts[machine_number] = 0

        return False
