from __future__ import annotations

from datetime import datetime

from .customer import Customer


class Reservation:
    def __init__(
        self,
        id: str,
        slot: datetime,
        machine_number: int,
        customer: Customer,
        pin: int,
    ) -> None:
        self._id = id
        self._slot = slot
        self._machine_number = machine_number
        self._customer = customer
        self._pin = pin
        self._is_used = False

    @property
    def id(self) -> str:
        return self._id

    @property
    def slot(self) -> datetime:
        return self._slot

    @property
    def machine_number(self) -> int:
        return self._machine_number

    @property
    def customer(self) -> Customer:
        return self._customer

    @property
    def pin(self) -> int:
        return self._pin

    @property
    def is_used(self) -> bool:
        return self._is_used

    def mark_used(self) -> None:
        self._is_used = True

    def update_pin(self, new_pin: int) -> None:
        self._pin = new_pin
