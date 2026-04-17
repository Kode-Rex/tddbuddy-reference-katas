from __future__ import annotations

from typing import Protocol

from .reservation import Reservation


class ReservationRepository(Protocol):
    def save(self, reservation: Reservation) -> None: ...
    def find_active_by_customer_email(self, email: str) -> Reservation | None: ...
    def find_active_by_machine_number(self, machine_number: int) -> Reservation | None: ...
